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
    public bool isPlayer;

    // 스킬 List 필요
    
    private void Awake()
    {
        if (!isPlayer)
        {
           Init(EUnitGroup.Enemy);
           //GetComponent<PlayerMovement>().enabled = false;
           //GetComponent<PlayerInput>().enabled = false;
        } else
        {
            Init(EUnitGroup.Allay);
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
