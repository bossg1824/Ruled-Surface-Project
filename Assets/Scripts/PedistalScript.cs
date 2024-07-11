using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.ShaderKeywordFilter;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PedistalScript : MonoBehaviour
{
    /*private string selectedTag = " ";

    [CustomEditor(typeof(DevelopableSurfacePlaneLocker))]
    public class DevelopableSurfacePlaneLockerEditor : Editor
    {
        string selectedTag = " ";
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            selectedTag = EditorGUI.TagField(new Rect(3,3,3,3), "New Tag", selectedTag);

        }
    }
    */

    [SerializeField]
    public string[] TagsToGrab;

    [SerializeField]
    public string[] TagsToAccept;

    [SerializeField]
    public Vector3 SuspendPosition;

    [SerializeField]
    public bool Rotate;

    [SerializeField]
    public Vector3 RotateOffset;

    [SerializeField]
    public int LevelsOfParentageAllowed;

    [SerializeField]
    public Transform startWith;

    private Transform storedObject = null;

    private Rigidbody storedRigid = null;

    private XRGrabInteractable storedGrab = null;

    private bool ObjectKinematic;

    private List<Collider> triggers = new();

    private float timer = 0;

    private float timeToRefresh = 5;

    // Start is called before the first frame update
    void Start()
    {
        storedObject = startWith;
        SetupStore();

        foreach(Collider c in transform.GetComponents<Collider>())
        {
            if (c.isTrigger)
            {
                triggers.Add(c);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (storedObject != null)
        {
            storedObject.position = transform.position + SuspendPosition;
            //if we are supposed to rotate
            if (Rotate)
            {
                //rotate by the given amount (rotate offset) 
                storedObject.rotation = Quaternion.Euler(storedObject.rotation.eulerAngles + (RotateOffset * Time.deltaTime));
            }
        } else
        {
            if (timer < timeToRefresh)
            {
                timer += Time.deltaTime;
            } else
            {
                foreach(Collider c in triggers)
                {
                    c.enabled = true;
                    timer = 0;
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision before check");
        //checking if we are already storing an object or if the object that activated this does not have a tag we should Grab
        if (storedObject != null)
        {
            return;
        }

        Debug.Log("Collision after check");
        //fining what we should grab going through parentage as indicated
        Transform tracking = other.transform;
        for(int i = 0; i <= LevelsOfParentageAllowed; i++)
        {
            if (TagsToGrab.Contains(tracking.tag))
            {
                storedObject = tracking;
                break;
            }
            if(tracking.parent == null)
            {
                break;
            }
            tracking = tracking.parent;
        }
        SetupStore();
    }

    private void SetupStore()
    {
        if (storedObject == null)
        {
            return;
        }
        storedObject.position = transform.position + SuspendPosition;
        //getting the rigidbody of the other object
        storedRigid = storedObject.GetComponent<Rigidbody>();
        //if one exists
        if (storedRigid != null)
        {
            //store whether or not the object was Kinematic to begin with
            ObjectKinematic = storedRigid.isKinematic;
            //make the rigid body kinematic
            storedRigid.isKinematic = true;
        }
        //getting the XRGrab component of the other object
        storedGrab = storedObject.GetComponent<XRGrabInteractable>();
        //if a grab component exists
        if (storedGrab != null)
        {
            //Add a listener to the selectEntered so that we release this object once it is grabbed again
            storedGrab.selectEntered.AddListener(ReleaseProxy);
        }
        foreach (Collider c in triggers)
        {
            c.enabled = false;
        }
    }
    public bool OnCheck()
    {
        //if there is no stored object
        if(storedObject == null)
        {
            return false;
        }
        //whether the object stored has a tag we should accept
        return TagsToAccept.Contains(storedObject.transform.tag);
    }


    public void ReleaseProxy(UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs NotUsed)
    {
        Debug.Log("Release Proxy");
        //Calls release without the extra arguments needed to be a listener
        Release();
    }
    public void Release()
    {
        //if no object is currently being stored by the pedistal
        if( storedObject == null)
        {
            return;
        }
        //if the object is XR Grabbable
        if(storedGrab != null)
        {
            //Remove the listener that calls this function everytime the object is selected
            storedGrab.selectEntered.RemoveListener(ReleaseProxy);

            //No longer store the XR Grabbable
            storedGrab = null;
        }
        //if the object has a rigid body
        if (storedRigid != null)
        {
            //Set the Kinemacity back to what it originally was
            storedRigid.isKinematic = ObjectKinematic;

            //No longer store the rigid body
            storedRigid = null;
        }


        //No longer store the Object
        storedObject = null;

        timer = 0;
    }
}
