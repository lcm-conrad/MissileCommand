using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

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
    [SerializeField] private float enemyMissileSpeedMultiplier = .5f;
    public float enemyMissileSpeed = 1f;
    public int currentMissilesLoadedInLauncher = 10;
    private bool isReloading = false;
    public int cityCounter;

    [SerializeField] private TextMeshProUGUI leftOverMissileBonusText;
    [SerializeField] private TextMeshProUGUI leftOverCityBonusText;
    [SerializeField] private TextMeshProUGUI totalBonusText;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private TextMeshProUGUI MissilesInLauncherText;

    void Start()
    {
        playerMissilesLeft -= currentMissilesLoadedInLauncher;
        myEnemyMissileSpawner = Object.FindFirstObjectByType<EnemyMissileSpawner>();
        cityCounter = GameObject.FindGameObjectsWithTag("Base").Length;
        Debug.Log("Number of cities: " + cityCounter);

        UpdateMissilesLeftText();
        UpdateMissilesInLauncherText();
        
        startRound();


    }

    void Update()
    {
        if (playerMissilesLeft < 0)
        {
            playerMissilesLeft = 0;
            UpdateMissilesLeftText();
        }

        if (enemyMissilesLeftInRound == 0 && !roundEnded)
        {
            roundEnded = true;
            StartCoroutine(EndOfRound());
        }

        if (currentMissilesLoadedInLauncher == 0 && !isReloading)
        {
            isReloading = true;
            StartCoroutine(ReloadMissileLauncher());
        }

        if (cityCounter <= 0)
        {
            SceneManager.LoadScene("TheEndScene");
            Debug.Log("Game Over! Final Score: " + score);
            // Here you can implement game over logic, such as showing a game over screen or restarting the game.
        }
        }

    public void UpdateMissilesLeftText()
    {
        missilesLeftText.text = "Missiles Left: " + playerMissilesLeft;
    }
    
    public void UpdateMissilesInLauncherText()
    {
        MissilesInLauncherText.text = "MIssiles in Launcher: " + currentMissilesLoadedInLauncher;
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

    public IEnumerator ReloadMissileLauncher()
    {
        yield return new WaitForSeconds(0.5f);

        
        currentMissilesLoadedInLauncher += 10;  
        playerMissilesLeft -= 10;                

        UpdateMissilesLeftText();
        UpdateMissilesInLauncherText();
        
        isReloading = false;
    }

    public IEnumerator EndOfRound()
    {
        yield return new WaitForSeconds(.5f);
        EndOfRoundPanel.SetActive(true);
        int leftOverMissilesBonus = (playerMissilesLeft + currentMissilesLoadedInLauncher) * missileEndOfRoundPoints;

        GameObject[] cities = GameObject.FindGameObjectsWithTag("Base");
        Debug.Log("Number of cities left: " + cities.Length);
        int leftOverCityBonus = cityEndOfRoundPoints * cities.Length;

        int totalBonus = leftOverMissilesBonus + leftOverCityBonus;
        leftOverMissileBonusText.text = "Missile Bonus: " + leftOverMissilesBonus;
        leftOverCityBonusText.text = "City Bonus: " + leftOverCityBonus;
        totalBonusText.text = "Total Bonus: " + totalBonus;

        int bonusMultiplier = 1;
        int[][] levelRanges = new int[][]
        {
            new int[] { 3, 5, 2 },
            new int[] { 5, 7, 3 },
            new int[] { 7, 9, 4 },
            new int[] { 9, 11, 5 },
            new int[] { 11, int.MaxValue, 6 }
        };

        foreach (int[] range in levelRanges)
        {
            if (level >= range[0] && level < range[1])
            {
                bonusMultiplier = range[2];
                break;
            }
        }

        totalBonus *= bonusMultiplier;
        totalBonusText.text = bonusMultiplier > 1 
            ? $"Total Bonus (x{bonusMultiplier}): {totalBonus}" 
            : $"Total Bonus: {totalBonus}";

            for (int i = 3; i > 0; i--)
            {
                countdownText.text = i.ToString();
                yield return new WaitForSeconds(1f);
            }

        score += totalBonus;
        UpdateScore(score);
        
        level++;
        levelText.text = "Level: " + level;
        
        enemyMIssilesPerRound += 5;
        playerMissilesLeft = 30 + (level - 1) * 5;
        currentMissilesLoadedInLauncher = 10;

        enemyMissileSpeed += enemyMissileSpeedMultiplier;
        if (enemyMissileSpeed > 3f)
        {
            enemyMissileSpeed = 3f;
        }

        UpdateMissilesLeftText();
        UpdateMissilesInLauncherText();

        roundEnded = false;
        EndOfRoundPanel.SetActive(false);
        
        startRound();

    }
}
