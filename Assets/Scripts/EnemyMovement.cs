using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyMovement : MonoBehaviour
{
    public FieldOfView fieldOfView;
    public float moveSpeed = 10;
    public float rotationSpeed = 20;
    private Rigidbody rb;
    private bool isChasing = false;
    public Vector3 startPos;
    public bool isAlive = true;
    public float arrivalDistance = 0.1f;

    public Transform target;
    public NavMeshAgent agent;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Alpha0))
        //{
        //    agent.SetDestination(target.position);
        //}
        if (fieldOfView.player != null)
        {
            if (!isChasing)
            {
                isChasing = true;
                startPos = transform.position;
            }
            Vector3 directionToPlayer = (fieldOfView.player.transform.position - transform.position).normalized;
            Vector3 newPos = transform.position + directionToPlayer * moveSpeed * Time.deltaTime;

            rb.MovePosition(newPos);

            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime));
        }
        else
        {
            if (isChasing)
            {
                isChasing = false;
                isAlive = false;
            }
            if (!isAlive)
            {
                Vector3 direction = (startPos - transform.position).normalized;
                Vector3 newPos = transform.position + direction * moveSpeed * Time.deltaTime;

                rb.MovePosition(newPos);

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime));
            }
            if (Vector3.Distance(transform.position, startPos) < arrivalDistance)
            {
                isAlive = true;
            }
        }
    }
}
