using UnityEngine;
using TMPro;
using System.Collections;

public class GameController : MonoBehaviour
{
    EnemyMissileSpawner myEnemyMissileSpawner;
    
    public int score = 0;
    public int level = 1;
    public int playerMissilesLeft = 30;
    public int enemyMissilesLeftInRound;
    private int enemyMIssilesPerRound = 20;
    private bool roundEnded = false;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI missilesLeftText;
    [SerializeField] private GameObject EndOfRoundPanel;
    [SerializeField] private int missileEndOfRoundPoints = 5;
    [SerializeField] private int cityEndOfRoundPoints = 100;

    [SerializeField] private TextMeshProUGUI leftOverMissileBonusText;
    [SerializeField] private TextMeshProUGUI leftOverCityBonusText;
    [SerializeField] private TextMeshProUGUI totalBonusText;
    [SerializeField] private TextMeshProUGUI countdownText;

    void Start()
    {
        myEnemyMissileSpawner = Object.FindFirstObjectByType<EnemyMissileSpawner>();
        UpdateMissilesLeftText();
        startRound();

    }

    void Update()
    {
        if (enemyMissilesLeftInRound <= 0 && !roundEnded)
        {
            roundEnded = true;
            EndRound();
        }
    }

    private void EndRound()
    {
       
        if (enemyMissilesLeftInRound <= 0)
        {
            StartCoroutine(EndOfRound());
        }
    }

    public void UpdateMissilesLeftText()
    {
        missilesLeftText.text = "Missiles Left: " + playerMissilesLeft;
    }

    public void UpdateScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
    }

    public void EnemyMissileDestroyed()
    {
        enemyMissilesLeftInRound--;
        Debug.Log("Enemy missiles left in round (Update): " + enemyMissilesLeftInRound);

    }

    private void startRound() 
    {
        myEnemyMissileSpawner.missilesToSpawnThisRound = enemyMIssilesPerRound;
        enemyMissilesLeftInRound = enemyMIssilesPerRound;
        myEnemyMissileSpawner.StartCoroutine(myEnemyMissileSpawner.SpawnMissiles());
    }

    public IEnumerator EndOfRound()
    {
        yield return new WaitForSeconds(.5f);
        EndOfRoundPanel.SetActive(true);
        int leftOverMissilesBonus = playerMissilesLeft * missileEndOfRoundPoints;

        GameObject[] cities = GameObject.FindGameObjectsWithTag("Base");
        Debug.Log("Number of cities left: " + cities.Length);
        int leftOverCityBonus = cityEndOfRoundPoints * cities.Length;

        int totalBonus = leftOverMissilesBonus + leftOverCityBonus;
        leftOverMissileBonusText.text = "Missile Bonus: " + leftOverMissilesBonus;
        leftOverCityBonusText.text = "City Bonus: " + leftOverCityBonus;
        totalBonusText.text = "Total Bonus: " + totalBonus;
        
        score += totalBonus;

    for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        //prepare for next round
        level++;
        levelText.text = "Level: " + level;
        enemyMIssilesPerRound += 5;
        playerMissilesLeft = 30 + (level - 1) * 5;
        UpdateMissilesLeftText();
        roundEnded = false;
        EndOfRoundPanel.SetActive(false);
        startRound();

    }
}
