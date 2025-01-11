using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObjectTrigger : MonoBehaviourPunCallbacks
{
    public NeutralUnit unitPrefab;

    private bool isPlayerNearby = false;
    private string myTag;
    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        myTag = gameObject.tag;
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Space))
        {
            HandleInteraction();
        }
    }

    private void HandleInteraction()
    {
        if (myTag == "Nexus Trigger")
        {
            InteractNexus();
        }
        else if (myTag == "Unit Trigger")
        {
            InteractUnit();
        }
    }

    void InteractNexus()
    {
        Debug.Log("넥서스 상호작용 실행");
        // 플레이어 넥서스 상호작용 로직 추가
    }

    void InteractUnit()
    {
        if (!PhotonNetwork.IsMessageQueueRunning) return;
        
        Debug.Log("유닛 상호작용 실행");
        pv.RPC("RPCSplit", RpcTarget.All);
    }

    [PunRPC]
    void RPCSplit()
    {
        unitPrefab.Split();
        Destroy(transform.parent.gameObject);
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.gameObject.GetComponent<UnitObject>().unitGroup == EUnitGroup.Allay)
        {
            isPlayerNearby = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.gameObject.GetComponent<UnitObject>().unitGroup == EUnitGroup.Allay)
        {
            isPlayerNearby = false;
        }
    }
}
