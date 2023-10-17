using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSpawner : MonoBehaviour
{
    public GameObject spawnPointsObj; //생성할 게임 위치가 담긴 오브젝트의 부모
    public int DataCount = 3;

    private Transform[] spawnPoints;
    private Vector3[] spawnPositions;

    private List<int> useSpawnIndex = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        SetSpawnPosList();
        SetAcitiveNoneDatas(false);
        CreateDatas();

    }

    private void Awake()
    { }

    private void SetSpawnPosList()
    {
        spawnPoints = spawnPointsObj.GetComponentsInChildren<Transform>();
        List<Transform> list = new List<Transform>();
        List<Transform> childList = new List<Transform>();

        foreach (Transform t in spawnPoints)
        {
            if (t.position != spawnPointsObj.transform.position)
            {
                list.Add(t);
            }
        }
        for(int i = 1; i <list.Count; i+=2)
        {
            childList.Add(list[i]);
        }
        spawnPoints = childList.ToArray();
    }

    private void SetAcitiveNoneDatas(bool isActive)
    {
        foreach (Transform t in spawnPoints)
        {
            t.gameObject.GetComponent<MissionData>().enabled = isActive;
            t.gameObject.GetComponent<Outline>().enabled = isActive;
        }
    }

    private void CreateDatas()
    {
        for (int i = 0; i < DataCount; i++)
        {
            int randomIndex = 0;
            do
            {
                randomIndex = Random.Range(0, spawnPoints.Length);
            } while (useSpawnIndex.Contains(randomIndex));

            spawnPoints[randomIndex].GetComponent<MissionData>().enabled = true;
            useSpawnIndex.Add(randomIndex);
            Debug.Log(spawnPoints[randomIndex].name);
        }
    }

}
