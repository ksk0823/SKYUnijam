using UnityEngine;
 
public class NeutralUnit : MonoBehaviour
{
    public GameObject neutralPrefab;
    public float speed = 3f;

    private Transform target;
    private Rigidbody2D rb;
    private Collider2D coll;
    private bool isMove;

    private void Awake()
    {
        isMove = true;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (target != null && isMove)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            coll.isTrigger = false;
            Split();
            Destroy(gameObject); // 원래 오브젝트 삭제
        }
        else if (collision.CompareTag("Enemy"))
        {
            // 전투 구현해야 함
        }
    }

    private void Split()
    {
        // 분열된 두 개의 오브젝트 생성
        for (int i = 0; i < 2; i++)
        {
            GameObject newObject = Instantiate(neutralPrefab, transform.position + Vector3.up * 0.5f * i, Quaternion.identity);
            Collider2D newColl = newObject.GetComponent<Collider2D>();
            newColl.isTrigger = false;
            NeutralUnit neutralScript = newObject.GetComponent<NeutralUnit>();
            neutralScript.SetTarget(FindClosestEnemy());
        }
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
