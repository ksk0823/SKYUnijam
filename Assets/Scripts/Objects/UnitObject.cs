using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum EUnitGroup
{
    //아군 플레이어
    Allay,

    //상대 플레이어
    Enemy,
}



public class UnitObject : MonoBehaviour
{
    public EUnitGroup unitGroup;

    public PhotonView pv;

    protected bool _isInit = false;

    protected virtual void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    public virtual void Init(EUnitGroup unitGroup)
    {
        unitGroup = unitGroup;
        _isInit = true;
    }

}
