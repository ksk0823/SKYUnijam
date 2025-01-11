using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

/// <summary>
/// 게임 타이틀 클래스
/// 로그인, 유저 데이터, 게임 데이터 등등을 받아오는 Scene에서 활용됨
/// </summary>
public class TitleManager : MonoBehaviourPunCallbacks
{
    [Header("Title UI")]
    public GameObject TitlePanel;

    [Header("Login UI")]
    public GameObject LoginPanel;
    public TMP_InputField PlayerNameInput;
    
    [Header("Inside Room Panel")]
    public GameObject InsideRoomPanel;
    public GameObject PlayerInfo;
    public GameObject EnemyInfo;
    public GameObject PlayerListPanel;

    public Button StartGameButton;
    public GameObject PlayerListEntryPrefab;
    
    private Dictionary<string, GameObject> roomListEntries;
    private Dictionary<int, GameObject> playerListEntries;

    [Header("Sound")]
    public AudioClip clickSound;

    private AudioSource audioSource;
    

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        audioSource = GetComponent<AudioSource>();
        
        roomListEntries = new Dictionary<string, GameObject>();
    }

    #region Photon Callbacks
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    
    public override void OnCreatedRoom()
    {
        Debug.Log("Game Room created. Waiting for players...");
    }
    
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Room creation failed: " + message);
        SetActivePanel(TitlePanel.name);
    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //Debug.LogError("Join Room failed: " + message);
        //SetActivePanel(SelectionPanel.name);
        Debug.Log("방이 없어서 새로 생성합니다.");
        
        // 랜덤 매칭 실패시 새로운 방 생성
        RoomOptions options = new RoomOptions
        {
            MaxPlayers = 2,
            IsVisible = true,
            IsOpen = true
        };
        PhotonNetwork.CreateRoom(null, options); // 랜덤한 방 이름 생성
    }
    
    public override void OnJoinedRoom()
    {
        SetActivePanel(InsideRoomPanel.name);
        
        if (playerListEntries == null)
        {
            playerListEntries = new Dictionary<int, GameObject>();
        }

        // 자신의 정보는 항상 PlayerInfo에 설정
        PlayerInfo.GetComponent<PlayerListEntry>().Initialize(PhotonNetwork.LocalPlayer.ActorNumber, PhotonNetwork.LocalPlayer.NickName);
        playerListEntries.Add(PhotonNetwork.LocalPlayer.ActorNumber, PlayerInfo);

        // 이미 방에 다른 플레이어가 있다면 EnemyInfo 설정
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                EnemyInfo.GetComponent<PlayerListEntry>().Initialize(p.ActorNumber, p.NickName);
                playerListEntries.Add(p.ActorNumber, EnemyInfo);
                
                // 기존 플레이어의 Ready 상태와 캐릭터 정보도 가져옴
                object isPlayerReady;
                if (p.CustomProperties.TryGetValue("IsReady", out isPlayerReady))
                {
                    EnemyInfo.GetComponent<PlayerListEntry>().SetPlayerReady((bool)isPlayerReady);
                }
                
                object characterIndex;
                if (p.CustomProperties.TryGetValue("CharacterIndex", out characterIndex))
                {
                    EnemyInfo.GetComponent<PlayerListEntry>().SetPlayerCharacter((int)characterIndex);
                }
                break;
            }
        }
        
        StartGameButton.gameObject.SetActive(CheckPlayersReady());
        
        // 방에 들어올 때 IsReady를 false로 초기화
        Hashtable props = new Hashtable
        {
            { "IsReady", false },
            { "CharacterIndex", 0 }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (newPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber) return;
        
        EnemyInfo.GetComponent<PlayerListEntry>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);
        playerListEntries.Add(newPlayer.ActorNumber, EnemyInfo);
        
        StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        EnemyInfo.GetComponent<PlayerListEntry>().Initialize(0, "Waiting...");
        EnemyInfo.GetComponent<PlayerListEntry>().SetPlayerReady(false);
        EnemyInfo.GetComponent<PlayerListEntry>().SetPlayerCharacter(0);
        
        playerListEntries.Remove(otherPlayer.ActorNumber);
        StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }
    
    public override void OnLeftRoom()
    {
        SetActivePanel(TitlePanel.name);

        if (playerListEntries != null)
        {
            playerListEntries.Clear();
        }
        playerListEntries = null;
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }
    }
    
    
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (playerListEntries == null)
        {
            playerListEntries = new Dictionary<int, GameObject>();
        }
        
        GameObject entry;
        if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
        {
            object isPlayerReady;
            if (changedProps.TryGetValue("IsReady", out isPlayerReady))
            {
                entry.GetComponent<PlayerListEntry>().SetPlayerReady((bool) isPlayerReady);
            }
            
            object characterSelect;
            if (changedProps.TryGetValue("CharacterIndex", out characterSelect))
            {
                entry.GetComponent<PlayerListEntry>().SetPlayerCharacter((int) characterSelect);
            }
        }

        StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    #endregion
    
    #region UI CallBacks

    public void OnTitleStartButtonClicked()
    {
        SetActivePanel(LoginPanel.name);
    }
    
    public void OnBackButtonClicked()
    {
        if (PhotonNetwork.IsConnected)
        {
            // 방에 있다면 방을 나갑니다
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
            // 로비에 있다면 로비를 나갑니다
            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveLobby();
            }
            // 서버와의 연결을 완전히 끊습니다
            PhotonNetwork.Disconnect();
        }
        
        // PlayerInfo와 EnemyInfo 초기화
        if (PlayerInfo != null)
        {
            PlayerInfo.GetComponent<PlayerListEntry>().Initialize(0, "");
            PlayerInfo.GetComponent<PlayerListEntry>().SetPlayerReady(false);
            PlayerInfo.GetComponent<PlayerListEntry>().SetPlayerCharacter(0);
        }
        
        if (EnemyInfo != null)
        {
            EnemyInfo.GetComponent<PlayerListEntry>().Initialize(0, "Waiting...");
            EnemyInfo.GetComponent<PlayerListEntry>().SetPlayerReady(false);
            EnemyInfo.GetComponent<PlayerListEntry>().SetPlayerCharacter(0);
        }
        
        // 플레이어 목록만 초기화
        if (playerListEntries != null)
        {
            playerListEntries.Clear();
        }
        
        // 입력 필드를 초기화합니다
        PlayerNameInput.text = "";
        
        SetActivePanel(TitlePanel.name);
    }
    
    public void OnLoginButtonClicked()
    {
        string playerName = PlayerNameInput.text;

        if (!string.IsNullOrEmpty(playerName))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.LogError("Player Name is invalid.");
        }
    }
    
    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }
    
    public void OnStartGameButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel("MultiCombat");
        
    }

    

    #endregion


    private bool CheckPlayersReady()
    {
        // 플레이어가 혼자면 시작 불가능
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            return false;
        }

        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
        }

        object isLocalPlayerReady;
        if(PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("IsReady", out isLocalPlayerReady))
        {
            if(!(bool)isLocalPlayerReady)
            {
                return false;
            }
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object isPlayerReady;
            if (p.CustomProperties.TryGetValue("IsReady", out isPlayerReady))
            {
                if (!(bool) isPlayerReady)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        return true;
    }
    
    public void SetActivePanel(string activePanel)
    {
        playSound(clickSound);
        TitlePanel.SetActive(activePanel.Equals(TitlePanel.name));
        LoginPanel.SetActive(activePanel.Equals(LoginPanel.name));
        InsideRoomPanel.SetActive(activePanel.Equals(InsideRoomPanel.name));
    }
    
    public void LocalPlayerPropertiesUpdated()
    {
        StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    public void playSound(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("서버와의 연결이 끊어졌습니다: " + cause.ToString());
        
        // PlayerInfo와 EnemyInfo 초기화
        if (PlayerInfo != null)
        {
            PlayerInfo.GetComponent<PlayerListEntry>().Initialize(0, "");
            PlayerInfo.GetComponent<PlayerListEntry>().SetPlayerReady(false);
            PlayerInfo.GetComponent<PlayerListEntry>().SetPlayerCharacter(0);
        }
        
        if (EnemyInfo != null)
        {
            EnemyInfo.GetComponent<PlayerListEntry>().Initialize(0, "Waiting...");
            EnemyInfo.GetComponent<PlayerListEntry>().SetPlayerReady(false);
            EnemyInfo.GetComponent<PlayerListEntry>().SetPlayerCharacter(0);
        }
        
        SetActivePanel(TitlePanel.name);
        
        if (playerListEntries != null)
        {
            playerListEntries.Clear();
        }
    }
}
