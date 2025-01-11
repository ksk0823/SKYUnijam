using UnityEditor.EditorTools;
using UnityEngine;
using Photon.Pun;

public class NeutralUnit : UnitObject
{
    [Header("Main Settings")]
    public int index;
    public bool isFirst;
    public int health = 8;
    public float moveSpeed = 3f;

    [Header("Objects")]
    public GameObject neutralPrefab;

    private Rigidbody2D rb;
    private Vector2 currentDirection;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        //coll = GetComponent<Collider2D>();
        if(isFirst)
        {
            if (!pv.IsMine)
        {
            unitGroup = EUnitGroup.Enemy;
            CombatMain.Instance.enemyUnits.Add(gameObject);
        } else
        {
            unitGroup = EUnitGroup.Allay;
            CombatMain.Instance.playerUnits.Add(gameObject);
        }}
        
        //myTag = gameObject.tag;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void Start()
    {
        //currentDirection = Vector2.zero;
    }

    private void FixedUpdate()
    {
        rb.velocity = currentDirection * moveSpeed;
    }

    public void Split(int splitTimes, Transform transform)
    {
        // 분열된 두 개의 오브젝트 생성
        for (int i = 0; i < splitTimes; i++)
        {
            GameObject tempObject = null; // splitTimes = 1일 때 오브젝트 활성화 위한 임시 오브젝트
            Vector3 spawnPosition = transform.position + Vector3.up * 0.5f * i;

            if (splitTimes == 1)
            {
                //tempObject = PhotonNetwork.Instantiate("Unit"+index.ToString(), spawnPosition, Quaternion.identity);
            }

            GameObject newObject = PhotonNetwork.Instantiate("Unit"+index.ToString(), spawnPosition, Quaternion.identity);
            //newObject.transform.position = transform.position + Vector3.up * 0.5f * i;
            //newObject.transform.rotation = Quaternion.identity;
            
            // RPC를 통해 새로 생성된 오브젝트의 속성을 설정
            PhotonView newPV = newObject.GetComponent<PhotonView>();
            newPV.RPC("RPCInitializeUnit", RpcTarget.All);
            
            
        }
    }

    [PunRPC]
    private void RPCInitializeUnit()
    {
        if (pv.IsMine)
        {
            unitGroup = EUnitGroup.Allay;
            CombatMain.Instance.playerUnits.Add(gameObject);
        } else
        {
            unitGroup = EUnitGroup.Enemy;
            CombatMain.Instance.enemyUnits.Add(gameObject);
        }
        Collider2D coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        
        coll.isTrigger = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        gameObject.layer = LayerMask.NameToLayer("Active Unit");
        gameObject.tag = "Un Fixed";
        foreach (Transform child in transform) // Unit 오브젝트 아래의 Trigger의 태그 변경
        {
            Debug.Log("Changed to split disable");
            child.gameObject.tag = "Split Disable";
        }

        //SetTarget(FindClosestEnemy());
        health = 8;
        MoveToRandomDirection();
        
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

        else if (objTag == "Nexus" || objTag == "Border" || objTag == "Enemy")
        {
            currentDirection.x *= Random.Range(-0.8f, -1.2f);
            currentDirection.y *= Random.Range(-0.8f, -1.2f);

            if (objTag == "Enemy")
            {
                Damage(1);
                collObj.GetComponent<Enemy>().Damage(1);
            }
        }
    }

    public void Damage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Debug.Log("I'm Dead");
            gameObject.SetActive(false);
        }
    }
    
}

    //public void SetTarget(Transform enemy)
    //{
    //    target = enemy;
    //}

    //private Transform FindClosestEnemy()
    //{
    //    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    //    Transform closest = null;
    //    float closestDistance = Mathf.Infinity;

    //    foreach (GameObject enemy in enemies)
    //    {
    //        float distance = Vector3.Distance(transform.position, enemy.transform.position);
    //        if (distance < closestDistance)
    //        {
    //            closestDistance = distance;
    //            closest = enemy.transform;
    //        }
    //    }

    //    return closest;
    //}

