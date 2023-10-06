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
        if (playerinput.move != 0 || playerinput.rotate != 0)
        {
            if (interactObj != null && interactObj.isCapturing)
            {
                interactObj.PauseCapture();
            }
        }
        if (playerinput.interact)
        {
            if(findObjectInFov.hitObject != null)
                interactObj = findObjectInFov.hitObject.GetComponent<MissionData>();
            else // 상호 작용 가능한 오브젝트가 조준점에 들어오지 않았음
            {
                Debug.Log("hitObject null");
                return;
            }
            if(interactObj.isCapturing) //캡쳐 중인 오브젝트에 상호작용 재시도 -> 아무일도 하지 않음
            {
                return;
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
