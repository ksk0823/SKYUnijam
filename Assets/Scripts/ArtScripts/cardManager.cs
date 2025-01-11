using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public CardAnim[] cards;
    public float cardTimer;
    public bool showCards = false;
    public float cardInterval = 5f;
    public bool isInsideTrigger = false;
    public bool isEnemy = false;

    [Header("Card Data List")]
    public List<CardData> allCards; // CardData ScriptableObject 리스트

    [Header("Card UI References")]
    public List<Card> cardSlots; // Card 오브젝트 (UI 슬롯)

    private void Update()
    {
        if (isEnemy)
        {
            EnemyCardPick();
        }
        else if (showCards)
        {
            ShowCards();
            showCards = false;
        }
    }

    void EnemyCardPick()
    {
        if (allCards.Count < 3)
        {
            Debug.Log("카드 데이터 3개 미만");
            return;
        }

        List<CardData> selectedCards = new List<CardData>();
        HashSet<int> usedIndices = new HashSet<int>();

        while (selectedCards.Count < 3)
        {
            int randomIndex = Random.Range(0, allCards.Count);
            if (!usedIndices.Contains(randomIndex))
            {
                usedIndices.Add(randomIndex);
                selectedCards.Add(allCards[randomIndex]);
            }
        }

        for (int i = 0; i < cardSlots.Count; i++)
        {
            if (i < selectedCards.Count)
            {
                cardSlots[i].data = selectedCards[i];
            }
        }

        Card xData = cardSlots[0];
        xData.Effect(GameManager.instance.enemyCharacter, GameManager.instance.enemyUnits);
    }

    void ShowCards()
    {
        foreach (CardAnim card in cards)
        {
            card.Init();
        }
        FindObjectOfType<CardManager>().GenerateRandomCards();
    }

    public void GenerateRandomCards()
    {
        if (allCards.Count < 3)
        {
            Debug.Log("카드 데이터 3개 미만"); // 이거 나오면 안됨. 예외처리 필요?
            return;
        }

        List<CardData> selectedCards = new List<CardData>();
        HashSet<int> usedIndices = new HashSet<int>();

        while (selectedCards.Count < 3)
        {
            int randomIndex = Random.Range(0, allCards.Count);
            if (!usedIndices.Contains(randomIndex))
            {
                usedIndices.Add(randomIndex);
                selectedCards.Add(allCards[randomIndex]);
            }
        }

        // UI 슬롯에 카드 데이터 적용
        for (int i = 0; i < cardSlots.Count; i++)
        {
            if (i < selectedCards.Count)
            {
                cardSlots[i].data = selectedCards[i];
                ApplyCardUI(cardSlots[i], selectedCards[i]);
            }
        }
    }
    
    private void ApplyCardUI(Card card, CardData data)
    {
        card.SetCardData(data);

        Debug.Log($"카드 {data.cardName} UI 적용됨");
    }

    public void dissolveCards()
    {
        for (int i = 0; i < 3; i ++) {
            cards[i].textDissapear();

            if(!cards[i].clicked)
                cards[i].dissolve(); // 불타 없어지는 이펙트
        }
    }
}
