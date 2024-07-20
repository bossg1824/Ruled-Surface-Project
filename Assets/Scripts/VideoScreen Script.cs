using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoScreenScript : MonoBehaviour
{
    private VideoPlayer player;
    private Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        player = GetComponent<VideoPlayer>();
        renderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.time >= player.length - .1)
        {
            player.time = 0;
            player.Stop();
            renderer.enabled = false;
        }
    }
}
