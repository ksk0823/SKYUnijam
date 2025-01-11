using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ObjectTrigger : MonoBehaviour
{
    [Header("Objects")]
    public NeutralUnit unitPrefab;
    public Nexus nexus;
    public float interactionTime;

    private bool isPlayerNearby = false;
    private string myTag;
    private float holdTime = 0f;

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
                DecreaseInteractionTime(interactionTime);
                break;

            default:
                interactionTime = 5f;
                DecreaseInteractionTime(interactionTime);
                break;
        }
    }

    void DecreaseInteractionTime(float time)
    {
        if (GameManager.instance.playerCharacterIndex == 1) // 플레이어가 파랑색일 때
        {
            time *= 0.7f; // 채굴 속도 30% 감소
        }
    }

    void Update()
    {
        if (isPlayerNearby)
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
    }

    void InteractUnit() 
    {
        Debug.Log("유닛 상호작용 실행");

        unitPrefab.Split(2, transform); // 한 개일 때 발생하는 버그 수정해야 함
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
