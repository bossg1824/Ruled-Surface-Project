using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class ButtonFunctions
{
    public delegate void buttonFunction(Transform target);
    public enum buttonFuncName {ToIntro, ToDevelopable, ToQuadrics, ToCubics, ToQuartics, Check, FastForward, Rewind, StartStop, ChangeButtonStartOrStop};
    public static buttonFunction[] buttonFunctions = {ToIntro, ToDevelopable, ToQuadrics, ToCubics, ToQuartics, Check, FastForward, Rewind, StartStop, ChangeButtonStartStop};

    public static buttonFunction GetButtonFunction(int index)
    {
        return buttonFunctions[index];
    } 

    public static buttonFunction GetButtonFunction(buttonFuncName buttonFunctionName)
    {
        return buttonFunctions[(int)buttonFunctionName];
    }

    public static void ToIntro(Transform target)
    {
        SceneManager.LoadScene("Intro");
    }

    public static void ToDevelopable(Transform target)
    {
        SceneManager.LoadScene("Developable");
    }
    public static void ToQuadrics(Transform target)
    {
        SceneManager.LoadScene("Quadrics");
    }
    public static void ToCubics(Transform target)
    {
        SceneManager.LoadScene("Cubics");
    }
    public static void ToQuartics(Transform target)
    {
        SceneManager.LoadScene("Quartics");
    }

    public static void Check(Transform target)
    {
        bool success = true;
        TargetsToCheck targets = target.GetComponent<TargetsToCheck>();
        foreach(PedistalScript p in targets.targets)
        {
            if(p != null)
            {
                if (!p.OnCheck())
                {
                    success = false;
                    break;
                }
            }
        }

        foreach(Light l in targets.lightsToIndicate)
        {
            if(l != null)
            {
                l.color = success ? Color.red : Color.green;
            }
        }
    }

    public static void FastForward(Transform target)
    {
        VideoPlayer videoPlayer = target.GetComponent<VideoPlayer>();
        if (videoPlayer == null)
        {
            return;
        }
        if(videoPlayer.time == 0)
        {
            return;
        }
        if(videoPlayer.length < videoPlayer.time + 10)
        {
            videoPlayer.time = videoPlayer.length;
        } else
        {
            videoPlayer.time += 10;
        }
    }

    public static void Rewind(Transform target)
    {
        VideoPlayer videoPlayer = target.GetComponent<VideoPlayer>();
        if(videoPlayer == null)
        {
            return;
        }
        if (videoPlayer.time == 0)
        {
            return;
        }
        if(videoPlayer.time - 10 < 0)
        {
            videoPlayer.time = 0;
        } else
        {
            videoPlayer.time -= 10;
        }

    }

    public static void StartStop(Transform target)
    {
        VideoPlayer video= target.GetComponent<VideoPlayer>();
        Renderer renderer = target.GetComponent<Renderer>();
        if (video.isPlaying)
        {
            video.Pause();
            renderer.enabled = false;
        }
        else
        {
            video.Play();
            renderer.enabled = true;
        }
    }
    public static void ChangeButtonStartStop(Transform target)
    {
        PausePlayScript button = target.GetComponent<PausePlayScript>();
        if(button != null)
        {
            button.PauseOrPlay = button.PauseOrPlay == PausePlayScript.state.play ? PausePlayScript.state.pause : PausePlayScript.state.play;
        }
    }
}
