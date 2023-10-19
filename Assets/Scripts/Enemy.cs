using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy : LivingEntity
{
    public enum State
    {
        Idle, //아무것도 하지 않는 상태
        Walk, //이동 중
        Doubt, //의심
        Trace //추적
    }

    //상태
    public State state { get; private set; }

    //속도
    public float runSpeed = 4;
    public float walkSpeed = 2;

    //시간
    private float waitTime = 3f;

    private float targetTimer = 0f;
    private float targetTime = 5f;

    private float resetTime = 60f; //1분 후 게이지 초기화
    private float resetTimer = 0f;

    private float lastAttackTime = 0;
    private float timeBetAttack = 2f;

    private float wakeUptime = 30f;

    //게이지
    public float gauge = 0; //위험 게이지
    private float maxGauge = 100f;
    public Michsky.MUIP.ProgressBar gaugeBar;

    public float rate = 15f; //초당 오르는 게이지 값
    private Transform player;

    //위치
    public Vector3 nextPos; //다음 이동 위치
    //List<Vector3> WayPoints = new List<Vector3>();
    private Vector3[] WayPoints;
    public Transform[] WayPointTransforms;
    public int wayPointCount = 0;
    public int nextWayPointIndex = 0;

    //오브젝트
    public LivingEntity target; //추적할 플레이어
    NavMeshAgent pathFinder; //추적 루트에 사용
    public FieldOfView fieldOfView;
    public LayerMask targetLayer;

    //bool
    private bool isWaiting = false;
    private bool hasTargetInFOV = false;
    private bool isWatingReset = false;
    private bool isReversing = false;
    private bool isWakeUp = false;

    //소리감지
    public float soundRadius = 5f;
    public float nearRadius = 3f; //근접 3유닛은 원통형으로 검사함

    public float damage = 10f;

    public string playerTag = "Player";

    //애니메이션
    public Animator enemyAnimator;
    private bool hasTarget
    {
        get
        {
            return target != null && !target.dead;
        }
    }

    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        state = State.Idle;
        nextPos = Vector3.zero;
        
    }

    private void Start()
    {
        wayPointCount = WayPointTransforms.Length;
        WayPoints = new Vector3[wayPointCount];
        for (int i = 0; i < wayPointCount; i++)
        {
            WayPoints[i] = WayPointTransforms[i].position;
        }
        nextPos = WayPoints[nextWayPointIndex];
        enemyAnimator = GetComponent<Animator>();
        player = GameObject.FindWithTag(playerTag).transform;
        //gaugeBar = gameObject.GetComponent< Michsky.MUIP.ProgressBar>();
    }

    private void Update()
    {
        if (dead) return; //죽으면 업데이트X

        if(GameManager.instance != null) //게임 오버되면 업데이트x
        {
            if(GameManager.instance.isGameover)
            {
                return;
            }
        }

        if (pathFinder.remainingDistance >= pathFinder.stoppingDistance)
        {
            enemyAnimator.SetFloat("MOVE", 1f);
        }
        else
        {
            enemyAnimator.SetFloat("MOVE", 0f);
        }
        //if (pathFinder.isStopped)
        //{
        //    enemyAnimator.SetFloat("MOVE", 0f);
        //}
        //else
        //{
        //    enemyAnimator.SetFloat("MOVE", 1f);
        //}
        enemyAnimator.SetFloat("SPEED", pathFinder.speed);

        if (isWatingReset && !isWakeUp)
        {
            resetTimer += Time.deltaTime;
            //Debug.Log(resetTimer);
            if(resetTimer > resetTime)
            {
                resetTimer = 0f;
                gauge = 0;
                isWatingReset = false;
            }
        }

        FindTarget(); //시야의 타겟을 감지함

        //Debug.Log(state);
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

        if (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK"))
        {
            pathFinder.isStopped = true;
        }

        gaugeBar.ChangeValue(gauge);
        gaugeBar.gameObject.transform.LookAt(player);
        Vector3 currentRotation = transform.rotation.eulerAngles;
        gaugeBar.gameObject.transform.rotation = Quaternion.Euler(0f, currentRotation.y, 0f);
        //Debug.Log(pathFinder.isStopped);

    }

    private void IdleUpdate()
    {
        pathFinder.isStopped = true;
        if (hasTarget) //시야안에 플레이어가 들어올때
        {
            if (gauge >= 100)
            {
                pathFinder.speed = runSpeed; //2;
                state = State.Trace;
            }
            else
            {
                state = State.Doubt;
            }
        }
        else if(!isWaiting)
        {
            StartCoroutine(WaitForWalk());
        }
    }
    private void WalkUpdate()
    {
        pathFinder.isStopped = false;
        if(hasTarget)
        {
            if (gauge >= 100)
            {
                pathFinder.speed = runSpeed; // 2;
                state = State.Trace;
            }
            else
            {
                state = State.Doubt;
                //pathFinder.isStopped = true;
            }
            SetNextPos();
            //nextWayPointIndex++;
            //nextPos = WayPoints[nextWayPointIndex];
        }
        else
        {
            if (pathFinder.remainingDistance <= pathFinder.stoppingDistance) //도착하면 아이들 상태로 전환
            {
                SetNextPos();
                //nextWayPointIndex++;
                //nextPos = WayPoints[nextWayPointIndex];
                state = State.Idle;
            }
            else
            {
                MoveToPos(nextPos);
            }
        }
    }

    private void DoubtUpdate()
    {
        if (!hasTarget)
        {
            state = State.Idle;
            gauge = 0;
            return;
        }
        if(hasTargetInFOV)
        {
            GaugeUp();
        }

        MoveToPos(target.transform.position);
        //Debug.Log(pathFinder.remainingDistance);
        //if (pathFinder.remainingDistance >= pathFinder.stoppingDistance)
        //{
        //    MoveToPos(target.transform.position);
        //}
        //else
        //{
        //    pathFinder.isStopped = true;
        //}
    }

    private void TraceUpdate()
    {
        if (!hasTarget)
        {
            state = State.Idle;
            pathFinder.speed = walkSpeed; //1;
            isWatingReset = true;
            return;
        }
        MoveToPos(target.transform.position);
        //if (pathFinder.remainingDistance >= pathFinder.stoppingDistance)
        //{
        //    MoveToPos(target.transform.position);
        //}
        //else
        //{
        //    pathFinder.isStopped = true;
        //}
    }

    private IEnumerator WaitForWalk() //3초 대기 후 워크 상태로 이동
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);

        isWaiting = false;
        state = State.Walk;
        pathFinder.SetDestination(nextPos);
    }

    private IEnumerator WaitForWakeUp()
    {
        yield return new WaitForSeconds(wakeUptime);
        OnEnable();
    }
    private void MoveToPos(Vector3 pos)
    {
        //transform.LookAt(pos);
        Vector3 lookDirection = (pos - transform.position).normalized;

        // 서서히 회전할 속도 설정
        float rotationSpeed = 5f;

        // 현재 방향을 서서히 목표 방향으로 보간
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        pathFinder.isStopped = false;
        pathFinder.SetDestination(pos);
    }

    private void FindTarget()
    {
        if (fieldOfView.player != null)
        {
            targetTimer = 0f;
            hasTargetInFOV = true;
            target = fieldOfView.player.GetComponent<LivingEntity>();
            if(isWatingReset)
            {
                isWatingReset = false;
                resetTimer = 0f;
            }
        }
        else
        {
            hasTargetInFOV = false;
            if (state == State.Trace)
            {
                //trace 상태에서는 장애물들을 무시하고 범의 내의 플레이어를 찾음
                Collider[] colliders = Physics.OverlapSphere(transform.position, soundRadius, targetLayer);
                bool isTarget = false;

                foreach(Collider collider in colliders)
                {
                    var livingEntity = collider.GetComponent<LivingEntity>();
                    if(livingEntity != null )
                    {
                        isTarget = true;
                        target = livingEntity.GetComponent<LivingEntity>();
                        
                    }
                }

                if(!isTarget)
                {
                    TimerUp();
                }
                else
                {
                    targetTimer = 0f;
                }   
            }
            else
            {
                if(!FindNearTarget())
                {
                    TimerUp();
                }
            }
        }
    }

    private bool FindNearTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, nearRadius);
        
        foreach (Collider collider in colliders)
        {
            if (collider.tag == playerTag)
            {
                Vector3 start = transform.position; // 선의 시작 위치
                start.y = 1f;
                Vector3 end = collider.transform.position; // 선의 종료 위치
                end.y = 1f;

                RaycastHit hit;
                Ray ray = new Ray(start, end - start);
                
                Physics.Raycast(ray, out hit, nearRadius);
                if(hit.transform.tag != null)
                {
                    if (hit.transform.tag == playerTag)
                    {
                        target = hit.transform.GetComponent<LivingEntity>();
                        //Debug.Log("true");
                        return true;
                    }
                    else
                    {
                        //Debug.Log("false");
                        return false;
                    }
                }
            }
        }
        target = null;
        return false;
    }

    private void GaugeUp()
    {
        if (gauge <= maxGauge)
        {
            gauge += rate * Time.deltaTime;
        }
        if (gauge >= maxGauge)
        {
            gauge = maxGauge;
            state = State.Trace;
            pathFinder.speed = runSpeed; // 2;
        }
        //Debug.Log(gauge);
    }

    private void SetNextPos()
    {
        if(isReversing)
        {
            nextWayPointIndex--;

            if(nextWayPointIndex < 0 )
            {
                nextWayPointIndex = 1;
                isReversing = false;
            }
        }
        else
        {
            nextWayPointIndex++;

            if(nextWayPointIndex >= wayPointCount)
            {
                nextWayPointIndex = wayPointCount - 2;

                if (nextWayPointIndex < 0)
                    nextWayPointIndex = 0;
                isReversing = true;
            }
        }
        nextPos = WayPoints[nextWayPointIndex];
    }

    private void TimerUp()
    {
        targetTimer += Time.deltaTime;

        if (targetTimer >= targetTime)
        {
            target = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(state == State.Trace)
        {
            //pathFinder.isStopped = true;
            //enemyAnimator.SetFloat("MOVE", 0.1f);
            if (Time.time > lastAttackTime + timeBetAttack && other.gameObject == target.gameObject && !dead)
            {
                //pathFinder.isStopped = true;
                lastAttackTime = Time.time;
                target.OnDamage(damage);
                enemyAnimator.SetTrigger("ATTACK");
                
            }
        }
    }

    public override void OnDamage(float damage)
    {
        // LivingEntity의 OnDamage() 실행(데미지 적용)
        base.OnDamage(damage);
        //Debug.Log($"enemy health: {health}");
    }

    protected override void OnEnable()
    {
        if(dead)
        {
            enemyAnimator.SetTrigger("WAKE");
            gauge = 100;
        }
        base.OnEnable();
        isWakeUp = true;
        target = null;
        state = State.Idle;
    }

    // 사망 처리
    public override void Die()
    {
        // LivingEntity의 Die() 실행(사망 적용)
        pathFinder.isStopped = true;
        base.Die();
        enemyAnimator.SetTrigger("DIE"); 
        StartCoroutine(WaitForWakeUp());
        //playerAnimator.SetTrigger("Die");
        //playerMovement.enabled = false;
        //playerAnimator.SetTrigger("Die");

    }
}
