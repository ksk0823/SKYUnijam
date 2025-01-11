using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Main Settings")]
    public float attackSpeed = 1f;
    public float moveSpeed = 5f;
    public float attackRadius = 2f;
    public float maxAttackDistance = 5f;
    public int attackDamage = 10;
    public LayerMask targetLayer;

    [Header("References")]
    private Rigidbody2D rb;
    private EUnitGroup unitGroup;
    private Transform currentTarget;
    private int nexusAttackCount = 0;

    private Collider2D[] hitColliders;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        unitGroup = GetComponent<UnitObject>().unitGroup;
    }

    private void Update()
    {
        DecideNextAction();
    }

    void DecideNextAction()
    {
        // 유닛 수 비교
        if (GameManager.instance.ActiveEnemyUnits > GameManager.instance.ActivePlayerUnits)
        {
            if (GameManager.instance.enemyNexusInteractionCount > GameManager.instance.playerNexusInteractionCount)
            {
                // 가장 가까운 플레이어 유닛 찾아 공격
                GameObject nearestUnit = FindNearestPlayerUnit();
                if (nearestUnit != null)
                {
                    MoveTowardsTarget(nearestUnit.transform);
                    AttackTarget(nearestUnit.transform);
                } else {
                    Debug.Log("플레이어 유닛 없음");
                }
            }
            else
            {
                // 자신의 넥서스 공격
                Transform nexus = GameManager.instance.enemyNexus.transform;
                MoveTowardsTarget(nexus);
                // 넥서스 구현 필요
                //AttackTarget(nexus);
                //nexusAttackCount++;
            }
        }
        else
        {
            // 중립 유닛 찾아 공격
            GameObject neutralUnit = FindNearestNeutralUnit();
            if (neutralUnit != null)
            {
                MoveTowardsTarget(neutralUnit.transform);
                //AttackTarget(neutralUnit.transform);
                // 중립 유닛은 공격 범위 내에 있으면 자동으로 채굴됨
            } else {
                Debug.Log("중립 유닛 없음");
            }
        }
    }

    GameObject FindNearestPlayerUnit()
    {
        float nearestDistance = float.MaxValue;
        GameObject nearestUnit = null;

        foreach (GameObject unit in GameManager.instance.playerUnits)
        {
            if (!unit.activeSelf) continue;
            
            float distance = Vector2.Distance(transform.position, unit.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestUnit = unit;
            }
        }
        return nearestUnit;
    }

    GameObject FindNearestNeutralUnit()
    {
        // NeutralUnit 태그를 가진 모든 오브젝트 찾기
        GameObject[] neutralUnits = GameObject.FindGameObjectsWithTag("Fixed");
        float nearestDistance = float.MaxValue;
        GameObject nearestUnit = null;

        foreach (GameObject unit in neutralUnits)
        {
            if (unit.GetComponent<UnitObject>().unitGroup != EUnitGroup.Neutral) continue;

            float distance = Vector2.Distance(transform.position, unit.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestUnit = unit;
            }
        }
        return nearestUnit;
    }

    void MoveTowardsTarget(Transform target)
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    void AttackTarget(Transform target)
    {
        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);
        if (distance <= maxAttackDistance)
        {
            // 가장 가까운 대상 하나만 찾아서 공격
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(target.position, attackRadius, targetLayer);
            
            if (hitColliders.Length > 0)
            {
                // 가장 가까운 대상 찾기
                float nearestDist = float.MaxValue;
                Collider2D nearestCollider = null;
                
                foreach (Collider2D collider in hitColliders)
                {
                    float dist = Vector2.Distance(transform.position, collider.transform.position);
                    if (dist < nearestDist)
                    {
                        nearestDist = dist;
                        nearestCollider = collider;
                    }
                }

                // 가장 가까운 대상만 공격
                if (nearestCollider != null)
                {
                    NeutralUnit targetUnit = nearestCollider.GetComponent<NeutralUnit>();
                    if (targetUnit != null && targetUnit.unitGroup != unitGroup)
                    {
                        targetUnit.Damage(attackDamage);
                    }
                }
            }
        }
    }

    

    int GetPlayerNexusAttackCount()
    {
        // 플레이어의 넥서스 공격 횟수를 가져오는 로직 구현 필요
        // GameManager나 다른 시스템에서 관리하도록 수정 필요
        return 0;
    }
}
