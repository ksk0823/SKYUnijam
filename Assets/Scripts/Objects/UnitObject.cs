using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EUnitGroup
{
    //아군 플레이어
    Allay,
    Neutral,

    //상대 플레이어
    Enemy,
}

public class UnitObject : MonoBehaviour
{
    public EUnitGroup unitGroup;

    protected bool _isInit = false;

    public void Init(EUnitGroup unitGroup)
    {
        unitGroup = unitGroup;
        _isInit = true;
    }

}
