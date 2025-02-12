using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Player playerPrefab;
    [SerializeField] private GameObject healthUIPrefab;

    [SerializeField] private GameObject endLevelMenu;
    [SerializeField] private Transform healthUIGroup;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private Image gradesObject;
    [SerializeField] private Sprite[] grades;

    [HideInInspector] public static int playerScore = 0;

    private void Update()
    {
        scoreText.text = "Score: " + playerScore;

        UpdateGrade();

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

    private void UpdateGrade()
    {
        int i = 0;

        if (playerScore < 175)
            i = 0;
        else if (playerScore >= 175 && playerScore < 350)
            i = 1;
        else if (playerScore >= 350 && playerScore < 525)
            i = 2;
        else if (playerScore >= 525 && playerScore < 700)
            i = 3;
        else if (playerScore >= 700 && playerScore < 875)
            i = 4;
        else if (playerScore >= 875 && playerScore < 1050)
            i = 5;
        else if (playerScore >= 1050 && playerScore < 1225)
            i = 6;
        else if (playerScore >= 1225 && playerScore < 1400)
            i = 7;
        else if (playerScore >= 1400 && playerScore < 1575)
            i = 8;
        else if (playerScore >= 1575 && playerScore < 1750)
            i = 9;
        else if (playerScore >= 1750 && playerScore < 1925)
            i = 10;
        else if (playerScore >= 1925 && playerScore < 2275)
            i = 11;
        else if (playerScore >= 2275)
            i = 12;
        
        gradesObject.sprite = grades[i];
    }
}
