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
    [Header("Login UI")]
    public GameObject LoginPanel;
    public TMP_InputField PlayerNameInput;
    
    [Header("Selection Panel")]
    public GameObject SelectionPanel;

    [Header("Create Room Panel")]
    public GameObject CreateRoomPanel;
    public TMP_InputField CreateRoomCodeInputField;
    
    [Header("Enter Room Panel")]
    public GameObject EnterRoomPanel;
    public TMP_InputField EnterRoomCodeInputField;

    [Header("Join Random Room Panel")]
    public GameObject JoinRandomRoomPanel;
    
    [Header("Inside Room Panel")]
    public GameObject InsideRoomPanel;
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
        //this.SetActivePanel(SelectionPanel.name);
        PhotonNetwork.JoinRandomRoom();
    }
    
    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinLobby();
        SetActivePanel(SelectionPanel.name);
    }
    
    public override void OnCreatedRoom()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("IsTutorial") &&
            (bool)PhotonNetwork.CurrentRoom.CustomProperties["IsTutorial"])
        {
            Debug.Log("Tutorial Room created. Loading tutorial scene...");
            // 튜토리얼 씬으로 이동
            PhotonNetwork.LoadLevel("TutorialScene");
        }
        else
        {
            Debug.Log("Game Room created. Waiting for players...");
            // 일반 게임 대기 로직 추가
        }
    }
    
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Room creation failed: " + message);
        SetActivePanel(SelectionPanel.name);
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
        
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(PlayerListEntryPrefab);
            entry.transform.SetParent(PlayerListPanel.transform);
            entry.transform.localScale = Vector3.one;
            PlayerListEntry playerEntry = entry.GetComponent<PlayerListEntry>();
            playerEntry.Initialize(p.ActorNumber, p.NickName);
            
            object isPlayerReady;
            if (p.CustomProperties.TryGetValue("IsReady", out isPlayerReady))
            {
                entry.GetComponent<PlayerListEntry>().SetPlayerReady((bool) isPlayerReady);
            }
            
            playerListEntries.Add(p.ActorNumber, entry);
        }
        
        StartGameButton.gameObject.SetActive(CheckPlayersReady());
        
        // 방에 들어올 때 IsReady를 false로 초기화
        Hashtable props = new Hashtable
        {
            { "IsReady", false },
                {"CharacterIndex", 0}}; // 기본값으로 false로 설정
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject entry = Instantiate(PlayerListEntryPrefab);
        entry.transform.SetParent(PlayerListPanel.transform);
        entry.transform.localScale = Vector3.one;
        entry.GetComponent<PlayerListEntry>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);
        
        playerListEntries.Add(newPlayer.ActorNumber, entry);
        
        StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
        playerListEntries.Remove(otherPlayer.ActorNumber);

        StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }
    
    public override void OnLeftRoom()
    {
        SetActivePanel(SelectionPanel.name);

        foreach (GameObject entry in playerListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        playerListEntries.Clear();
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
    
    public void OnBackButtonClicked()
    {
        
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        SetActivePanel(SelectionPanel.name);
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
    
    public void OnCreateRoomButtonClicked()
    {
        string roomCode = CreateRoomCodeInputField.text;
        if (!string.IsNullOrEmpty(roomCode))
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 2; // 2인 멀티플레이로 설정
            PhotonNetwork.CreateRoom(roomCode, options, null);
        }
        else
        {
            Debug.LogError("Room Code is invalid.");
        }
    }
    
    public void OnEnterRoomButtonClicked()
    {
        string roomCode = EnterRoomCodeInputField.text;
        if (!string.IsNullOrEmpty(roomCode))
        {
            PhotonNetwork.JoinRoom(roomCode);
        }
        else
        {
            Debug.LogError("Room Code is invalid.");
        }
    }
    
    public void OnJoinRandomRoomButtonClicked()
    {
        SetActivePanel(JoinRandomRoomPanel.name);
        PhotonNetwork.JoinRandomRoom();
    }
    
    
    
    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }
    
    public void OnStartGameButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        //SceneManager.Load(SceneType.Combat);
        PhotonNetwork.LoadLevel("TestCombat");
        
    }

    

    #endregion
    
    private bool CheckPlayersReady()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
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
        LoginPanel.SetActive(activePanel.Equals(LoginPanel.name));
        SelectionPanel.SetActive(activePanel.Equals(SelectionPanel.name));
        CreateRoomPanel.SetActive(activePanel.Equals(CreateRoomPanel.name));
        EnterRoomPanel.SetActive(activePanel.Equals(EnterRoomPanel.name));
        JoinRandomRoomPanel.SetActive(activePanel.Equals(JoinRandomRoomPanel.name));
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

    
}
