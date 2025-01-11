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
