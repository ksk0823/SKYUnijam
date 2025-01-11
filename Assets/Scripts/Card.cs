using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Card : MonoBehaviour
{
    public CardData data;

    public TMP_Text cardNameText;
    public TMP_Text cardDescriptionText;

    private void Awake()
    {
        cardNameText = transform.Find("Card Name").GetComponent<TMP_Text>();
        cardDescriptionText = transform.Find("Description").GetComponent<TMP_Text>();
    }

    public void SetCardData(CardData newData)
    {
        data = newData;

        if (cardNameText != null)
        {
            cardNameText.text = data.cardName;
            cardNameText.color = Color.white;
            cardNameText.alpha = 1f;
            //Debug.Log($"Card Name : {cardNameText.text}");
        }

        if (cardDescriptionText != null)
        {
            cardDescriptionText.text = data.description;
            cardDescriptionText.color = Color.white;
            cardDescriptionText.alpha = 1f;
            //Debug.Log($"Desciption : {cardDescriptionText.text}");
        }
    }

    public void Effect(GameObject player, GameObject unit)
    {
        Debug.Log(data.cardType);

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

            case CardData.CardType.AttackCount:
                PlayerAttackCountBuff(player, data.effectValue);
                break;

            case CardData.CardType.UnitMoveSpeed:
                AllySpeedBuff(unit, data.effectValue);
                break;
                
            case CardData.CardType.UnitAttackDamage:
                AllyAttackDamageBuff(unit, data.effectValue);
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
            if (stats.moveSpeed <= 10f)
            {
            stats.moveSpeed *= value;
            Debug.Log($"플레이어 속도 {value} 증가");
            }
            else
                Debug.Log($"너무 빨라요!");
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

    private void PlayerAttackCountBuff(GameObject player, float value)
    {
        var stats = player.GetComponent<PlayerMovement>();
        if (stats != null)
        {
            stats.clickDamage += value;
            Debug.Log($"플레이어 공격 횟수 {value} 증가");
        }
    }

    private void AllySpeedBuff(GameObject unit, float value)
    {
        var stats = unit.GetComponent<GameManager>();

        Debug.Log("아군 유닛이 이미 최대 속도에 도달했습니다.");

    }


    private void AllyAttackDamageBuff(GameObject unit, float value)
    {
        Debug.Log("유닛 데미지 증가");
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