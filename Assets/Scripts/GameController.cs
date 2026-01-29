using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    EnemyMissileSpawner myEnemyMissileSpawner;
    
    public int score = 0;
    public int level = 1;
    public int playerMissilesLeft = 30;
    private int enemyMissilesLeftInRound;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI missilesLeftText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myEnemyMissileSpawner = Object.FindFirstObjectByType<EnemyMissileSpawner>();
        UpdateMissilesLeftText();
        RoundStart();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    private void RoundStart()
    {
        myEnemyMissileSpawner.missilesToSpawnThisRound = enemyMissilesLeftInRound;

        myEnemyMissileSpawner.StartCoroutine(myEnemyMissileSpawner.SpawnMissiles());
    }
}
