using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Main Settings")]
    public float attackSpeed;
    public float moveSpeed;

    [Header("Click Attack Settings")]
    public LayerMask enemyLayer;
    public float clickDamage = 10;
    public float attackRadius = 2f; // 원형 범위 크기
    public float maxAttackDistance = 5f;

    [Header("Objects")]
    Rigidbody2D rb;
    Collider2D coll;

    private Vector2 inputVec;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
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

    void ClickAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            worldPosition.z = 0f; // 2D 환경에서 Z축 제거

            float distanceToPlayer = Vector3.Distance(transform.position, worldPosition);

            if (distanceToPlayer <= maxAttackDistance)
            {
                DealDamageInArea(worldPosition);
            }
        }
    }

    private void DealDamageInArea(Vector3 center)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, attackRadius, enemyLayer);

        foreach (Collider2D collider in hitColliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();

            if (enemy != null)
            {
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
            // 플레이어 기준 최대 공격 거리 원 표시
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, maxAttackDistance);
        }
    }

    void OnMove(InputValue value)
    {
        //Debug.Log("OnMove");
        inputVec = value.Get<Vector2>();
    }
}
