using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Main Settings")]
    public int health;

    public void Damage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("Enemy Dead");
            gameObject.SetActive(false);
        }
    }
}
