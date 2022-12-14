using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Balls : MonoBehaviour
{
    public GameObject ballsNextPrefab;
    public int id;
    Rigidbody2D ridge;
    public Action<Balls,Balls> OnLevilUp;
    public Action OnGameOver;
    public Action OnGameWin;
    bool isTouchRedline;
    float timer;
    public int score;
    // Start is called before the first frame update
    void Start()
    {
        ridge = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isTouchRedline==false)
        {
            return;
        }
        timer+=Time.deltaTime;
        if(timer>=3)
        {
            OnGameOver?.Invoke();
        }
    }

    public void SetSimulated(bool bridge)
    {
        if(ridge == null)
        {
            ridge = GetComponent<Rigidbody2D>();
        }
        ridge.simulated = bridge;
    }
    void OnCollisionEnter2D(Collision2D other) 
    {
        var obj = other.gameObject;
        var fruit = obj.GetComponent<Balls>();
        if(obj.tag=="Ball")
        {
            if(obj.name ==this.gameObject.name)
            {
                if(ballsNextPrefab!=null)
                {
                    OnLevilUp?.Invoke(this, fruit);  
                    if(ballsNextPrefab.name=="Ball_10")
                    {
                        OnGameWin?.Invoke();
                    }
       
                }
            }           
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        var obj = other.gameObject;
        if(obj.tag=="RedLine")
        {
            isTouchRedline = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        var obj = other.gameObject;
        if(obj.tag=="RedLine")
        {
            isTouchRedline = false;
            timer = 0;
        }
    }

}
