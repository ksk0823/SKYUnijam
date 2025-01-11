using UnityEditor.EditorTools;
using UnityEngine;
 
public class NeutralUnit : MonoBehaviour
{
    [Header("Main Settings")]
    public int health = 8;
    public float moveSpeed = 3f;

    [Header("Objects")]
    public GameObject neutralPrefab;

    private Rigidbody2D rb;
    private Vector2 currentDirection;

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

    public void Split(int splitTimes, Transform transform)
    {
        // 분열된 두 개의 오브젝트 생성
        for (int i = 0; i < splitTimes; i++)
        {
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
        Debug.Log($"Unit Damaged, Damage : {damage}");
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