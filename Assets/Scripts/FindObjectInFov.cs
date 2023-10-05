using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FindObjectInFov : MonoBehaviour
{
    //public float Distance = 5f;
    //public LayerMask layerMask;
    //public string targetTag = "key"; //찾을 게임 오브젝트의 태그
    //public float maxAngle = 45; //최대 시야 각도

    //public GameObject lookAt;

    //private List<GameObject> keys; //찾은 key 들을 저장할 배열

    //private void Start()
    //{
    //    keys = new List<GameObject>(); //초기 키 배열
    //}

    //private void Update()
    //{

    //}

    // 시야 영역의 반지름과 시야 각도
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public Material defaultMaterial;
    public Material newMaterial;

    // 마스크 2종
    public LayerMask targetMask, obstacleMask;

    // Target mask에 ray hit된 transform을 보관하는 리스트
    public List<Transform> visibleTargets = new List<Transform>();

    void Start()
    {
        // 0.2초 간격으로 코루틴 호출
        StartCoroutine(FindTargetsWithDelay(0.2f));
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        // viewRadius를 반지름으로 한 원 영역 내 targetMask 레이어인 콜라이더를 모두 가져옴
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        // 시야 밖으로 나간 타겟을 추적하기 위한 리스트
        List<Transform> targetsOutsideView = new List<Transform>();

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            // 플레이어와 forward와 target이 이루는 각이 설정한 각도 내라면
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

                // 타겟으로 가는 레이캐스트에 obstacleMask가 걸리지 않으면 visibleTargets에 Add
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                    Renderer targetRenderer = target.GetComponent<Renderer>();
                    targetRenderer.material = newMaterial;
                    Debug.Log(target.name);
                }
            }
            else
            {
                // 시야 밖으로 나간 타겟을 리스트에 추가
                targetsOutsideView.Add(target);
            }
        }

        // 시야 밖으로 나간 타겟에 대한 처리
        foreach (Transform target in targetsOutsideView)
        {
            // renderer 컴포넌트가 있는지 확인
            Renderer targetRenderer = target.GetComponent<Renderer>();
            if (targetRenderer != null)
            {
                // 시야 밖으로 나간 타겟의 material을 defaultMaterial로 변경
                targetRenderer.material = defaultMaterial;
            }
        }
    }

    // y축 오일러 각을 3차원 방향 벡터로 변환한다.
    // 원본과 구현이 살짝 다름에 주의. 결과는 같다.
    public Vector3 DirFromAngle(float angleDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Cos((-angleDegrees + 90) * Mathf.Deg2Rad), 0, Mathf.Sin((-angleDegrees + 90) * Mathf.Deg2Rad));
    }

}
