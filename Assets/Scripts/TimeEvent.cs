using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeFunctions;

[Serializable]
public class TimeEvent
{
    public Transform ToShow;
    public float TimeToShowAt;
    public TimeFunctionName ToDo = TimeFunctionName.Activate; 

    public class TimeEventComparer : Comparer<TimeEvent> 
    {
        public override int Compare(TimeEvent x, TimeEvent y)
        {
            return x.TimeToShowAt.CompareTo(y.TimeToShowAt);
        }
    }

}
