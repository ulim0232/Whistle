using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    private CinemachineFreeLook cineCam;

    private float yAxisSpeed = 0.07f;
    private float xAxisSpeed = 5.0f;

    private void Start()
    {
        cineCam = GetComponent<CinemachineFreeLook>();
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftAlt))
        {
            cineCam.m_XAxis.m_MaxSpeed = 0;
            cineCam.m_YAxis.m_MaxSpeed = 0;
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            cineCam.m_XAxis.m_MaxSpeed = xAxisSpeed;
            cineCam.m_YAxis.m_MaxSpeed = yAxisSpeed;
        }
    }
}
