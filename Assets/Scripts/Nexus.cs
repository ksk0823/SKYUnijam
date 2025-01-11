using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nexus : UnitObject
{
    [Header("Main Settings")]
    public int health;
    public int interactionTime;

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
}
