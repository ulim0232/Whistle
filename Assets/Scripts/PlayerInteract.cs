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
            else // ��ȣ �ۿ� ������ ������Ʈ�� �������� ������ �ʾ���
            {
                Debug.Log("hitObject null");
                return;
            }
            if(interactObj.isCapturing) //ĸ�� ���� ������Ʈ�� ��ȣ�ۿ� ��õ� -> �ƹ��ϵ� ���� ����
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
