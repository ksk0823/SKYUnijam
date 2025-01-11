using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class chrSelectBtns : MonoBehaviour, IPointerClickHandler
{
    public int chrNum;
    public CharacterChanger cC;

    public void OnPointerClick(PointerEventData eventData)
    {
        cC.imagechange(chrNum);
        cC.changeSelectorPosition(chrNum);
        FinalTitleManager.instance.PlayerDescriptionText.text = cC.descriptions[chrNum];
        FinalTitleManager.instance.PlayerCharacterText.text = cC.names[chrNum];
        FinalTitleManager.instance.SetPlayerCharacter(chrNum);
        PlayerPrefs.SetInt("PlayerCharacter", chrNum);
        PlayerPrefs.Save();
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
