using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class computerUpgradePanel : MonoBehaviour
{
    private bool isAlive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            isAlive = !isAlive;
            if(isAlive)StartCoroutine(die());
            else StartCoroutine(generate());
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void pannelonoff()
    {
        
    }

    IEnumerator die()
    {
        transform.DOMoveX(9f,0.1f);
        yield return new WaitForSeconds(0.1f);
        transform.DOMoveX(13f,0.2f);
        yield return null;
    }
    IEnumerator generate()
    {
        transform.DOMoveX(9f,0.1f);
        yield return new WaitForSeconds(0.1f);
        transform.DOMoveX(9.3f,0.2f);
        yield return null;
    }
}
