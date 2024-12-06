using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player playerPrefab;
    [SerializeField] private GameObject healthUIPrefab;

    [SerializeField] private GameObject endLevelMenu;
    [SerializeField] private Transform healthUIGroup;

    private void Update()
    {
        if (playerPrefab == null)
            endLevelMenu.SetActive(true);

        foreach(RectTransform child in healthUIGroup)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0; i < playerPrefab.health; i++)
        {
            GameObject.Instantiate(healthUIPrefab, healthUIGroup);
        }
    }
}
