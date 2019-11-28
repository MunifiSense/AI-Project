using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public GameObject winText;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            winText.SetActive(true);
        }
    }
}
