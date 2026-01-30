using UnityEngine;
using TMPro;

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
        Debug.Log("Level " + level + " complete!");
        Debug.Log("EndOfRoundPanel is: " + (EndOfRoundPanel == null ? "NULL" : "assigned"));
        if (EndOfRoundPanel != null)
        {
            EndOfRoundPanel.SetActive(true);
            Debug.Log("Panel set to active");
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
        roundEnded = false;
        myEnemyMissileSpawner.missilesToSpawnThisRound = enemyMIssilesPerRound;
        enemyMissilesLeftInRound = enemyMIssilesPerRound;
        myEnemyMissileSpawner.StartCoroutine(myEnemyMissileSpawner.SpawnMissiles());
    }
}
