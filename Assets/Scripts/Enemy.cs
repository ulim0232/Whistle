using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    //����
    public State state { get; private set; }

    //�ӵ�
    public float runSpeed = 4;
    public float walkSpeed = 2;

    //�ð�
    private float waitTime = 3f;

    private float targetTimer = 0f;
    private float targetTime = 5f;

    private float resetTime = 60f; //1�� �� ������ �ʱ�ȭ
    private float resetTimer = 0f;

    private float lastAttackTime = 0;
    private float timeBetAttack = 2f;

    //������
    public float gauge = 0; //���� ������
    private float maxGauge = 100f;

    private float rate = 15f; //�ʴ� ������ ������ ��

    //��ġ
    public Vector3 nextPos; //���� �̵� ��ġ
    //List<Vector3> WayPoints = new List<Vector3>();
    private Vector3[] WayPoints;
    public Transform[] WayPointTransforms;
    public int wayPointCount = 0;
    public int nextWayPointIndex = 0;

    //������Ʈ
    public LivingEntity target; //������ �÷��̾�
    NavMeshAgent pathFinder; //���� ��Ʈ�� ���
    public FieldOfView fieldOfView;
    public LayerMask targetLayer;

    //bool
    private bool isWaiting = false;
    private bool hasTargetInFOV = false;
    private bool isWatingReset = false;
    private bool isReversing = false;

    //�Ҹ�����
    private float soundRadius = 5f;

    public float damage = 10f;

    //�ִϸ��̼�
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
    }

    private void Update()
    {
        if (dead) return; //������ ������ƮX

        if(GameManager.instance != null) //���� �����Ǹ� ������Ʈx
        {
            if(GameManager.instance.isGameover)
            {
                return;
            }
        }
        if (pathFinder.isStopped)
        {
            enemyAnimator.SetFloat("MOVE", 0f);
        }
        else
        {
            enemyAnimator.SetFloat("MOVE", 1f);
        }
        enemyAnimator.SetFloat("SPEED", pathFinder.speed);


        if (isWatingReset)
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

        FindTarget(); //�þ��� Ÿ���� ������

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

    }

    private void IdleUpdate()
    {
        pathFinder.isStopped = true;
        if (hasTarget) //�þ߾ȿ� �÷��̾ ���ö�
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
            }
            SetNextPos();
            //nextWayPointIndex++;
            //nextPos = WayPoints[nextWayPointIndex];
        }
        else
        {
            if (pathFinder.remainingDistance <= pathFinder.stoppingDistance) //�����ϸ� ���̵� ���·� ��ȯ
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
    }

    private IEnumerator WaitForWalk() //3�� ��� �� ��ũ ���·� �̵�
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);

        isWaiting = false;
        state = State.Walk;
        pathFinder.SetDestination(nextPos);
    }

    private void MoveToPos(Vector3 pos)
    {
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
                Collider[] colliders = Physics.OverlapSphere(transform.position, soundRadius, targetLayer);
                bool isTarget = false;

                foreach(Collider collider in colliders)
                {
                    var livingEntity = collider.GetComponent<LivingEntity>();
                    if(livingEntity != null )
                    {
                        isTarget = true;
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
                TimerUp();
            }
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
            pathFinder.speed = runSpeed; // 2;
        }
        //Debug.Log(gauge);
    }

    private Vector3 GetRandomPos()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 5;
        randomDirection += transform.position;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, 5, NavMesh.AllAreas))
        {
            return hit.position;
        }

        // ������ ��� �ٽ� �õ��ϰų� ���� ó��
        return transform.position;
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
                lastAttackTime = Time.time;
                target.OnDamage(damage);
                enemyAnimator.SetTrigger("ATTACK");
                
            }
        }
    }

    public override void OnDamage(float damage)
    {
        // LivingEntity�� OnDamage() ����(������ ����)
        base.OnDamage(damage);
        Debug.Log($"enemy health: {health}");
    }

    // ��� ó��
    public override void Die()
    {
        // LivingEntity�� Die() ����(��� ����)
        base.Die();
        enemyAnimator.SetTrigger("DIE");
        //playerAnimator.SetTrigger("Die");
        //playerMovement.enabled = false;
        //playerAnimator.SetTrigger("Die");

    }
}
