using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;
using static TimeFunctions;

public class TimeScripts : MonoBehaviour
{
    public bool releasing;
    public List<TimeEvent> events;
    public float ExtraTimeForAlll;
    public float TimeMultForAll = 1;
    private float timer = 0;
    void Start()
    {
        foreach(TimeEvent e in events)
        {
            e.ToShow.gameObject.SetActive(false);
            e.TimeToShowAt *= TimeMultForAll;
            e.TimeToShowAt += ExtraTimeForAlll;
            Debug.Log(e.ToString());
        }
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!releasing || events.Count == 0)
        {
            return;
        }
        for(int i = 0; i < events.Count; i++)
        {
            TimeEvent e = events[i];
            if(timer >= e.TimeToShowAt)
            {
                Debug.Log("Time: " + timer);
                Debug.Log("Object: " + e.ToShow);
                Debug.Log("GameObject: " + e.ToShow.gameObject);
                Debug.Log("Doing: " + e.ToDo);
                GetTimeFunction(e.ToDo)(e.ToShow);
                events.Remove(e);
                i--;
            }
        }
        timer += Time.deltaTime;
    }
}
