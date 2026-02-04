using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
    private float speed = 1f;
    [SerializeField] private GameObject ExplosionPrefab;
    GameObject[] Base;

    private GameController myGameController;
    Vector3 target;
    private bool isDestroyed = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myGameController = Object.FindFirstObjectByType<GameController>();
        Base = GameObject.FindGameObjectsWithTag("Base");
        target = Base[Random.Range(0, Base.Length)].transform.position;
        speed *= myGameController.enemyMissileSpeed; 

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
        }

}

