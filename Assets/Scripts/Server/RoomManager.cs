using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; //Mirror API를 사용하기 위한 using선언

//MonoBehabior 대신, API사용을 위해 NetworkRoomManager를 상속
 public  class RoomManager : NetworkRoomManager 
{
     private int connectedPlayers = 0; // 현재 연결된 플레이어 수

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);

        connectedPlayers++; // 플레이어 수 증가
        Debug.Log($"Player connected. Total players: {connectedPlayers}");

        // 2명이 연결되었을 때 게임 시작
        if (connectedPlayers == 2)
        {
            StartGame();
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);

        connectedPlayers--; // 플레이어 수 감소
        Debug.Log($"Player disconnected. Total players: {connectedPlayers}");
    }

    private void StartGame()
    {
        Debug.Log("2 Players connected. Starting the game!");
        // 게임 시작 로직 실행
        // 예: 특정 씬 로드, 초기화 등

        ServerChangeScene("PlayScene"); // GameScene으로 이동

    }
    // public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    // {
    // // PlayerPrefab을 수동으로 생성
    // GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity); // 원하는 위치 지정 가능
    // NetworkServer.AddPlayerForConnection(conn, player);

    // Debug.Log($"Player added for connection: {conn.connectionId}");
    // }
}