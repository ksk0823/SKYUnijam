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
    
    public Button ReadyButton;
    public TMP_Text ReadyStateText;
    
    public Button CharacterSelectButton;
    public Image CharacterImage;
    public Sprite[] CharacterSprites;
    
    private int ownerId;
    private bool isPlayerReady;
    private int characterIndex;
    
    #region Unity Callbacks
    
    public void OnEnable()
    {
        PlayerNumbering.OnPlayerNumberingChanged += OnPlayerNumberingChanged;
    }

    public void OnDisable()
    {
        PlayerNumbering.OnPlayerNumberingChanged -= OnPlayerNumberingChanged;
    }

    private void Start()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
        {
            ReadyButton.gameObject.SetActive(false);
            CharacterSelectButton.gameObject.SetActive(false);
        }
        else
        {
            // 초기 속성만 설정
        Hashtable initialProps = new Hashtable() {
            {"IsReady", isPlayerReady}, 
            {"CharacterIndex", characterIndex}
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
        PhotonNetwork.LocalPlayer.SetScore(0);
            /*
            // 초기 속성 설정
            Hashtable initialProps = new Hashtable() {{"IsReady", isPlayerReady}, 
                {"CharacterIndex", characterIndex}};
            PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
            PhotonNetwork.LocalPlayer.SetScore(0);

            // Ready 버튼 리스너 설정
            ReadyButton.onClick.AddListener(() =>
            {
                isPlayerReady = !isPlayerReady;
                SetPlayerReady(isPlayerReady);

                Hashtable props = new Hashtable() {{"IsReady", isPlayerReady}, 
                    {"CharacterIndex", characterIndex}};
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                if (PhotonNetwork.IsMasterClient)
                {
                    FindObjectOfType<TitleManager>().LocalPlayerPropertiesUpdated();
                }
            });
            
            // 캐릭터 선택 버튼 리스너 설정
            CharacterSelectButton.onClick.AddListener(() =>
            {
                if (characterIndex < 2)
                {
                    characterIndex++;
                }
                else
                {
                    characterIndex = 0;
                }
                
                SetPlayerCharacter(characterIndex);

                Hashtable props = new Hashtable() {{"IsReady", isPlayerReady}, 
                    {"CharacterIndex", characterIndex}};
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                if (PhotonNetwork.IsMasterClient)
                {
                    FindObjectOfType<TitleManager>().LocalPlayerPropertiesUpdated();
                }
            });
            */
        }
        
    }

    #endregion
    
    public void Initialize(int playerId, string playerName)
    {
        ownerId = playerId;
        PlayerNameText.text = playerName;
        characterIndex = 0;
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

    public void OnSelectButtonClicked()
    {
        Debug.Log("Select Button Clicked");
        // Select 버튼 클릭 시 동작
        characterIndex = (characterIndex + 1) % 3;
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
        ReadyButton.GetComponentInChildren<TMP_Text>().text = playerReady ? "Ready!" : "Ready?";
        ReadyStateText.text = playerReady ? "Ready" : "Not Ready";
        if (playerReady==true && PhotonNetwork.LocalPlayer.ActorNumber == ownerId)
        {
            CharacterSelectButton.gameObject.SetActive(false);
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == ownerId)
        {
            CharacterSelectButton.gameObject.SetActive(true);
        }
    }
    
    public void SetPlayerCharacter(int index)
    {
        CharacterImage.sprite = CharacterSprites[index];
    }


    
}
