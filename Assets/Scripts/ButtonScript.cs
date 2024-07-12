using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static ButtonFunctions;

public class ButtonScript : MonoBehaviour
{
    private Vector3 pos;
    private Vector3 down;
    [Serializable]
    public class funcTargetCell 
    {
        public Transform target;
        public buttonFuncName function;
    }

    [SerializeField]
    public List<funcTargetCell> functions;

    void Start()
    {
        pos = transform.position;
        down = new Vector3(0, -.01f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hands"))
        {
            transform.position = pos + down;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Hands"))
        {
            transform.position = pos;


            foreach (funcTargetCell func in functions)
            {
                GetButtonFunction(func.function)(func.target);
            }
        }


    }
}
