using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    public enum State
    {
        Idle, //�ƹ��͵� ���� �ʴ� ����
        Walk, //�̵� ��
        Doubt, //�ǽ�
        Trace //����
    }

    public float gauge = 0; //���� ������
    public State state { get; private set; }
    public LivingEntity target; //������ �÷��̾�
    //private bool isStunned; //���� �����ΰ�
    NavMeshAgent pathFinder; //���� ��Ʈ�� ���
    Vector3 startPos; //���ư� ��ġ

    public FieldOfView fieldOfView;

    private float maxGauge = 100f;
    //private float duration = 6.67f;
    private float rate = 15f;

    private bool hasTarget
    {
        get
        {
            return target != null; //!target.daed ���� �ʾ��� ��� ���� �߰�
        }
    }

    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        state = State.Idle;
    }

    private void Update()
    {
        if(dead) return; //������ ������ƮX

        FindTarget();
        Debug.Log(state);
        switch (state)
        {
            case State.Idle:
                IdleUpdate();
                break; 
            case State.Walk:
                WalkUpdate();
                break; 
            case State.Doubt:
                DoubtUpdate();
                break; 
            case State.Trace:
                TraceUpdate();
                break;
        }
        
    }

    private void IdleUpdate()
    {
        pathFinder.isStopped = true;
        if (hasTarget) //�þ߾ȿ� �÷��̾ ���ö�
        {
            state = State.Doubt;
        }
    }
    private void WalkUpdate()
    {

    }

    private void DoubtUpdate()
    {
        if(!hasTarget)
        {
            state = State.Idle;
            return;
        }
        MoveToPlayer();
    }

    private void TraceUpdate()
    {
        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        pathFinder.isStopped = false;
        pathFinder.SetDestination(target.transform.position);
        GaugeUp();

        //if (gauge <= maxGauge)
        //{
        //    GaugeUp();
        //}
        //if (gauge >= maxGauge)
        //{
        //    state = State.Trace;
        //}
    }

    //private void MovetoStartPos()
    //{
    //    pathFinder.SetDestination(startPos);
    //}


    private void FindTarget()
    {
        if (fieldOfView.player != null)
        {
            target = fieldOfView.player.GetComponent<LivingEntity>();
        }
        else
        {
            target = null;
        }
    }

    private void GaugeUp()
    {
        if (gauge <= maxGauge)
        {
            gauge += rate * Time.deltaTime;
        }
        if (gauge >= maxGauge)
        {
            state = State.Trace;
        }

        //Debug.Log(gauge);
    }
}
