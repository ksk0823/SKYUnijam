using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ObjectTrigger : MonoBehaviour
{
    [Header("Objects")]
    public NeutralUnit unitPrefab;
    public Nexus nexus;
    public ParticleSystem part;

    private bool isPlayerNearby = false;
    private string myTag;
    private float holdTime = 0f;
    public float interactionTime;

    private EUnitGroup hitUnitGroup;
    public Transform spawnPosition;

    private void Awake()
    {
        part.gameObject.SetActive(false);  
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
            case "Fixed":
                interactionTime = 5f;
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
        if(isPlayerNearby)
        {
            holdTime += Time.deltaTime;

            if (holdTime >= interactionTime)
            {
                HandleInteraction();
                holdTime = 0f;
            }
        } else
        {
            holdTime = 0f;
        }
        /*
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
        }*/
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
        } else if (myTag == "Fixed")  // 중립 유닛 생성 트리거
    {
        SpawnNeutralUnit();
    }
    }

    void SpawnNeutralUnit()
    {
        if(spawnPosition != null)
        {
            if(hitUnitGroup == EUnitGroup.Allay)
            {
                GameManager.instance.pool.prefabs[GameManager.instance.playerCharacterIndex].GetComponent<NeutralUnit>().Split(2, spawnPosition.transform, EUnitGroup.Allay);
            } 
            else if (hitUnitGroup == EUnitGroup.Enemy)
            {
                GameManager.instance.pool.prefabs[GameManager.instance.computerCharacterIndex].GetComponent<NeutralUnit>().Split(2, spawnPosition.transform, EUnitGroup.Enemy);
            }
            
            //gameObject.SetActive(false);
            
            if(hitUnitGroup == EUnitGroup.Allay)
            {
                GameManager.instance.ActivePlayerUnits--;
            } 
            else if (hitUnitGroup == EUnitGroup.Enemy)
            {
                GameManager.instance.ActiveEnemyUnits--;
            }
            
            holdTime = 0f;
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
            part.gameObject.SetActive(true);
            isPlayerNearby = true;
            hitUnitGroup = other.GetComponent<UnitObject>().unitGroup;

            // 트리거 타입에 따라 파티클 설정
            switch (myTag)
            {
                case "Nexus Trigger":
                    ActivateParticleSystem(5f);  // 파티클 수명 5초
                    break;

                case "Fixed":
                    ActivateParticleSystem(5f);  // 파티클 수명 5초
                    break;

                case "Unit Trigger":
                    ActivateParticleSystem(2f);  // 파티클 수명 2초
                    break;

                default:
                    break;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            part.Stop();
            part.gameObject.SetActive(false);
            isPlayerNearby = false;
            hitUnitGroup = EUnitGroup.Neutral;
        }
    }
    void ActivateParticleSystem(float lifetime)
    {
        if (part != null)
        {
            part.gameObject.SetActive(true);
            var main = part.main;
            main.startLifetime = lifetime;  // 파티클 시작 수명 설정
            part.Play();
        }
    }
}
