using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public MissionData interactObj;
    private PlayerInput playerinput;
    private FindObjectInFov findObjectInFov;

    private void Start()
    {
        playerinput = GetComponent<PlayerInput>();
        findObjectInFov = GetComponent<FindObjectInFov>();
    }


    private void Update()
    {
        if(playerinput.interact)
        {
            if(findObjectInFov.hitObject != null)
                interactObj = findObjectInFov.hitObject.GetComponent<MissionData>();
            else
            {
                Debug.Log("hitObject null");
            }
            if (interactObj != null)
            {
                if(interactObj.isPaused)
                {
                    interactObj.ResumeCapture();
                }
                else
                {
                    interactObj.StartCapture();
                }
            }
            else
            {
                Debug.Log("don't exist obj");
            }
        }

        if(interactObj != null)
        {
            if (interactObj.gauge == 100)
            {
                interactObj = null;
            }
        }
       
    }
}
