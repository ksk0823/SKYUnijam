using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public void Effect(GameObject player, List<GameObject> units)
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
                AllySpeedBuff(units, data.effectValue);
                break;
                
            case CardData.CardType.UnitAttackDamage:
                AllyAttackDamageBuff(units, data.effectValue);
                break;

            case CardData.CardType.UnitHealth:
                AllyHealthBuff(units, data.effectValue);
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
        if (stats != null && stats.moveSpeed < 10)
        {
            stats.moveSpeed *= value;
            Debug.Log($"플레이어 이동 속도가 {value}배 증가했습니다.");
        }
        else
        {
            Debug.Log("플레이어 이동 속도가 이미 최대치입니다.");
        }
    }

    private void PlayerAttackSpeedBuff(GameObject player, float value)
    {
        var stats = player.GetComponent<PlayerMovement>();
        if (stats != null)
        {
            stats.attackSpeed *= value;
            Debug.Log($"플레이어 공격 속도가 {value}배 증가했습니다.");
        }
    }

    private void PlayerAttackRangeBuff(GameObject player, float value)
    {
        var stats = player.GetComponent<PlayerMovement>();
        if (stats != null)
        {
            stats.maxAttackDistance *= value;
            Debug.Log($"플레이어 공격 범위가 {value}배 증가했습니다.");
        }
    }

    private void PlayerAttackCountBuff(GameObject player, float value)
    {
        var stats = player.GetComponent<PlayerMovement>();
        if (stats != null)
        {
            stats.clickDamage += value;
            Debug.Log($"플레이어 공격 횟수가 {value}만큼 증가했습니다.");
        }
    }

    private void AllySpeedBuff(List<GameObject> units, float value)
    {
        foreach (GameObject unit in units)
        {
            NeutralUnit neutralUnit = unit.GetComponent<NeutralUnit>();
            neutralUnit.moveSpeed *= value;
        }    
        Debug.Log("유닛 이동속도 증가");
    }

    private void AllyAttackDamageBuff(List<GameObject> units, float value)
    {
        foreach (GameObject unit in units)
        {
            NeutralUnit neutralUnit = unit.GetComponent<NeutralUnit>();
            // 데미지 적용하기
        }
        Debug.Log("유닛 데미지 증가");
    }

    private void AllyHealthBuff(List<GameObject> units, float value)
    {
        foreach (GameObject unit in units)
        {
            NeutralUnit neutralUnit = unit.GetComponent<NeutralUnit>();
            neutralUnit.health *= value;
        }
        Debug.Log("유닛 체력 증가");
    }

    private void PlayerMineSpeedBuff(GameObject player, float value)
    {
        Debug.Log("플레이어 채굴속도 감소");
    }
}