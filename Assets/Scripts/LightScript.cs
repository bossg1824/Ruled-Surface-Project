using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;

public class LightScript : MonoBehaviour
{
    public float TimeToMaxIntensity;
    public float MaxIntensity;
    private Light controlling;
    private float timer;
    private bool increasingIntensity;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        increasingIntensity = true;
        controlling = transform.GetComponent<Light>();
        controlling.intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!increasingIntensity)
        {
            return;
        }
        float nextIntensity = timer/ TimeToMaxIntensity;

        if(nextIntensity >= 1)
        {
            controlling.intensity = MaxIntensity;
            increasingIntensity = false;
        }

        else
        {
            controlling.intensity = nextIntensity * MaxIntensity;
            timer += Time.deltaTime;
        }
    }
}
