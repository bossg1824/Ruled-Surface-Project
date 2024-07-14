using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;

public class TimeScripts : MonoBehaviour
{
    public bool releasing;
    public List<TimeEvent> events;
    public float ExtraTimeForAlll;
    private float timer = 0;
    void Start()
    {
        foreach(TimeEvent e in events)
        {
            e.ToShow.gameObject.SetActive(false);
            e.TimeToShowAt += ExtraTimeForAlll;
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
        foreach(TimeEvent e in events)
        {
            if(timer >= e.TimeToShowAt)
            {
                e.ToShow.gameObject.SetActive(true);
                events.Remove(e);
            }
        }
        timer += Time.deltaTime;
    }
}
