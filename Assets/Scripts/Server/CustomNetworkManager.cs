using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Server has started successfully.");
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        Debug.Log("Server has stopped.");
    }
}
