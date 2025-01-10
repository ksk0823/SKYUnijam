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

    //게임 제한시간 5분
    private const float DEADLINE = 120f;

    private bool _isInit = false;

    //게임 플레이 시간
    private float _gameTime;

    private AudioSource _audioSource;

    //[SerializeField] private CombatUI _combatUI;
    //[SerializeField] private CombatCameraController _combatCameraController;
    
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
    
        CheckTime();
        //UpdataInput();
            
        // 주기적 아이템 생성 관리는 Master에서만 하도록
        if (PhotonNetwork.IsMasterClient)
        {
                //CheckUnit();
        }
            
            // 스코어 및 시간 업데이트
            //_combatUI.UpdateScoreText(_scoreDict[EUnitGroup.Allay], _scoreDict[EUnitGroup.Enemy]);
            //_combatUI.UpdateTimeText((int)(DEADLINE - _gameTime));
            //_combatUI.UpdateArrow();
        
    }

    private void UpdateInput()
    {
        /*
        bool isInput = false;
        Vector3 moveVec = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            moveVec += Vector3.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveVec += Vector3.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveVec += Vector3.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveVec += Vector3.right;
        }
        
        if (moveVec != Vector3.zero)
        {
            GetPlayerObject().UpdateMoveDir(moveVec);
            if (GetPlayerObject().StateMachine.CurrentState != GetPlayerObject().StateMachine.Move)
            {
                GetPlayerObject().StateMachine.ChangeState(GetPlayerObject().StateMachine.Move, GetPlayerObject());
            }
        }
        
        if (moveVec == Vector3.zero && _combatUI.GetIsDrag() == false)
        {
            //GetPlayerObject().UpdateMoveDir(moveVec);
            GetPlayerObject().StateMachine.ChangeState(GetPlayerObject().StateMachine.Idle, GetPlayerObject());
        }
        
        if (Input.GetKey(KeyCode.L))
        {
            GetPlayerObject().Attack();
        }
        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            GetPlayerObject().UseItem(0);
        }
        if (Input.GetKeyDown(KeyCode.Quote))
        {
            GetPlayerObject().UseItem(1);
        }
        */
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

        yield return StartCoroutine(FindPlayerCoroutine());
        
        _isInit = true;
    }
    
    //캐릭터 움직임은 끊김 방지를 위해 fixed에...
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

    private void CheckTime()
    {
        _gameTime += Time.deltaTime;
        //종료
        if (_gameTime >= DEADLINE)
        {
            photonView.RPC("EndGame", RpcTarget.All);
        }
    }
    
    public IEnumerator GenerateEnemyPlayerUnitCoroutine(Vector3 objectPos)
    {
        
        //var loadOperation = AddressableManager.LoadAssetAsync<GameObject>("Assets/Prefab/Unit/Potato.prefab");
        yield return null;
        
        string characterPath = "TestPlayer";
        /*
        PlayerObject playerObject =
            Instantiate(characterPath, objectPos, transform.rotation)
                .GetComponent<PlayerObject>();
        //_unitDB.InitUnit(playerObject, EUnitGroup.Enemy);
        */
    }

    private IEnumerator GeneratePlayerUnitCoroutine()
    {
        string characterPath = "One";  // 기본값
        int index = 0;
        
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
        }

        //PlayerObject playerObject =
                PhotonNetwork.Instantiate(characterPath, new Vector3(7f, 1.25f, 0), transform.rotation, 0);
        //            .GetComponent<PlayerObject>();
        
        Hashtable playerProperties = new Hashtable
        {
            { "IsObjectCreated", true }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        
        yield return null;
    }
    
    
    // 게임 시작 시, 플레이어블 캐릭터 전부 가져와서 Allay, Enemy로 나눔
    public IEnumerator FindPlayerCoroutine()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        Debug.Log(players.Length);
        foreach (GameObject p in players)
        {
            if (p.GetComponent<PhotonView>().IsMine)
            {
                //_unitDB.InitUnit(p.GetComponent<PlayerObject>(), EUnitGroup.Allay);
            }
            else
            {
                //_unitDB.InitUnit(p.GetComponent<PlayerObject>(), EUnitGroup.Enemy);
            }
        }
        
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