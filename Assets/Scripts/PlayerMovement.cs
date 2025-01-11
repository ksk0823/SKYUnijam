using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Main Settings")]
    public float attackSpeed;
    public float moveSpeed;
    public EUnitGroup unitGroup;
    public CardManager cardManager;

    [Header("Click Attack Settings")]
    public LayerMask enemyLayer;
    public float clickDamage;
    public float attackRadius = 2f;
    public float maxAttackDistance = 5f;

    [Header("Cooldown Settings")]
    public float cooldownTime = 2f;  // 쿨타임 (2초)
    private float lastAttackTime = -Mathf.Infinity;  // 마지막 공격 시간

    [Header("Objects")]
    Rigidbody2D rb;
    Collider2D coll;

    private Vector2 inputVec;
    private Coroutine triggerCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        unitGroup = GetComponent<PlayerObject>().unitGroup;
        StartCoroutine(WaitEffect());
    }

    private void Update()
    {
        ClickAttack();
    }

    private void FixedUpdate()
    {
        Vector2 nextVec = inputVec * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVec);
    }
    IEnumerator WaitEffect()
    {
        yield return new WaitForSeconds(3f);
    }

    void ClickAttack()
    {
        // 쿨타임 확인
        if (Time.time - lastAttackTime < attackSpeed)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            worldPosition.z = 0f;

            float distanceToPlayer = Vector3.Distance(transform.position, worldPosition);

            if (distanceToPlayer <= maxAttackDistance)
            {
                DealDamageInArea(worldPosition);

                // 공격 성공 시 마지막 공격 시간 갱신
                lastAttackTime = Time.time;
            }
        }
    }

    private void DealDamageInArea(Vector3 center)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, attackRadius, enemyLayer);

        foreach (Collider2D collider in hitColliders)
        {
            NeutralUnit enemy = collider.GetComponent<NeutralUnit>();

            if (enemy != null && enemy.unitGroup != unitGroup)
            {
                Debug.Log($"공격 대상 : {enemy.unitGroup}");
                enemy.Damage(clickDamage);
            }
        }

        // 디버그용 동그라미 표시
        DebugDrawCircle(center, attackRadius, Color.green, 0.5f);
    }

    private void DebugDrawCircle(Vector3 center, float radius, Color color, float duration)
    {
        int segments = 36;
        float angleStep = 360f / segments;

        Vector3 prevPoint = center + new Vector3(radius, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector3 newPoint = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            Debug.DrawLine(prevPoint, newPoint, color, duration);
            prevPoint = newPoint;
        }
    }

    private void OnDrawGizmos()
    {
        if (transform != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, maxAttackDistance);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Nexus Region")
        {
            if (triggerCoroutine != null)
            {
                StopCoroutine(triggerCoroutine);
            }

            triggerCoroutine = StartCoroutine(TriggerTimer());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Nexus Region")
        {
            if (triggerCoroutine != null)
            {
                StopCoroutine(triggerCoroutine);
                triggerCoroutine = null;
            }
        }
    }

    private IEnumerator TriggerTimer()
    {
        yield return new WaitForSeconds(cardManager.cardInterval);
        cardManager.showCards = true;
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
}
