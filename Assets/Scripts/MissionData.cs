using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionData : MonoBehaviour
{
    public float gauge { get; private set; } //수집 진행도
    public float duration = 40f;

    private float startTime;
    public bool isCapturing; //수집 중인지 확인. 이것이 활성화되면 진행도가 점점 오름
    public bool isCaptured; //수집 완료 되었는지 확인
    public Outline outline;
    public bool isPaused;
    private float pauseTime;
    public Transform player;
    private float outlineDis = 3f;
    public bool hasCompletedCapture = false;

    GameManager.MissionType type { get; } = GameManager.MissionType.Data;

    private void Start()
    {
        isCapturing = false;
        isCaptured = false;
        gauge = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        hasCompletedCapture = false;
    }

    public void Update()
    {
        if (!isCaptured)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            //Debug.Log(distanceToPlayer);
            if (distanceToPlayer > outlineDis)
            {
                outline.enabled = false;
            }
            else
            {
                outline.enabled = true;
            }
        }
        if (!isCapturing)
            return;
        Capturing();
    }
    public void Capturing()
    {
        float elapsed = Time.time - startTime; //경과 시간
        if (elapsed >= duration)
        {
            if (!hasCompletedCapture)
                CompleteCapture();
        }
        else
        {
            gauge = Mathf.Lerp(0f, 100f, elapsed / duration);
            UIManager.instance.SetDataProgress(gauge);
        }
        //Debug.Log("Current Gauge Value: " + gauge);
    }
    public void StartCapture() //활성화
    {
        startTime = Time.time;
        isCapturing = true;
        UIManager.instance.SetActivePorgressUI(true);
    }

    public void CompleteCapture()
    {
        gauge = 100;
        isCapturing = false;
        isCaptured = true;
        if (outline != null)
        {
            outline.enabled = false;
        }
        UIManager.instance.SetActivePorgressUI(false);
        GameManager.instance.AddScore(1);
        GameManager.instance.UpdateMissionList(type);
        hasCompletedCapture = true;
        player.GetComponent<PlayerInteract>().PlayCompleteSound();
    }

    public void PauseCapture()
    {
        Debug.Log("pause");
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
            startTime += Time.time - pauseTime; // 일시정지된 시간을 더해 다시 시작
            UIManager.instance.SetActivePorgressUI(true);
        }
    }
}
