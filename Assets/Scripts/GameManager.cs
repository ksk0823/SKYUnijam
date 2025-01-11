using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PoolManager pool;
    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }
}
