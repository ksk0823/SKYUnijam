using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Pun.UtilityScripts;

public class PlayerListEntry : MonoBehaviourPunCallbacks
{
    [Header("UI References")]
    public TMP_Text PlayerNameText;
    
    [Header("Player UI")]
    public Button ReadyButton;  // PlayerInfo에만 있음
    
    [Header("Enemy UI")]
    public TMP_Text ReadyStateText;  // EnemyInfo에만 있음
    
    public GameObject CharacterSelectPanel;
    public Image CharacterImage;
    public Sprite[] CharacterSprites;
    
    private int ownerId;
    private bool isPlayerReady;
    private int characterIndex;
    
    #region Unity Callbacks
    
    private void Start()
    {
        // 자신의 PlayerInfo가 아닌 경우 컨트롤 비활성화
        if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
        {
            if (ReadyButton != null)
                ReadyButton.gameObject.SetActive(false);
            if (CharacterSelectPanel != null)
                CharacterSelectPanel.SetActive(false);
        }
        else
        {
            if (ReadyButton != null)
                ReadyButton.gameObject.SetActive(true);
            if (CharacterSelectPanel != null)
                CharacterSelectPanel.SetActive(!isPlayerReady); // Ready 상태에 따라 설정
            
            // 초기 속성 설정
            Hashtable initialProps = new Hashtable() {
                {"IsReady", isPlayerReady}, 
                {"CharacterIndex", characterIndex}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
            PhotonNetwork.LocalPlayer.SetScore(0);
        }
    }

    #endregion
    
    public void Initialize(int playerId, string playerName)
    {
        ownerId = playerId;
        PlayerNameText.text = playerName;
        characterIndex = 0;
        isPlayerReady = false; // 초기화 시 Ready 상태를 false로 설정
        
        // UI 컨트롤 상태 업데이트
        if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
        {
            if (ReadyButton != null)
                ReadyButton.gameObject.SetActive(false);
            if (CharacterSelectPanel != null)
                CharacterSelectPanel.SetActive(false);
        }
        else
        {
            if (ReadyButton != null)
            {
                ReadyButton.gameObject.SetActive(true);
                ReadyButton.GetComponentInChildren<TMP_Text>().text = "Ready?";
            }
            if (CharacterSelectPanel != null)
                CharacterSelectPanel.SetActive(true); // 항상 true로 시작
        }
    }
    
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer.ActorNumber == ownerId && changedProps.ContainsKey("IsReady")
            && changedProps.ContainsKey("CharacterIndex"))
        {
            SetPlayerReady((bool)changedProps["IsReady"]);
            SetPlayerCharacter((int)changedProps["CharacterIndex"]);
        }
    }
    
    private void OnPlayerNumberingChanged()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.ActorNumber == ownerId)
            {
                // 여기서 플레이어 번호에 따른 추가적인 처리 가능
                // ex) 플레이어 색상 설정, 이미지 변경 등
                break;
            }
        }
    }

    public void OnReadyButtonClicked()
    {
        Debug.Log("Ready Button Clicked");
        isPlayerReady = !isPlayerReady;
        SetPlayerReady(isPlayerReady);

        Hashtable props = new Hashtable() {{"IsReady", isPlayerReady}, 
        {"CharacterIndex", characterIndex}};
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        if (PhotonNetwork.IsMasterClient)
        {
            FindObjectOfType<TitleManager>().LocalPlayerPropertiesUpdated();
        }
    }

    public void OnSelectButtonClicked(int index)
    {
        Debug.Log("Select Button Clicked");
        // Select 버튼 클릭 시 동작
        characterIndex = index;
        Debug.Log("Character Index: " + characterIndex);
                
        SetPlayerCharacter(characterIndex);

        Hashtable props = new Hashtable() {{"IsReady", isPlayerReady}, 
            {"CharacterIndex", characterIndex}};
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        if (PhotonNetwork.IsMasterClient)
        {
             FindObjectOfType<TitleManager>().LocalPlayerPropertiesUpdated();
        }
    }
    
    public void SetPlayerReady(bool playerReady)
    {
        isPlayerReady = playerReady;
        
        // PlayerInfo의 경우 (ReadyButton이 있는 경우)
        if (ReadyButton != null)
        {
            ReadyButton.GetComponentInChildren<TMP_Text>().text = playerReady ? "Ready!" : "Ready?";
        }
        
        // EnemyInfo의 경우 (ReadyStateText가 있는 경우)
        if (ReadyStateText != null)
        {
            ReadyStateText.text = playerReady ? "Ready" : "Not Ready";
        }
            
        // 자신의 PlayerInfo인 경우에만 CharacterSelectPanel 상태를 변경
        if (PhotonNetwork.LocalPlayer.ActorNumber == ownerId)
        {
            if (CharacterSelectPanel != null)
                CharacterSelectPanel.SetActive(!playerReady);
        }
    }
    
    public void SetPlayerCharacter(int index)
    {
        CharacterImage.sprite = CharacterSprites[index];
    }


    
}
