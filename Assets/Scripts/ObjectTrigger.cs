using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTrigger : MonoBehaviour
{
    public NeutralUnit unitPrefab;

    private bool isPlayerNearby = false;
    private string myTag;

    private void Awake()
    {
        myTag = gameObject.tag;
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Space))
        {
            if (myTag == "Nexus Trigger")
                InteractNexus();
            else if (myTag == "Unit Trigger")
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
        Debug.Log("유닛 상호작용 실행");
        // 플레이어 유닛 상호작용 로직 추가
        unitPrefab.Split();
        Destroy(transform.parent.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}
