using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionData : MonoBehaviour
{
    public float gauge { get; private set; } //���� ���൵
    public float duration = 60f;
    private float startTime;
    public bool isCapturing; //���� ������ Ȯ��. �̰��� Ȱ��ȭ�Ǹ� ���൵�� ���� ����
    public Outline outline;
    public bool isPaused;
    private float pauseTime;

    private void Start()
    {
        isCapturing = false;
        gauge = 0;
    }

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            PauseCapture();
        }
        if (!isCapturing)
            return;
        Capturing();

    }
    public void Capturing()
    {
        float elapsed = Time.time - startTime; //��� �ð�
        if (elapsed >= duration)
        {
            CompleteCapture();
        }
        else
        {
            gauge = Mathf.Lerp(0f, 100f, elapsed / duration);
            UIManager.instance.SetDataProgress(gauge);
        }
        Debug.Log("Current Gauge Value: " + gauge);
    }
    public void StartCapture() //Ȱ��ȭ
    {
        startTime = Time.time;
        isCapturing = true;
        UIManager.instance.SetActivePorgressUI(true);
    }

    public void CompleteCapture()
    {
        gauge = 100;
        isCapturing = false;
        if(outline != null)
        {
            outline.enabled = false;
        }
        UIManager.instance.SetActivePorgressUI(false);
    }

    public void PauseCapture()
    {
        if (isCapturing && !isPaused)
        {
            isPaused = true;
            isCapturing = false;
            pauseTime = Time.time;
            UIManager.instance.SetActivePorgressUI(false);
        }
    }

    public void ResumeCapture()
    {
        if (!isCapturing && isPaused)
        {
            isPaused = false;
            isCapturing = true;
            startTime += Time.time - pauseTime; // �Ͻ������� �ð��� ���� �ٽ� ����
            UIManager.instance.SetActivePorgressUI(true);
        }
    }
}
