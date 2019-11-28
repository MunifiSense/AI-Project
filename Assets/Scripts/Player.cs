using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int computersLeft = 6;
    public Text pcsLeft;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pcsLeft.text = computersLeft.ToString();

        if (computersLeft == 0)
        {
            GameObject.Find("ExitDoor").SetActive(true);
            GameObject.Destroy(GameObject.Find("EndDoors"));
        }
    }
}
