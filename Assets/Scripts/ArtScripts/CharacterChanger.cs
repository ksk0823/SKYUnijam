using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using UnityEngine.SceneManagement;

public class CharacterChanger : MonoBehaviour
{
    int chrNum = 0;
    // 0 = red, 1= blue, 2 = emerald
    public bool isClient = false;
    public Texture2D[] tD;
    public GameObject chrSelector;
    private Material material;
    private Image img;
    public string[] names;
    public string[] descriptions;
    // Start is called before the first frame update
    void Start()
    {
        img = gameObject.GetComponent<Image>();
        material = Instantiate(img.material);
        img.material = material;
        if(!isClient)
        {
            img.material.SetTexture("_Light_Texture",tD[0]);
        }
        else
        {
            img.material.SetTexture("_Light_Texture",tD[3]);
        }

    }

    public void imagechange(int chrnum)
    {
        // img.sprite = spr[chrnum];
        img.material.SetTexture("_Light_Texture",tD[chrnum]);

    }
    public void changeSelectorPosition(int chrnum)
    {
        chrSelector.transform.DOLocalMoveX(-172 + 172 * chrnum,0.2f);
    }
    public void getReady()
    {
        StartCoroutine(selectAnim());
    }

    IEnumerator selectAnim()
    {
        // transform.DOLocalMoveY(20f,0.2f);
        transform.DOScale(0.9f,0.2f);
        transform.DOShakeRotation(0.2f,15,3);
        yield return new WaitForSeconds(0.2f);
        transform.DOScale(1.15f,0.2f);

        // transform.DOLocalMoveY(-80f,0.2f);

    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
             SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
