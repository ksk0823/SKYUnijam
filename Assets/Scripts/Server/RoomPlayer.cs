using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; //Mirror API 사용을 위한 using선언

//RoomPlayer는 MonoBehabiour대신, NetworkRoomPlayer를 상속받는다.
public class RoomPlayer : NetworkRoomPlayer
{
     //Start와 Update는 지워도 된다.
     //추후에 필요한 기능이 있다면 여기 넣으면 된다.
}