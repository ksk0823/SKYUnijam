using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardManager : MonoBehaviour
{
    public CardAnim[] cards; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void dissolveCards()
    {
        for (int i = 0; i <= 2; i ++) {
        cards[i].textDissapear();
        if(!cards[i].clicked)
        {
            cards[i].dissolve();
        }
        }
    }

}
