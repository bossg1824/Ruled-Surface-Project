using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePlayScript : MonoBehaviour
{
    public enum state 
    {
        play,
        pause
    }

    public state PauseOrPlay;
    private state PreviousState;
    public Transform[] ShowOnPlayOnly;
    public Transform[] ShowOnPauseOnly;
    void Start()
    {
        StateSwitch(PauseOrPlay == state.play);
        PreviousState = PauseOrPlay;
    }

    // Update is called once per frame
    void Update()
    {
        if(PreviousState == PauseOrPlay)
        {
            return;
        }
        Debug.Log("Switching States");
        StateSwitch(PauseOrPlay == state.play);
        PreviousState= PauseOrPlay;
    }
    private void StateSwitch(bool Play)
    {
        foreach (Transform t in ShowOnPlayOnly)
        {
            if (t != null)
            {
                t.gameObject.SetActive(Play);
            }
        }
        foreach (Transform t in ShowOnPauseOnly)
        {
            if (t != null)
            {
                t.gameObject.SetActive(!Play);
            }
        }
    }
}
