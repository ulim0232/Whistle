using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Outline outline;
    public Transform player;
    private float outlineDis = 3f;

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
