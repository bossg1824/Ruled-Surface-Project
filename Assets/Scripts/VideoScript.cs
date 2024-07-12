using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoScript : MonoBehaviour
{
    public VideoPlayer player;

    [Serializable]
    public class VideoEvent 
    {
        public Transform ToShow;
        public float TimeToShowAt;
    }

    [SerializeField]
    public List<VideoEvent> events;

    public void Start()
    {
        if(player == null)
        {
            player = GetComponent<VideoPlayer>();
        }
        foreach(VideoEvent e in events)
        {
            e.ToShow.gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        if (player.isPlaying)
        {
            foreach(VideoEvent e in events)
            {
                if(player.clockTime >= e.TimeToShowAt)
                {
                    e.ToShow.gameObject.SetActive(true);
                }
            }
        }   
    }
}
