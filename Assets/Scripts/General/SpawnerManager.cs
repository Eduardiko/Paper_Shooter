using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;


public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private GameObject swordFishPrefab;
    [SerializeField] private GameObject pufferFishPrefab;
    [SerializeField] private GameObject kelpPrefab;
    [SerializeField] private GameObject crabPrefab;

    private List<GameObject> spawners;
    private List<GameObject> enemyPrefabs;

    private List<GameObject> currentSpawnedEntities;

    private bool canSpawnNextWave;

    private List<int[]> patternList;

    private float distanceBetweenColumns = 3f;

    private void Start()
    {
        spawners = new List<GameObject>();
        currentSpawnedEntities = new List<GameObject>();
        enemyPrefabs = new List<GameObject>();
        patternList = new List<int[]>();

        enemyPrefabs.Add(swordFishPrefab);
        enemyPrefabs.Add(pufferFishPrefab);
        enemyPrefabs.Add(kelpPrefab);
        enemyPrefabs.Add(crabPrefab);

        SetPatterns();

        SetSpawners();
    }

    private void Update()
    {
        if(IsListEntirelyNull(currentSpawnedEntities))
            SpawnRandomWave();
    }

    void SpawnRandomWave()
    {
        int patternIndex = Random.Range(0, patternList.Count);

        Vector3 horizontalOffset = Vector3.zero;

        for (int i = 0; i < patternList[patternIndex].Length; i++)
        {
            if (patternList[patternIndex][i] == -1)
                continue;

            GameObject prefabToInstantiate = enemyPrefabs[patternList[patternIndex][i]];

            if (i % 5 == 0 && i != 0)
                horizontalOffset += new Vector3(distanceBetweenColumns, 0f, 0f);

            Vector3 spawnPosition = spawners[i % spawners.Count].transform.position;
            Quaternion spawnRotation = spawners[i % spawners.Count].transform.rotation;

            currentSpawnedEntities.Add(GameObject.Instantiate(prefabToInstantiate, spawnPosition + horizontalOffset, spawnRotation));
        }
    }


    void SetSpawners()
    {
        foreach (Transform child in transform)
        {
            spawners.Add(child.gameObject);
        }
    }

    void SetPatterns()
    {
        //Hay maneras mejores de hacer esto
        patternList.Add(pattern1);
        patternList.Add(pattern2);
    }

    bool IsListEntirelyNull(List<GameObject> list)
    {
        foreach(GameObject t in list)
        {
            if (t != null)
                return false;
        }
        return true;
    }

    #region Patterns

    //Easy

    //Medium
    int[] pattern1 =
    { 
        0, 0, 0, 0, 0,
        1, -1, 1, -1, 1
    };

    int[] pattern2 =
    {
        -1, 1, 0, 1, -1,
        1, -1, -1, -1, 1
    };

    //Hard

    #endregion
}
