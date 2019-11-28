using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HackPC : MonoBehaviour
{
    public float timeLeftInHack = 10.0f;
    public Text title;
    public Text timeLeft;
    public Text hackHint;
    private bool playerInArea;
    public void Start()
    {
        playerInArea = false;
        title.text = "";
        timeLeft.text =  "";
    }

    public void Update()
    {
        if(timeLeftInHack > 0)
        {
            if (playerInArea && Input.GetKey(KeyCode.E))
            {
                timeLeftInHack -= Time.deltaTime;
                title.text = "Hacking";
                timeLeft.text = Mathf.RoundToInt(timeLeftInHack).ToString() + " seconds left";
            }
        }
        else
        {
            // Handle finish hack
            title.text = "Hack complete!";
            timeLeft.text = "";
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().computersLeft--;
            GetComponent<HackPC>().enabled = false;
            hackHint.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerInArea = true;
            hackHint.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInArea = false;
            hackHint.enabled = false;
        }
    }
}
