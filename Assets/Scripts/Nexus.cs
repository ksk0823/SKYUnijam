using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nexus : MonoBehaviour
{
    [Header("Main Settings")]
    public int health;
    public int interactionTime;

    [Header("Objects")]
    public GameObject cardGroup;
    public GameObject player;
    public GameObject unit;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            health--;
            Debug.Log($"Nexus Health : {health}");
        }
    }

    public void ShowCardUI()
    {
        cardGroup.SetActive(true);
    }

    public void HideCardUI()
    {
        cardGroup.SetActive(false);
    }

    public void OnCardSelected(Card selectedCard)
    {
        selectedCard.Effect(player, unit);

        Debug.Log($"{selectedCard.data.cardName} 카드가 적용되었습니다!");
        HideCardUI();
    }
}
