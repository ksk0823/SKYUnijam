using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Object/CardData")]
public class CardData : ScriptableObject
{
    public enum CardType
    {
        MoveSpeed,
        AttackSpeed,
        AttackRange,
        AttackCount,
        UnitMoveSpeed,
        UnitAttackSpeed,
        UnitHealth,
        MineSpeed
    }

    [Header("Main Info")]
    public CardType cardType;
    public Sprite cardImage;
    public string cardName;
    public string description;
    public float effectValue; // 카드 효과 값
}

