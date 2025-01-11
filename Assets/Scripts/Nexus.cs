using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nexus : UnitObject
{
    [Header("Main Settings")]
    public int health;
    public int interactionTime;

    protected override void Awake()
    {
        base.Awake();
        if (!pv.IsMine)
        {
            unitGroup = EUnitGroup.Enemy;
        } else
        {
            unitGroup = EUnitGroup.Allay;
        }
    }
}
