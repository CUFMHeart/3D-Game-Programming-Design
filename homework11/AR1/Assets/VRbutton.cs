using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VRbutton : MonoBehaviour, IVirtualButtonEventHandler
{
    public GameObject plane;
    public GameObject VRbutton_up;
    public GameObject VRbutton_down;
    public VirtualButtonBehaviour[] VRbehaviours;

    void Start()
    {
        Debug.Log("0000000000");
        VRbehaviours = GetComponentsInChildren<VirtualButtonBehaviour>();
        for (int i = 0; i < VRbehaviours.Length; i++)
        {
            VRbehaviours[i].RegisterEventHandler(this);
        }
        plane = GameObject.Find("airplane");
        VRbutton_up = GameObject.Find("Up");
        VRbutton_down = GameObject.Find("Down");
    }

    void Update()
    {

    }

    public void OnButtonPressed(VirtualButtonBehaviour myButton)
    {        
        switch (myButton.VirtualButtonName)
        {
            case "Up":
                plane.transform.position += new Vector3(0, 0.03f, 0);
                break;
            case "Down":
                plane.transform.position -= new Vector3(0, 0.03f, 0);
                break;
        }
    }

    
    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {

    }
}