using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class VideoScript : MonoBehaviour
{
    public VideoPlayer player;

    

    [SerializeField]
    public List<TimeEvent> events;

    public void Start()
    {
        if(player == null)
        {
            player = GetComponent<VideoPlayer>();
        }
        foreach(TimeEvent e in events)
        {
            e.ToShow.gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        if (player.isPlaying)
        {
            foreach(TimeEvent e in events)
            {
                if(player.clockTime >= e.TimeToShowAt)
                {
                    e.ToShow.gameObject.SetActive(true);
                }
            }
        }   
    }
}
