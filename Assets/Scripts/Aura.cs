using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : MonoBehaviour
{
    public Sprite[] sprites;

    SpriteRenderer sr;
    GameObject parent;
    float auraRadius;
    public float scaleMultiplier = 1f;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        parent = transform.parent.gameObject;
        auraRadius = parent.GetComponent<PlayerMovement>().maxAttackDistance;
    }

    private void Start()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 newScale = originalScale * auraRadius;
        transform.localScale = newScale;
    }

    private void Update()
    {
        SetAura();
        SetAuraRadius();
    }

    void SetAura()
    {
        if (parent.name == "Player")
        {
            int parentIndex = GameManager.instance.playerCharacterIndex;
            sr.sprite = sprites[parentIndex];
        }
        else if (parent.name == "Enemy")
        {
            int parentIndex = GameManager.instance.computerCharacterIndex;
            sr.sprite = sprites[parentIndex];
        }
    }

    void SetAuraRadius()
    {
        auraRadius = parent.GetComponent<PlayerMovement>().maxAttackDistance;
        float newScale = auraRadius * scaleMultiplier;
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}
