using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EndingScene : MonoBehaviour
{
    [Header("Objects")]
    public GameObject winImageObject;
    public TMP_Text ResultText;
    public TMP_Text DescriptionText;
    private Image winImage;
    public Button backToTitleButton;
    [Header("Texts")]
    public string[] winDescriptionTexts;
    public string[] loseDescriptionTexts;
    [Header("Images")]  
    public Sprite[] winImages;
    public Sprite[] loseImages;
    
    void Awake()
    {
        winImage = winImageObject.GetComponent<Image>();
        if (PlayerPrefs.GetInt("GameResult") == 1)
        {
            winImage.sprite = winImages[PlayerPrefs.GetInt("PlayerCharacter")];
            ResultText.text = "VICTORY";
            DescriptionText.text = winDescriptionTexts[PlayerPrefs.GetInt("PlayerCharacter")];
        } else
        {
            winImage.sprite = loseImages[PlayerPrefs.GetInt("PlayerCharacter")];
            ResultText.text = "DEFEAT";
            DescriptionText.text = loseDescriptionTexts[PlayerPrefs.GetInt("PlayerCharacter")];

        }
    }

    public void OnTitleButtonClicked()
    {
        SceneManager.LoadScene("FinalTitleScene");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
