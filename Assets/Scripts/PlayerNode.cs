using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNode : MonoBehaviour
{
    public int roomLoc = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Room")
        {
            roomLoc = other.gameObject.GetComponent<LaserDoor>().room;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Room")
        {
            roomLoc = -1;
        }
    }
}
