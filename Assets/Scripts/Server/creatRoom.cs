using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createRoom : MonoBehaviour
{
   public void CreateRoom(){
    var manager = RoomManager.singleton; 
    //방 설정 작업
    //추후에 여기에서 방에 대한 설정작업이 이루어질것이다.
    manager.StartHost();
}   
}
