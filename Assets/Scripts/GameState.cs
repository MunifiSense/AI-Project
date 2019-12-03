using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private bool alarmTriggered;
    // Start is called before the first frame update
    void Start()
    {
        alarmTriggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool Alarm()
    {
        return alarmTriggered;
    }
    public void StopAlarm()
    {
        alarmTriggered = false;
    }

    public void StartAlarm()
    {
        alarmTriggered = true;
    }
}
