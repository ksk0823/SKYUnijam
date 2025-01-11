using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoDisplay : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text PlayerNameText;
    
    [Header("Player UI")]
    public Button ReadyButton;  // 플레이어에게만 표시
    public TMP_Text ReadyStateText;  // 컴퓨터에게만 표시
    
    [Header("Character Selection")]
    public GameObject CharacterSelectPanel;
    public Image CharacterImage;
    public Sprite[] CharacterSprites;
    
    private bool isPlayerReady;
    private int characterIndex;
    private bool isComputer;
    
    private void Start()
    {
        if (isComputer)
        {
            // 컴퓨터인 경우 컨트롤 비활성화
            if (ReadyButton != null)
                ReadyButton.gameObject.SetActive(false);
            if (CharacterSelectPanel != null)
                CharacterSelectPanel.SetActive(false);
                
            // 컴퓨터는 항상 준비된 상태
            SetPlayerReady(true);
            // 랜덤한 캐릭터 선택
            SetPlayerCharacter(Random.Range(0, CharacterSprites.Length));
        }
        else
        {
            // 플레이어인 경우
            if (ReadyButton != null)
                ReadyButton.gameObject.SetActive(true);
            if (CharacterSelectPanel != null)
                CharacterSelectPanel.SetActive(true);
        }
    }
    
    public void Initialize(string playerName)
    {
        PlayerNameText.text = playerName;
        characterIndex = 0;
        isPlayerReady = false;
        isComputer = playerName.Equals("Computer");
        
        if (CharacterImage != null && CharacterSprites.Length > 0)
        {
            CharacterImage.sprite = CharacterSprites[0];
        }
    }

    public void OnReadyButtonClicked()
    {
        isPlayerReady = !isPlayerReady;
        SetPlayerReady(isPlayerReady);
        
        if (CharacterSelectPanel != null)
            CharacterSelectPanel.SetActive(!isPlayerReady);
    }

    public void OnSelectButtonClicked(int index)
    {
        if (index < 0 || index >= CharacterSprites.Length) return;
        
        characterIndex = index;
        SetPlayerCharacter(characterIndex);
    }
    
    public void SetPlayerReady(bool playerReady)
    {
        isPlayerReady = playerReady;
        
        if (ReadyButton != null)
        {
            ReadyButton.GetComponentInChildren<TMP_Text>().text = 
                playerReady ? "Ready!" : "Ready?";
        }
        
        if (ReadyStateText != null)
        {
            ReadyStateText.text = playerReady ? "Ready" : "Not Ready";
        }
    }
    
    public void SetPlayerCharacter(int index)
    {
        if (index < 0 || index >= CharacterSprites.Length) return;
        
        characterIndex = index;
        CharacterImage.sprite = CharacterSprites[index];
    }
    
    public bool IsReady()
    {
        return isPlayerReady;
    }
    
    public int GetCharacterIndex()
    {
        return characterIndex;
    }
}
