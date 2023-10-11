using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    public enum State
    {
        Idle, //아무것도 하지 않는 상태
        Walk, //이동 중
        Doubt, //의심
        Trace //추적
    }

    public float gauge = 0; //위험 게이지
    public State state { get; private set; }
    public LivingEntity target; //추적할 플레이어
    //private bool isStunned; //기절 상태인가
    NavMeshAgent pathFinder; //추적 루트에 사용
    Vector3 startPos; //돌아갈 위치

    public FieldOfView fieldOfView;

    private float maxGauge = 100f;
    //private float duration = 6.67f;
    private float rate = 15f;

    private bool hasTarget
    {
        get
        {
            return target != null; //!target.daed 죽지 않았을 경우 추후 추가
        }
    }

    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        state = State.Idle;
    }

    private void Update()
    {
        if(dead) return; //죽으면 업데이트X

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
        if (hasTarget) //시야안에 플레이어가 들어올때
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
