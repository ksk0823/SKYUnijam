using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalTitleManager : MonoBehaviour
{
    public static FinalTitleManager instance {get; private set;}
    [Header("UI Panels")]
    public GameObject TitlePanel;
    public GameObject GameReadyPanel;
    
    [Header("Player Character")]
    public Image PlayerCharacterImage;
    public Image ComputerCharacterImage;
    public TMP_Text PlayerCharacterText;
    public TMP_Text PlayerDescriptionText;

    public Sprite[] CharacterSprites;
    
    [Header("Ready State")]
    public Button PlayerReadyButton;
    public TMP_Text ComputerReadyText;
    public GameObject CharacterSelectPanel;
    
    [Header("Sound")]
    public AudioClip clickSound;
    private AudioSource audioSource;

    private bool isPlayerReady;
    public int playerCharacterIndex;
    public int computerCharacterIndex;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        InitializeGame();
        instance = this;
    }

    private void InitializeGame()
    {
        /*
        isPlayerReady = false;
        playerCharacterIndex = 0;
        computerCharacterIndex = Random.Range(0, CharacterSprites.Length);
        
        if (PlayerCharacterImage != null && CharacterSprites.Length > 0)
        {
            PlayerCharacterImage.sprite = CharacterSprites[0];
        }
        
        if (ComputerCharacterImage != null)
        {
            ComputerCharacterImage.sprite = CharacterSprites[computerCharacterIndex];
        }
        
        if (ComputerReadyText != null)
        {
            ComputerReadyText.text = "Ready";
        }
        */
    }

    #region UI Callbacks
    public void OnTitleStartButtonClicked()
    {
        SetActivePanel(GameReadyPanel.name);
    }

    public void OnBackButtonClicked()
    {
        SetActivePanel(TitlePanel.name);
        InitializeGame();
    }

    public void OnReadyButtonClicked()
    {
        isPlayerReady = !isPlayerReady;
        UpdateReadyState();
    }

    public void OnCharacterSelectButtonClicked(int index)
    {
        if (index < 0 || index >= CharacterSprites.Length) return;
        
        //playerCharacterIndex = index;
        PlayerCharacterImage.sprite = CharacterSprites[index];
    }

    public void OnStartGameButtonClicked()
    {
        if (isPlayerReady)
        {
            Debug.Log("PlayerCharacter: " + playerCharacterIndex);
            PlayerPrefs.SetInt("PlayerCharacter", playerCharacterIndex);
            PlayerPrefs.SetInt("ComputerCharacter", computerCharacterIndex);
            SceneManager.LoadScene("SingleCombat");
        }
    }
    #endregion

    private void UpdateReadyState()
    {
        
        if (CharacterSelectPanel != null)
        {
            CharacterSelectPanel.SetActive(!isPlayerReady);
        }
    }

    private void SetActivePanel(string activePanel)
    {
        PlaySound(clickSound);
        TitlePanel.SetActive(activePanel.Equals(TitlePanel.name));
        GameReadyPanel.SetActive(activePanel.Equals(GameReadyPanel.name));
    }

    private void PlaySound(AudioClip sound)
    {
        if (audioSource != null && sound != null)
        {
            audioSource.PlayOneShot(sound);
        }
    }

    public void SetPlayerCharacter(int index)
    {
        playerCharacterIndex = index;
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
