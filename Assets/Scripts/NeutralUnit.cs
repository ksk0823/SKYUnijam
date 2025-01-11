using UnityEditor.EditorTools;
using UnityEngine;
 
public class NeutralUnit : UnitObject
{
    [Header("Main Settings")]
    public float health;
    public float moveSpeed = 3f;
    public float spawnInterval = 1f;
    public float damage;

    [Header("Objects")]
    public GameObject neutralPrefab;

    private Rigidbody2D rb;
    private Vector2 currentDirection;
    private bool canSpawn = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void Start()
    {
        currentDirection = Vector2.zero;
    }

    private void FixedUpdate()
    {
        rb.velocity = currentDirection * moveSpeed;
    }

    public void Split(int splitTimes, Transform transform, EUnitGroup hitUnitGroup)
    {
        if (!canSpawn)
                {
                    Debug.Log("Spawn is on cooldown.");
                    return;
                }
        
        StartCoroutine(SpawnCooldown());
                
        EUnitGroup newUnitGroup = EUnitGroup.Neutral;
        if (hitUnitGroup == EUnitGroup.Neutral)
        {
            newUnitGroup = hitUnitGroup;
        } 
        else if (hitUnitGroup == EUnitGroup.Allay)
        {
            newUnitGroup = EUnitGroup.Allay;
            GameManager.instance.ActivePlayerUnits += splitTimes;
        }
        else if (hitUnitGroup == EUnitGroup.Enemy)
        {
            newUnitGroup = EUnitGroup.Enemy;
            GameManager.instance.ActiveEnemyUnits += splitTimes;
        }

        for (int i = 0; i < splitTimes; i++)
        {
            //GameObject newObject = GameManager.instance.pool.Get(0);
            //GameObject tempObject = null; // splitTimes = 1일 때 오브젝트 활성화 위한 임시 오브젝트

            //if (splitTimes == 1) // 예외처리 코드
            //{
            //    Debug.Log("Make only one object");

            //    tempObject = GameManager.instance.pool.Get(0);
            //    //SpriteRenderer tempSr = tempObject.GetComponent<SpriteRenderer>();

            //    tempObject.transform.position = transform.position + Vector3.up * 0.5f;
            //    tempObject.transform.rotation = Quaternion.identity;
            //    tempObject.layer = LayerMask.NameToLayer("Active Unit");
            //}
            GameObject newObject = null;
            if(newUnitGroup == EUnitGroup.Enemy)
            {
                newObject = GameManager.instance.pool.Get(GameManager.instance.computerCharacterIndex);
                newObject.GetComponent<NeutralUnit>().unitGroup = EUnitGroup.Enemy;
            } else if (newUnitGroup == EUnitGroup.Allay)
            {
                newObject = GameManager.instance.pool.Get(GameManager.instance.playerCharacterIndex);
                newObject.GetComponent<NeutralUnit>().unitGroup = EUnitGroup.Allay;
            } else
            {
                newObject = GameManager.instance.pool.Get(0);
            }

            //GameObject newObject = GameManager.instance.pool.Get(0);
            newObject.transform.position = transform.position + Vector3.up * 0.5f * i;
            newObject.transform.rotation = Quaternion.identity;

            Collider2D newColl = newObject.GetComponent<Collider2D>();
            Rigidbody2D newRb = newObject.GetComponent<Rigidbody2D>();
            NeutralUnit neutralScript = newObject.GetComponent<NeutralUnit>();

            newColl.isTrigger = false;
            newRb.isKinematic = false;
            newObject.layer = LayerMask.NameToLayer("Active Unit");
            
            // GameManager의 리스트에 추가
            if (newUnitGroup == EUnitGroup.Allay)
            {
                GameManager.instance.playerUnits.Add(newObject);
            }
            else if (newUnitGroup == EUnitGroup.Enemy)
            {
                GameManager.instance.enemyUnits.Add(newObject);
            }

            foreach (Transform child in newObject.transform)
            {
                child.gameObject.tag = "Split Disable";
            }

            //NeutralUnit neutralScript = newObject.GetComponent<NeutralUnit>();
            neutralScript.health = 8;
            neutralScript.MoveToRandomDirection();
        }
    }

    private System.Collections.IEnumerator SpawnCooldown()
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnInterval);
        canSpawn = true;
    }

    void MoveToRandomDirection()
    {
        currentDirection = Random.insideUnitCircle.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collObj = collision.gameObject;
        string objTag = collision.transform.tag;

        if (objTag == "Fixed")
        {
            MoveToRandomDirection();
        }
        else if (objTag == "Nexus" || objTag == "Border")
        {
            currentDirection.x *= Random.Range(-0.8f, -1.2f);
            currentDirection.y *= Random.Range(-0.8f, -1.2f);
        }
        
        // NeutralUnit끼리의 충돌 처리
        NeutralUnit otherUnit = collObj.GetComponent<NeutralUnit>();
        if (otherUnit != null)
        {
            // 서로 다른 그룹일 경우에만 데미지 처리
            if (unitGroup != otherUnit.unitGroup && 
                unitGroup != EUnitGroup.Neutral && 
                otherUnit.unitGroup != EUnitGroup.Neutral)
            {
                Debug.Log($"Collision between {unitGroup} and {otherUnit.unitGroup}");
                Damage(damage);
                otherUnit.Damage(damage);
            }
            
            // 충돌 시 방향 전환
            currentDirection.x *= Random.Range(-0.8f, -1.2f);
            currentDirection.y *= Random.Range(-0.8f, -1.2f);
        }
    }

    public void Damage(float damage)
    {
        Debug.Log($"Unit Damaged, Damage : {damage}");
        health -= damage;

        if (health <= 0)
        {
            Debug.Log("I'm Dead");
            if(unitGroup == EUnitGroup.Allay)
            {
                GameManager.instance.pool.prefabs[GameManager.instance.computerCharacterIndex].GetComponent<NeutralUnit>().Split(1, transform, EUnitGroup.Enemy);
            } else if (unitGroup == EUnitGroup.Enemy)
            {
                GameManager.instance.pool.prefabs[GameManager.instance.playerCharacterIndex].GetComponent<NeutralUnit>().Split(1, transform, EUnitGroup.Allay);
            }
            gameObject.SetActive(false);
            if(unitGroup == EUnitGroup.Allay)
            {
                GameManager.instance.ActivePlayerUnits--;
            } else if (unitGroup == EUnitGroup.Enemy)
            {
                GameManager.instance.ActiveEnemyUnits--;
            }
        }
    }
}