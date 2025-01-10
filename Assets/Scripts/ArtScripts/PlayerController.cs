using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        if (!isLocalPlayer) return;

        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Translate(moveX, moveY, 0);
    }
}
