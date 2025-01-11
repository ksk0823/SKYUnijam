using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Nexus : UnitObject
{
    [Header("Main Settings")]
    public float health;
    public int interactionTime;

    public TMP_Text healthText;

    public bool isPlayer;

    private void Awake()
    {
        if (!isPlayer)
        {
           Init(EUnitGroup.Enemy);
        } else
        {
            Init(EUnitGroup.Allay);
        }
    }

    public void Damage(float damage)
    {
        health -= damage;
        healthText.text = health.ToString();
        if(health <= 0)
        {
            //GameManager.instance.GameOver();
            Debug.Log("Nexus Destroyed");
        }
    }
}
