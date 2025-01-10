using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;


public class PlayerObject : UnitObject
{
    public int index;
    public PlayerMovement pm;
    public GameObject neutralUnitPrefab;
    public GameObject UnitsObjects;
    public List<GameObject> neutralUnitList;

    // 스킬 List 필요
    
    protected override void Awake()
    {
        base.Awake();
        if (!pv.IsMine)
        {
            GetComponent<PlayerInput>().enabled = false;
            unitGroup = EUnitGroup.Enemy;
        } else
        {
            unitGroup = EUnitGroup.Allay;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
