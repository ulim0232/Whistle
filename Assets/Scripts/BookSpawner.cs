using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class BookSpawner : MonoBehaviour
{
    //public MissionBook bookPrefab;
    public GameObject bookPrefab;

    public GameObject spawnPointsObj; //생성할 게임 위치가 담긴 오브젝트의 부모
    public int bookCount = 2;

    private Transform[] spawnPoints;
    private Vector3[] spawnPositions;

    private List<int> useSpawnIndex = new List<int>();


    private void Start()
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
        //spawnPositions = new Vector3[spawnPoints.Length];

        //for (int i = 0; i < spawnPoints.Length; i++)
        //{
        //    spawnPositions[i] = spawnPoints[i].transform.position;
        //}
    }

    private void CreateBooks()
    {
        for(int i = 0; i < bookCount; i++)
        {
            int randomIndex = 0;
            do
            {
                randomIndex = Random.Range(0, spawnPoints.Length);
            } while (useSpawnIndex.Contains(randomIndex));

            Instantiate(bookPrefab, spawnPoints[randomIndex].position, spawnPoints[randomIndex].rotation);
            useSpawnIndex.Add(randomIndex);
            Debug.Log(spawnPoints[randomIndex].name);
        }
    }

    private void SetBook()
    {
        
    }
}


