using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.StructWrapping;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;

/// <summary>
/// 게임의 중심이 되는 클래스
/// </summary>
public class CombatMain : MonoBehaviourPunCallbacks
{
    public static CombatMain Instance; // 싱글톤

    private bool _isInit = false;

    private AudioSource _audioSource;

    [Header("Units Object")]
    public List<GameObject> playerUnits = new List<GameObject>();
    public List<GameObject> enemyUnits = new List<GameObject>();
    
    private void Awake()
    {
        PhotonNetwork.SendRate = 20; // 초당 20번 데이터 전송
        PhotonNetwork.SerializationRate = 10; // 초당 10번 데이터 동기화

        _audioSource = GetComponent<AudioSource>();

        Instance = this;
        StartCoroutine(InitCoroutine());
    }
    
    private void Update()
    {
        if (!_isInit) return;
    }


    private IEnumerator InitCoroutine()
    {
        Debug.Log("Init 실행");
        
        yield return StartCoroutine(GeneratePlayerUnitCoroutine());
        
        // 모든 플레이어의 오브젝트 생성 완료 대기
        yield return new WaitUntil(() =>
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (!player.CustomProperties.ContainsKey("IsObjectCreated") ||
                    !(bool)player.CustomProperties["IsObjectCreated"])
                {
                    return false;
                }
            }
            return true;
        });

        //yield return StartCoroutine(FindPlayerCoroutine());
        
        _isInit = true;
    }
    
    private void FixedUpdate()
    {
        if (_isInit == false)
        {
            return;
        }

        //_unitDB.UpdateUnitDB();
    }

    public bool GetIsInit()
    {
        return _isInit;
    }

    private IEnumerator GeneratePlayerUnitCoroutine()
    {
        string characterPath = null;  // 기본값
        int index = 0;
        Vector3 spawnPosition;
        
        // Master Client인 경우 Player 1 위치, 아닌 경우 Player 2 위치에 스폰
         if (PhotonNetwork.IsMasterClient)
        {
            spawnPosition = new Vector3(-4.95f, 0.02f, 0);  // Player 1 위치
        }
        else
        {
            spawnPosition = new Vector3(4.95f, 0.02f, 0);   // Player 2 위치
        }

        // 각 플레이어 오브젝트 생성
        object playerCharecterIndex;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("CharacterIndex", out playerCharecterIndex))
        {
            index = (int)playerCharecterIndex;
        }

        switch (index)
        {
            case 0:
                characterPath = "TestPlayer";
                break;
            case 1:
                characterPath = "TestPlayer2";
                break;
        }

        // 플레이어 캐릭터 생성
        GameObject playerObj = PhotonNetwork.Instantiate(characterPath, spawnPosition, transform.rotation, 0);

        StartCoroutine(GenerateNeutralUnitCoroutine(index));
        
        yield return null;
    }

    IEnumerator GenerateNeutralUnitCoroutine(int index)
    {
        string characterPath = "One";  // 기본값
        Vector3 spawnPosition;
        
        switch (index)
        {
            case 0:
                characterPath = "BaseResource1";
                break;
            case 1:
                characterPath = "BaseResource2";
                break;
        }

        // Master Client인 경우 Player 1 위치, 아닌 경우 Player 2 위치에 스폰
        if (PhotonNetwork.IsMasterClient)
        {
            spawnPosition = new Vector3(-5.5f, 3.5f, 0);  // Player 1 위치
            // 자원 유닛 생성 (6개)
            for (int i = 0; i < 6; i++)
            {   
                
                GameObject resourceUnit = PhotonNetwork.Instantiate(characterPath, spawnPosition, Quaternion.identity, 0);
                //resourceUnit.transform.parent = player1Units.transform;  // 플레이어 오브젝트의 자식으로 설정
                spawnPosition = spawnPosition - new Vector3(0f, 1f, 0);  // 자원 유닛 위치 조정
                
            }
        }
        else
        {
            spawnPosition = new Vector3(8.21f, 3.5f, 0);   // Player 2 위치
            for (int i = 0; i < 6; i++)
            {   
                
                GameObject resourceUnit = PhotonNetwork.Instantiate(characterPath, spawnPosition, Quaternion.identity, 0);
                //resourceUnit.transform.parent = player2Units.transform;  // 플레이어 오브젝트의 자식으로 설정
                spawnPosition = spawnPosition - new Vector3(0f, 1f, 0);  // 자원 유닛 위치 조정
                
            }
        }

        StartCoroutine(GenerateNexusCoroutine(index));

        yield return null;
    }

    IEnumerator GenerateNexusCoroutine(int index)
    {
        string characterPath = "One";  // 기본값
        Vector3 spawnPosition;
        
        switch (index)
        {
            case 0:
                characterPath = "Nexus1";
                break;
            case 1:
                characterPath = "Nexus2";
                break;
        }

        // Master Client인 경우 Player 1 위치, 아닌 경우 Player 2 위치에 스폰
         if (PhotonNetwork.IsMasterClient)
        {
            spawnPosition = new Vector3(-7.26f, 0.02f, 0);  // Player 1 위치
            GameObject resourceUnit = PhotonNetwork.Instantiate(characterPath, spawnPosition, Quaternion.identity, 0);
                
        }
        else
        {
            spawnPosition = new Vector3(6.63f, 0.02f, 0);   // Player 2 위치
            GameObject resourceUnit = PhotonNetwork.Instantiate(characterPath, spawnPosition, Quaternion.identity, 0);
        }

        Hashtable playerProperties = new Hashtable
        {
            { "IsObjectCreated", true }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

        yield return null;
    }
    

    #region PUN Callbacks

    
    public override void OnDisconnected(DisconnectCause cause)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    #endregion
    

    #region RPC 메소드들

    [PunRPC]
    private void EndGame()
    {
        //_GameState = EGameState.End;
        //_combatUI.ShowEndScreen(_scoreDict[EUnitGroup.Allay], _scoreDict[EUnitGroup.Enemy]);
        _audioSource.Stop();
    }

    #endregion
    
    public void ToMainMenu()
    {
        PhotonNetwork.LeaveRoom();
    }
    
}