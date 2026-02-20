using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
    private float speed = 1f;
    [SerializeField] private GameObject ExplosionPrefab;
    GameObject[] Base;

    [SerializeField] private GameObject enemyMissilePrefab;


    private GameController myGameController;
    Vector3 target;
    private bool isDestroyed = false;

    [SerializeField] private AudioClip explosionSound;

    private float randomTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myGameController = Object.FindFirstObjectByType<GameController>();
        Base = GameObject.FindGameObjectsWithTag("Base");
        target = Base[Random.Range(0, Base.Length)].transform.position;
        
        speed *= myGameController.enemyMissileSpeed; 

        randomTimer = Random.Range(0.1f, 10f);
        randomTimer /= myGameController.enemyMissileSpeed; // Adjust the timer based on the missile speed
        Invoke(nameof(SplitMissile), randomTimer);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (transform.position == target && !isDestroyed)
        {
            isDestroyed = true;
            MissileExplode();
        }
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDestroyed) return;

        if (collision.gameObject.CompareTag("Base"))
        {
            isDestroyed = true;
            MissileExplode();
            if (collision.GetComponent<MissileLauncher>() != null)
            {
                    myGameController.playerMissilesLeft -= 10;
                    myGameController.UpdateMissilesLeftText();
                return;
            }
            myGameController.cityCounter--;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Explosion"))
        {
            isDestroyed = true;
            myGameController.UpdateScore(5);
            MissileExplode();
        }
    }

        private void MissileExplode()
        {
        myGameController.EnemyMissileDestroyed();
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        AudioSource.PlayClipAtPoint(explosionSound, transform.position);
    }

    private void SplitMissile()
    {
        float yValue = Camera.main.ViewportToWorldPoint(new Vector3(0, 1f, 0)).y;
        if (transform.position.y > yValue - 1) // Only split if the missile is still high enough
        {
            myGameController.enemyMissilesLeftInRound++;
            Instantiate(enemyMissilePrefab, transform.position, Quaternion.identity);
        }
    }
}

