using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDoor : MonoBehaviour
{
    public int room = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Guard closestGuard = null;
            float distance = float.MaxValue;
            GameObject[] guards = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0 ; i < guards.Length; i++)
            {
                // Add to room visits for ngram
                guards[i].gameObject.GetComponent<GuardLearning>().AddRoomVisit(room);

                // Alert nearest guard of player
                if(Vector3.Distance(transform.position, guards[i].transform.position) < distance)
                {
                    distance = Vector3.Distance(transform.position, guards[i].transform.position);
                    closestGuard = guards[i].GetComponent<Guard>();
                }
                
            }
            closestGuard.nearbyRoomTripped = room;
        }
    }
}
