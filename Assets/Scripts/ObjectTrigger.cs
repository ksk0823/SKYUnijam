using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ObjectTrigger : MonoBehaviour
{
    [Header("Objects")]
    public NeutralUnit unitPrefab;
    public Nexus nexus;

    private bool isPlayerNearby = false;
    private string myTag;
    private float holdTime = 0f;
    public float interactionTime;

    private EUnitGroup hitUnitGroup;

    private void Awake()
    {
        myTag = gameObject.tag;

        SetInterTime();
    }

    public void ChangeTag()
    { 
        myTag = gameObject.tag;
        Debug.Log("ChangedTg");
        SetInterTime();
    }

    public void SetInterTime()
    {
        switch (myTag)
        {
            case "Unit Trigger":
                break;

            case "Nexus Trigger":
                interactionTime = 6f;
                break;

            default:
                interactionTime = 5f;
                break;
        }
    }

    void Update()
    {
        if (isPlayerNearby && hitUnitGroup == EUnitGroup.Allay)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                holdTime += Time.deltaTime;

                if (holdTime >= interactionTime)
                {
                    HandleInteraction();
                }
            }
            else
            {
                holdTime = 0f;
            }
        } else if (isPlayerNearby && hitUnitGroup == EUnitGroup.Enemy)
        {
           holdTime += Time.deltaTime;
           if(holdTime >= interactionTime)
           {
            HandleInteraction();
           }
        }
    }

    private void HandleInteraction()
    {
       if (myTag == "Nexus Trigger")
        {
            InteractNexus();
        }

        else if (myTag == "Unit Trigger") // myTag : Unit 오브젝트 아래의 Trigger의 태그
        {
            InteractUnit();
        }
    }

    void InteractNexus()
    {
        Debug.Log("넥서스 상호작용 실행");
        // 플레이어 넥서스 상호작용 로직 추가
        //nexus.ShowCardUI();
        if(transform.parent.gameObject.GetComponent<UnitObject>().unitGroup == EUnitGroup.Allay)
        {
            GameManager.instance.playerNexusInteractionCount++;
        }
        else if(transform.parent.gameObject.GetComponent<UnitObject>().unitGroup == EUnitGroup.Enemy)
        {
            GameManager.instance.enemyNexusInteractionCount++;
        }
    }

    void InteractUnit() 
    {
        Debug.Log("유닛 상호작용 실행");

        unitPrefab.Split(2, transform, hitUnitGroup); // 한 개일 때 발생하는 버그 수정해야 함
        Destroy(transform.parent.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            hitUnitGroup = other.GetComponent<UnitObject>().unitGroup;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            hitUnitGroup = EUnitGroup.Neutral;
        }
    }
}
