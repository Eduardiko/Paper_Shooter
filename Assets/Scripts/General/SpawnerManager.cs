using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;


public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private GameObject swordFishPrefab;
    [SerializeField] private GameObject pufferFishPrefab;
    [SerializeField] private GameObject kelpPrefab;
    [SerializeField] private GameObject crabPrefab;

    private List<GameObject> spawners;
    private List<GameObject> enemyPrefabs;
    private List<GameObject> currentSpawnedEntities;
    private List<int[]> patternList;

    private float distanceBetweenColumns = 3f;

    private int bossSpawnWave = 4;
    private int waveCount = 0;
    private int difficulty = 1;

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
        if (IsListEntirelyNull(currentSpawnedEntities))
        {
            if (waveCount < bossSpawnWave - 1)
                SpawnRandomWave();
            else
                SpawnBoss();
        }
    }

    void SpawnRandomWave()
    {
        AudioManager.Instance.PlaySFX(4);

        waveCount++;

        int patternIndex = Random.Range(0, patternList.Count);

        Vector3 horizontalOffset = Vector3.zero;

        for (int i = 0; i < patternList[patternIndex].Length; i++)
        {
            if (patternList[patternIndex][i] < 0)
                continue;

            GameObject prefabToInstantiate = enemyPrefabs[patternList[patternIndex][i]];

            if (i % 5 == 0 && i != 0)
                horizontalOffset += new Vector3(distanceBetweenColumns, 0f, 0f);

            Vector3 spawnPosition = spawners[i % spawners.Count].transform.position;
            Quaternion spawnRotation = spawners[i % spawners.Count].transform.rotation;

            GameObject spawnedEnemy = GameObject.Instantiate(prefabToInstantiate, spawnPosition + horizontalOffset, spawnRotation);

            currentSpawnedEntities.Add(spawnedEnemy);

            // The less rows, the less they will move, so that they don't stop in the middle of the screen
            //StartCoroutine(StartingWaveBehavior(spawnedEnemy, startingTranslationDistance * patternList[patternIndex].Length / maxSpawnedEnemiesNumber));
            StartCoroutine(StartingWaveBehavior(spawnedEnemy, 7 + 0.4f * (patternList[patternIndex].Length - 5)));
        }
    }

    void SpawnBoss(float distance = 6f)
    {
        AudioManager.Instance.PlaySFX(6, 0.2f);

        waveCount = 0;
        float waveLengthMultiplier = 1.2f;
        bossSpawnWave = Mathf.RoundToInt(bossSpawnWave * waveLengthMultiplier);

        GameObject spawnedEnemy = GameObject.Instantiate(crabPrefab, spawners[2].transform.position, Quaternion.identity);
        
        currentSpawnedEntities.Add(spawnedEnemy);

        StartCoroutine(StartingWaveBehavior(spawnedEnemy, distance));
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
        int[][][] patterns = new int[][][]
        {
        // Easy patterns
        new int[][] 
        {
            patternEasy1,
            patternEasy2,
            patternEasy3,
            patternEasy4,
            patternEasy5,
            patternEasy6
        },
        // Medium patterns
        new int[][] 
        {
            patternMedium1,
            patternMedium2,
            patternMedium3,
            patternMedium4,
            patternMedium5,
            patternMedium6
        },
        // Hard patterns
        new int[][]
        {
            patternHard1,
            patternHard2,
            patternHard3,
            patternHard4,
            patternHard5,
            patternHard6
        }
        };

        patternList.Clear();

        if (difficulty >= 1 && difficulty <= 3)
        {
            patternList.AddRange(patterns[difficulty - 1]);
        }
        else if (difficulty == 4)
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                patternList.AddRange(patterns[i]);
            }
        }
    }

    private IEnumerator StartingWaveBehavior(GameObject prefab, float distance)
    {
        if (prefab.gameObject.GetComponent<Crab>() != null)
        {
            prefab.gameObject.GetComponent<Crab>().SetPhase(difficulty);

            difficulty++;
            SetPatterns();
        }

        float duration = 1.5f;
        float stopTime = 1.5f;
        float elapsed = 0f;

        Vector3 startPosition = prefab.transform.position;
        Vector3 targetPosition = startPosition + Vector3.left * distance;

        while (elapsed < duration)
        {
            if (prefab.gameObject != null)
            {
                float t = elapsed / duration;
                prefab.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                elapsed += Time.deltaTime;
            }    

            yield return null;
        }

        if(prefab.gameObject != null)
            prefab.transform.position = targetPosition;

        yield return new WaitForSeconds(stopTime);

        if(prefab.gameObject != null)
        {
            prefab.gameObject.GetComponent<Mob>().canAct = true;

            if (prefab.gameObject.GetComponent<Crab>() != null)
            {
                foreach (Transform child in prefab.transform)
                {
                    CrabClaw crabClawChild = child.GetComponent<CrabClaw>();
                    Pufferfish pufferChild = child.GetComponent<Pufferfish>();

                    if (pufferChild != null && pufferChild.gameObject.activeSelf)
                        pufferChild.canAct = true;
                    else if (crabClawChild != null && crabClawChild.gameObject.activeSelf)
                        crabClawChild.canAct = true;
                }
            }
        }
    }

    bool IsListEntirelyNull(List<GameObject> list)
    {
        foreach (GameObject t in list)
        {
            if (t != null)
                return false;
        }
        return true;
    }
    

    #region Patterns

    //Easy
    int[] patternEasy1 =
    {
        2, -1, 1, -1, 2,
    };

    int[] patternEasy2 =
    {
        -1, 1, 0, 1, -1,
    };

    int[] patternEasy3 =
    {
        0, -1, 0, -1, 0,
    };

    int[] patternEasy4 =
    {
        1, -1, 1, -1, 1,
    };

    int[] patternEasy5 =
    {
        2, -1, 0, -1, 2,
    };

    int[] patternEasy6 =
    {
        0, -1, 1, -1, 0,
    };

    //Medium
    int[] patternMedium1 =
    { 
        0, 0, 0, 0, 0,
        1, -1, 1, -1, 1,
    };

    int[] patternMedium2 =
    {
        -1, 1, 0, 1, -1,
        1, -1, -1, -1, 1,
    };

    int[] patternMedium3 =
    {
        2, -1, 1, -1, 2,
        1, -1, 0, -1, 1,
    };

    int[] patternMedium4 =
    {
        -1, 0, 1, 0, -1,
        1, -1, -1, -1, 1,
    };

    int[] patternMedium5 =
    {
        1, 0, -1, 0, 1,
        2, -1, 1, -1, 2,
    };

    int[] patternMedium6 =
    {
        0, -1, 2, -1, 0,
        1, -1, -1, -1, 1,
    };

    //Hard
    int[] patternHard1 =
    {
        1, 0, -1, 0, 1,
        -1, -1, 1, -1, -1,
        0, 1, 0, 1, 0,
    };

    int[] patternHard2 =
    {
        1, 0, 1, 0, 1,
        0, 1, 0, 1, 0,
        1, 0, 1, 0, 1,
    };

    int[] patternHard3 =
    {
        0, 0, 0, 0, 0,
        2, -1, 0, -1, 2,
        1, 1, 1, 1, 1,
    };

    int[] patternHard4 =
    {
        2, -1, 1, -1, 2,
        2, -1, 0, -1, 2,
        2, -1, 1, -1, 2,
    };

    int[] patternHard5 =
    {
        -1, 2, -1, 2, -1,
        0, -1, 0, -1, 0,
        -1, 1, -1, 1, -1,
    };

    int[] patternHard6 =
    {
        0, -1, 2, -1, 0,
        1, -1, 2, -1, 1,
        1, -1, 2, -1, 1,
    };

    #endregion
}
