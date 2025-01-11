using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class startSceneHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject selector;
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.2f,0.2f);
        selector.SetActive(true);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f,0.2f);
        selector.SetActive(false);

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
