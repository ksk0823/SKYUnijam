using UnityEditor.EditorTools;
using UnityEngine;
 
public class NeutralUnit : MonoBehaviour
{
    [Header("Main Settings")]
    public int health = 8;
    public float attackSpeed;
    public float moveSpeed = 3f;

    [Header("Objects")]
    public GameObject neutralPrefab;
    public Transform target;

    private Rigidbody2D rb;
    private Collider2D coll;
    private string myTag;
    private Vector2 currentDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        myTag = gameObject.tag;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void Start()
    {
        currentDirection = Vector2.zero;
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
        rb.velocity = currentDirection * moveSpeed;
    }

    public void Split()
    {
        // 분열된 두 개의 오브젝트 생성
        for (int i = 0; i < 2; i++)
        {
            GameObject newObject = GameManager.instance.pool.Get(0);
            newObject.transform.position = transform.position + Vector3.up * 0.5f * i;
            newObject.transform.rotation = Quaternion.identity;

            Collider2D newColl = newObject.GetComponent<Collider2D>();
            Rigidbody2D newRb = newObject.GetComponent<Rigidbody2D>();

            newColl.isTrigger = false;
            newRb.isKinematic = false;
            newObject.layer = LayerMask.NameToLayer("Active Unit"); // Physics2D 상호작용 제거

            foreach (Transform child in newObject.transform) // Unit 오브젝트 아래의 Trigger의 태그 변경
            {
                child.gameObject.tag = "Split Disable";
            }

            NeutralUnit neutralScript = newObject.GetComponent<NeutralUnit>();
            //neutralScript.SetTarget(FindClosestEnemy());
            neutralScript.health = 8;
            neutralScript.MoveToRandomDirection();
        }
    }

    void MoveToRandomDirection()
    {
        currentDirection = Random.insideUnitCircle.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
            }
        }
    }

    public void Damage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
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

