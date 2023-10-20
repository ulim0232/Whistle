using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class KeySpawner : MonoBehaviour
{
    public GameObject keyPrefab;

    public GameObject spawnPointsObj; //생성할 게임 위치가 담긴 오브젝트의 부모
    public int keyCount = 1;

    private Transform[] spawnPoints;
    private Quaternion rotation = Quaternion.Euler(-90, 0, 0);

    private List<int> useSpawnIndex = new List<int>();
    void Start()
    {
        SetSpawnPosList();

        CreateBooks();

    }

    private void SetSpawnPosList()
    {
        spawnPoints = spawnPointsObj.GetComponentsInChildren<Transform>();
        List<Transform> list = new List<Transform>();

        foreach (Transform t in spawnPoints)
        {
            if (t.position != spawnPointsObj.transform.position)
            {
                list.Add(t);
            }
        }
        spawnPoints = list.ToArray();
    }

    private void CreateBooks()
    {
        for (int i = 0; i < keyCount; i++)
        {
            int randomIndex = 0;
            do
            {
                randomIndex = Random.Range(0, spawnPoints.Length);
            } while (useSpawnIndex.Contains(randomIndex));

            Instantiate(keyPrefab, spawnPoints[randomIndex].position, keyPrefab.transform.rotation);
            useSpawnIndex.Add(randomIndex);
            Debug.Log(spawnPoints[randomIndex].name);
        }
    }
}
