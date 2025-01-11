using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class VSAnimation : MonoBehaviour
{
    public GameObject playerVS;
    public GameObject AIVS;
    public ParticleSystem[] PlayerParticles;
    public ParticleSystem[] AIParticles;
    public Sprite[] playerImgs = new Sprite[3];
    public Sprite[] AIImgs = new Sprite[3];

    Vector3 OGplayerPos;
    Vector3 OGAIPos;
    Image playerImg;
    Image aIImg;
    // Start is called before the first frame update
    void Start()
    {
        playerImg = playerVS.GetComponent<Image>();
        aIImg = AIVS.GetComponent<Image>();
        playerImg.sprite = playerImgs[PlayerPrefs.GetInt("PlayerCharacter")];
        aIImg.sprite = AIImgs[PlayerPrefs.GetInt("ComputerCharacter")];
        //원래 위에 이건데, 편의상 2로 할게요.
        // aIImg.sprite = AIImgs[2];
        
        OGplayerPos = playerVS.transform.localPosition;
        OGAIPos = AIVS.transform.localPosition;
        playerVS.transform.localPosition = new Vector3(-1600,100,0);
        AIVS.transform.localPosition = new Vector3(1600,100,0);
        StartCoroutine(VS());
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator VS()
    {
        yield return new WaitForSeconds(0.4f);
        playerVS.transform.DOLocalMove(OGplayerPos, 0.2f);
        AIVS.transform.DOLocalMove(OGAIPos, 0.2f);
        PlayerParticles[PlayerPrefs.GetInt("PlayerCharacter")].Play();
        AIParticles[PlayerPrefs.GetInt("ComputerCharacter")].Play();
        yield return new WaitForSeconds(2f);
        playerImg.DOFade(0,0.2f);
        aIImg.DOFade(0,0.2f);
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);
        yield return null;
        
    }
}
