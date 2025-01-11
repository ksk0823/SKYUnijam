using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circleGage : MonoBehaviour
{
    public GameObject minicircle;
    
    [SerializeField, Range(1f,30)]
    public float time = 3;
    public float radius = 30;
    float val = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        startgaging();
    }
    void startgaging()
    {
        float growthRate = (2*Mathf.PI)/time;
        val += growthRate * Time.deltaTime;
        // float xPos = this.gameObject.transform.position.x;
        // float yPos = this.gameObject.transform.position.y;
        minicircle.transform.localPosition= radius* new Vector3(Mathf.Sin(val),Mathf.Cos(val),0);
    }
}
