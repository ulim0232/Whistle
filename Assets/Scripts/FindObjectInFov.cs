using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FindObjectInFov : MonoBehaviour
{
    //public float Distance = 5f;
    //public LayerMask layerMask;
    //public string targetTag = "key"; //ã�� ���� ������Ʈ�� �±�
    //public float maxAngle = 45; //�ִ� �þ� ����

    //public GameObject lookAt;

    //private List<GameObject> keys; //ã�� key ���� ������ �迭

    //private void Start()
    //{
    //    keys = new List<GameObject>(); //�ʱ� Ű �迭
    //}

    //private void Update()
    //{

    //}

    // �þ� ������ �������� �þ� ����
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public Material defaultMaterial;
    public Material newMaterial;

    // ����ũ 2��
    public LayerMask targetMask, obstacleMask;

    // Target mask�� ray hit�� transform�� �����ϴ� ����Ʈ
    public List<Transform> visibleTargets = new List<Transform>();

    void Start()
    {
        // 0.2�� �������� �ڷ�ƾ ȣ��
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
        // viewRadius�� ���������� �� �� ���� �� targetMask ���̾��� �ݶ��̴��� ��� ������
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        // �þ� ������ ���� Ÿ���� �����ϱ� ���� ����Ʈ
        List<Transform> targetsOutsideView = new List<Transform>();

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            // �÷��̾�� forward�� target�� �̷�� ���� ������ ���� �����
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

                // Ÿ������ ���� ����ĳ��Ʈ�� obstacleMask�� �ɸ��� ������ visibleTargets�� Add
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
                // �þ� ������ ���� Ÿ���� ����Ʈ�� �߰�
                targetsOutsideView.Add(target);
            }
        }

        // �þ� ������ ���� Ÿ�ٿ� ���� ó��
        foreach (Transform target in targetsOutsideView)
        {
            // renderer ������Ʈ�� �ִ��� Ȯ��
            Renderer targetRenderer = target.GetComponent<Renderer>();
            if (targetRenderer != null)
            {
                // �þ� ������ ���� Ÿ���� material�� defaultMaterial�� ����
                targetRenderer.material = defaultMaterial;
            }
        }
    }

    // y�� ���Ϸ� ���� 3���� ���� ���ͷ� ��ȯ�Ѵ�.
    // ������ ������ ��¦ �ٸ��� ����. ����� ����.
    public Vector3 DirFromAngle(float angleDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Cos((-angleDegrees + 90) * Mathf.Deg2Rad), 0, Mathf.Sin((-angleDegrees + 90) * Mathf.Deg2Rad));
    }

}
