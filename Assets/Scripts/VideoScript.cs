using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using static TimeFunctions;
public class VideoScript : MonoBehaviour
{
    public VideoPlayer player;



    [SerializeField]
    public List<TimeEvent> events;

    public void Start()
    {
        if (player == null)
        {
            player = GetComponent<VideoPlayer>();
        }
        foreach (TimeEvent e in events)
        {
            e.ToShow.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!player.isPlaying || events.Count == 0)
        {
            return;
        }
        for (int i = 0; i < events.Count; i++)
        {
            TimeEvent e = events[i];
            if (player.time >= e.TimeToShowAt)
            {
                Debug.Log("Time: " + player.time);
                Debug.Log("Object: " + e.ToShow);
                Debug.Log("GameObject: " + e.ToShow.gameObject);
                GetTimeFunction(e.ToDo)(e.ToShow);
                events.Remove(e);
                i--;
            }
        }
    }
}