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

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI missilesLeftText;
    [SerializeField] private GameObject EndOfRoundPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myEnemyMissileSpawner = Object.FindFirstObjectByType<EnemyMissileSpawner>();
        UpdateMissilesLeftText();
        nextLevel();
    }

    void Update()
    {
        if (enemyMissilesLeftInRound <= 0)
        {
            Debug.Log("Level " + level + " complete!");
            EndOfRoundPanel.SetActive(true);
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

    private void nextLevel() 
    {
    myEnemyMissileSpawner.missilesToSpawnThisRound = enemyMIssilesPerRound + (level * 5);
        enemyMissilesLeftInRound = myEnemyMissileSpawner.missilesToSpawnThisRound;
        myEnemyMissileSpawner.StartCoroutine(myEnemyMissileSpawner.SpawnMissiles());
    }
}
