﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardTrigger : MonoBehaviour
{
    GameState gameState;
    // Start is called before the first frame update
    void Start()
    {
        gameState = GameObject.Find("GameState").GetComponent<GameState>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name.Contains("Button"))
        {
            gameState.StartAlarm();
        }

        if(other.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("Player").gameObject.transform.position = 
                GameObject.Find("PlayerSpawn").gameObject.transform.position;
            GameObject.FindGameObjectWithTag("Player").gameObject.transform.rotation =
                GameObject.Find("PlayerSpawn").gameObject.transform.rotation;
        }
    }
}
