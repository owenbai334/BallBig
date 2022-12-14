using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLine : MonoBehaviour
{
    public GameObject redline;
    void OnTriggerStay2D(Collider2D other) 
    {
        redline.SetActive(true);
    }
    void OnTriggerExit2D(Collider2D other) 
    {  
        redline.SetActive(false);
    }
}
