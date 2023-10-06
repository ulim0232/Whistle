using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionData : MonoBehaviour
{
    public float gauge { get; private set; } //수집 진행도
    public float duration = 60f;
    private float startTime;
    public bool isCapturing; //수집 중인지 확인. 이것이 활성화되면 진행도가 점점 오름
    public Outline outline;

    private void Start()
    {
        isCapturing = false;
        gauge = 0;
    }

    public void Update()
    {
        if (!isCapturing)
            return;
        Capturing();
    }
    public void Capturing()
    {
        float elapsed = Time.time - startTime; //경과 시간
        if (elapsed >= duration)
        {
            CompleteCapture();
        }
        else
        {
            gauge = Mathf.Lerp(0f, 100f, elapsed / duration);
        }
        Debug.Log("Current Gauge Value: " + gauge);
    }
    public void StartCapture() //활성화
    {
        startTime = Time.time;
        isCapturing = true;
    }

    public void CompleteCapture()
    {
        gauge = 100;
        isCapturing = false;
        if(outline != null)
        {
            outline.enabled = false;
        }
    }
}
