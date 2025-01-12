using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Threading;

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
    public GameObject ComputerCharacterInfo;
    public GameObject startGameButton;

    [Header("Tutorial")]
    public GameObject TutorialPanel;
    public Button next;
    public Button start;

    [Header("Text Index")]
    public Image[] tuto;

    [Header("Sound")]
    public AudioClip clickSound;
    private AudioSource audioSource;
    int count = 0;
    public bool isPlayerReady;
    public int playerCharacterIndex;
    public int computerCharacterIndex;

    private void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            tuto[i].gameObject.SetActive(false);
        }
        audioSource = GetComponent<AudioSource>();
        instance = this;
        next.gameObject.SetActive(false);
        start.gameObject.SetActive(false);
    }

    private void SelectComputerCharacter()
    {
        computerCharacterIndex = Random.Range(0, 3);
        while(computerCharacterIndex == playerCharacterIndex)
        {
            computerCharacterIndex = Random.Range(0, 3);
        }
        Debug.Log("Computer Character Index: " + computerCharacterIndex);

        CharacterChanger characterChanger = ComputerCharacterInfo.GetComponent<CharacterChanger>();
        characterChanger.imagechange(computerCharacterIndex);
        characterChanger.getReady();

        Thread.Sleep(30);
        Tutorial();
    }
    public void Tutorial()
    {
        if (count == 0)
            next.gameObject.SetActive(true);
        if (count != 0)
        {
            tuto[count-1].gameObject.SetActive(false);
        }
        tuto[count].gameObject.SetActive(true);
        count++;
        if (count == 4)
        {
            next.gameObject.SetActive(false);
            start.gameObject.SetActive(true);
        }
    }

    #region UI Callbacks
    public void OnTitleStartButtonClicked()
    {
        SetActivePanel(GameReadyPanel.name);
    }

    public void OnBackButtonClicked()
    {
        SetActivePanel(TitlePanel.name);
    }

    public void OnReadyButtonClicked()
    {
        isPlayerReady = !isPlayerReady;
        SelectComputerCharacter();
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
            SceneManager.LoadScene("BackUp");
        }
    }
    #endregion

    private void UpdateReadyState()
    {
    
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
