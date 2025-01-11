using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public CardData data;
    
    public void Effect(GameObject player, GameObject unit)
    {
        switch (data.cardType)
        {
            case CardData.CardType.MoveSpeed:
                PlayerSpeedBuff(player, data.effectValue);
                break;

            case CardData.CardType.AttackSpeed:
                PlayerAttackSpeedBuff(player, data.effectValue);
                break;

            case CardData.CardType.AttackRange:
                PlayerAttackRangeBuff(player, data.effectValue);
                break;

            //case CardData.CardType.AttackCount:
            //    PlayerAttackCountBuff(player, data.effectValue);
            //    break;

            case CardData.CardType.UnitMoveSpeed:
                AllySpeedBuff(unit, data.effectValue);
                break;

            case CardData.CardType.UnitAttackSpeed:
                AllyAttackSpeedBuff(unit, data.effectValue);
                break;

            case CardData.CardType.UnitHealth:
                AllyHealthBuff(unit, data.effectValue);
                break;

            case CardData.CardType.MineSpeed:
                PlayerMineSpeedBuff(player, data.effectValue);
                break;

            default:
                Debug.LogError("Invalid Card Type!");
                break;
        }
    }

    private void PlayerSpeedBuff(GameObject player, float value)
    {
        var stats = player.GetComponent<PlayerMovement>();

        if (stats != null)
        {
            stats.moveSpeed *= value;
            Debug.Log($"플레이어 속도 {value} 증가");
        }
    }

    private void PlayerAttackSpeedBuff(GameObject player, float value)
    {
        var stats = player.GetComponent<PlayerMovement>();

        if (stats != null)
        {
            stats.attackSpeed *= value;
            Debug.Log($"플레이어 공격속도 {value} 증가");
        }
    }

    private void PlayerAttackRangeBuff(GameObject player, float value)
    {
        var stats = player.GetComponent<PlayerMovement>();

        if (stats != null)
        {
            stats.maxAttackDistance *= value;
            Debug.Log($"플레이어 공격 범위 {value} 증가");
        }
    }

    //private void PlayerAttackCountBuff(GameObject player, float value)
    //{
    //    var stats = player.GetComponent<PlayerMovement>();

    //    if (stats != null)
    //    {
    //        stats.attackCount += (int)value;
    //        Debug.Log($"Player attack count increased by {(int)value}");
    //    }
    //}

    private void AllySpeedBuff(GameObject unit, float value)
    {
        Debug.Log("유닛 이동속도 증가");
    }

    private void AllyAttackSpeedBuff(GameObject unit, float value)
    {
        Debug.Log("유닛 공격속도 증가");
    }

    private void AllyHealthBuff(GameObject unit, float value)
    {
        Debug.Log("유닛 체력 증가");
    } 

    private void PlayerMineSpeedBuff(GameObject unit, float value)
    {
        Debug.Log("플레이어 채굴속도 감소");
        // interactionTime을 NeutralUnit에서 관리하고 있음
    }
}
