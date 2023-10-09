using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionBook : MonoBehaviour
{
    public Outline outline;
    public Transform player;
    public float outlineDis = 10f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void Update()
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
}
