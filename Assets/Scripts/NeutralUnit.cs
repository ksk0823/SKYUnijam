using UnityEngine;
using Photon.Pun;

public class NeutralUnit : UnitObject
{
    [Header("Main Settings")]
    public int index;
    public int health = 8;
    public float attackSpeed;
    public float moveSpeed = 3f;

    [Header("Objects")]
    public GameObject neutralPrefab;
    public Transform target;

    private Rigidbody2D rb;
    private Collider2D coll;
    private string myTag;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        if (!pv.IsMine)
        {
            unitGroup = EUnitGroup.Enemy;
            CombatMain.Instance.enemyUnits.Add(gameObject);
        } else
        {
            unitGroup = EUnitGroup.Allay;
            CombatMain.Instance.playerUnits.Add(gameObject);
        }
        
        myTag = gameObject.tag;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.CompareTag("Player"))
        //{
        //    coll.isTrigger = false;
        //    Split();
        //    Destroy(gameObject); // 원래 오브젝트 삭제
        //}
        //else if (collision.CompareTag("Enemy"))
        //{
        //    // 전투 구현해야 함
        //}
    }

    public void Split()
    {
        //if (!PhotonNetwork.IsMasterClient) return;
        
        for (int i = 0; i < 2; i++)
        {
            Vector3 spawnPosition = transform.position + Vector3.up * 0.5f * i;
            GameObject newObject = PhotonNetwork.Instantiate("Unit"+index.ToString(), spawnPosition, Quaternion.identity);
            
            Collider2D newColl = newObject.GetComponent<Collider2D>();
            Rigidbody2D newRb = newObject.GetComponent<Rigidbody2D>();
            
            // RPC를 통해 새로 생성된 오브젝트의 속성을 설정
            PhotonView newPV = newObject.GetComponent<PhotonView>();
            newPV.RPC("RPCInitializeUnit", RpcTarget.All);
            
            
        }
    }

    [PunRPC]
    private void RPCInitializeUnit()
    {
        Collider2D coll = GetComponent<Collider2D>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        
        coll.isTrigger = false;
        rb.isKinematic = false;
        gameObject.layer = LayerMask.NameToLayer("Active Unit");
        gameObject.tag = "Un Fixed";

        SetTarget(FindClosestEnemy());
    }

    public void SetTarget(Transform enemy)
    {
        target = enemy;
    }

    private Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = enemy.transform;
            }
        }

        return closest;
    }
}
