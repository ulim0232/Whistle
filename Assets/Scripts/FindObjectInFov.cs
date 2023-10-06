using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FindObjectInFov : MonoBehaviour
{
    private Camera mainCamera;
    public float rayLength = 100f; // ������ ���� (���� ����)  
    public LayerMask targetLayer;
    public GameObject hitObject { get; private set; }
    public UIManager manager;

    void Start()
    {
        mainCamera = Camera.main; // ���� ī�޶� ������
    }

    void Update()
    {
        FindObject();
    }

    public void FindObject()
    {
        if (mainCamera != null)
        {
            hitObject = null;
            Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 50, targetLayer))
            {
                hitObject = hit.transform.gameObject;
                if(!hitObject.GetComponent<MissionData>().isCapturing) 
                {
                    UIManager.instance.SetActiveInteractUI(true);
                }
                else
                {
                    UIManager.instance.SetActiveInteractUI(false);
                }
                //return hit.transform.gameObject;
                //Debug.Log(hitObject.name);
            }
            else
            {
                UIManager.instance.SetActiveInteractUI(false);
            }
        }
    }
}
