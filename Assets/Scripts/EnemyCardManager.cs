using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCardManager : MonoBehaviour
{
    public float cardTimer;
    public float cardInterval = 5f;
    
    public TMP_Text cardName;
    public TMP_Text description;

    [Header("Card Data List")]
    public List<CardData> allCards;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetCardData();
        }
    }

    private void SetCardData()
    {
        int randIndex = Random.Range(0, allCards.Count);
        CardData data = allCards[randIndex];

        cardName.text = data.cardName;
        description.text = data.description;
    }


}
