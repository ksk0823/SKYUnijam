using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class VictoryorDefeatPannel : MonoBehaviour
{
    // public GameObject playerVS;
    // public GameObject AIVS;
    public bool win = true;
    public bool gameEND = false;
    public Sprite[] MainImgs = new Sprite[6];
    public TMP_Text MainTxt;
    public TMP_Text bonmoon;
    public Image img;

    void Decide()
    {
        this.gameObject.SetActive(true);
        if(win){
            MainTxt.text = "VICTORY";
             switch(PlayerPrefs.GetInt("PlayerCharacter"))
        {
            case 0:
                img.sprite = MainImgs[0];
                bonmoon.text = "찬란한 승리의 광휘는 루비에게 돌아가고, 심석의 정원은 타오르는 들불처럼 붉게 물든다.";
                break;
            case 1:
                img.sprite = MainImgs[1];
                bonmoon.text = "찬란한 승리의 광휘는 사파이어에게 돌아가고, 심석의 정원은 바다와 같은 푸른색으로 조용히 물든다.";
                break;
            case 2:
                img.sprite = MainImgs[2];
                bonmoon.text = "찬란한 승리의 광휘는 에메랄드에게 돌아가고, 심석의 정원은 슾의 상록으로 깊게물든다.";
                break; 
        }
        }
        else{
            MainTxt.text = "DEFEAT";
            bonmoon.text = "영광의 빛 이면에는 길고 긴 그림자가 남는다. 정원에서의 패배는 조각난 파편만을 의미한다";
        switch(PlayerPrefs.GetInt("PlayerCharacter"))
        {
            case 0:
                img.sprite = MainImgs[3];
                break;
            case 1:
                img.sprite = MainImgs[4];
                
                break;
            case 2:
                img.sprite = MainImgs[5];

                break;
        }
        }
        
       
    }
    // Vector3 OGplayerPos;
    // Vector3 OGAIPos;
    Image mainImg;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if(gameEND)
        {
            Decide();
        }
    }

}
