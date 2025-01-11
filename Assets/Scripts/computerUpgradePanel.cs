using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class computerUpgradePanel : MonoBehaviour
{
    private bool isAlive;
    public float firstPos = 8;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            isAlive = !isAlive;
            if (!isAlive) StartCoroutine(die());
            else StartCoroutine(generate());
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    IEnumerator die()
    {
        transform.DOMoveX(firstPos, 0.1f);
        yield return new WaitForSeconds(0.1f);
        transform.DOMoveX(firstPos+4, 0.2f);
        yield return null;
    }
    IEnumerator generate()
    {
        transform.DOMoveX(firstPos, 0.1f);
        yield return new WaitForSeconds(0.1f);
        transform.DOMoveX(firstPos+0.3f, 0.2f);
        yield return null;
    }
}