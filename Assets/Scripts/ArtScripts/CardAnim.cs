using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CardAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Material material;
    private Image image;
    public bool clicked = false;
    public TMP_Text[] tmp;

    public void OnPointerClick(PointerEventData eventData)
    {
        // StartCoroutine(click());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.1f,0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f,0.2f);
    }

    void Start()
    {
        image = gameObject.GetComponent<Image>();
        material = Instantiate(image.material);
        image.material = material;
        Init();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }
    public IEnumerator click()
    {
        clicked = true;
        transform.DOScale(0.9f,0.1f);
        yield return new WaitForSeconds(0.1f);
        transform.DOScale(1f,0.1f);
        yield return new WaitForSeconds(0.1f);
        transform.DOLocalMoveY(-500,0.2f);
        material.DOFloat(0f,"_Alive", 0.2f);

        yield return null;
    }

    public void Init()
    {
        material.SetFloat("_Alive", 1f);
        material.SetFloat("_cutoffHeight", 0f);
    }
  
    public void cardClick()
    {
        StartCoroutine(click());
        
    }
    public void dissolve()
    {
        StartCoroutine(dsv());
    }
    IEnumerator dsv()
    {
        yield return new WaitForSeconds(0.3f);
        material.DOFloat(-7,"_cutoffHeight",0.5f);

    }
    public void textDissapear()
    {
        StartCoroutine(tD());
    }
    IEnumerator tD()
    {
        yield return new WaitForSeconds(0.2f);
      tmp[0].DOColor(new Vector4(0,0,0,0), 0.2f);
      tmp[1].DOColor(new Vector4(0,0,0,0), 0.2f);
    }
}
