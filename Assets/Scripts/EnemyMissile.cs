using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject ExplosionPrefab;

    GameObject[] Base;
    Vector3 target;
    private bool isDestroyed = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Base = GameObject.FindGameObjectsWithTag("Base");
        target = Base[Random.Range(0, Base.Length)].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDestroyed) return;

        if (collision.gameObject.CompareTag("Base"))
        {
            isDestroyed = true;
            MissileExplode();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Explosion"))
        {
            isDestroyed = true;
            Object.FindFirstObjectByType<GameController>().UpdateScore(5);
            MissileExplode();
            // Don't destroy the explosion - let it run its course
        }
    }

        private void MissileExplode()
        {
        Object.FindFirstObjectByType<GameController>().EnemyMissileDestroyed();
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            GameController gameController = Object.FindFirstObjectByType<GameController>();
            Destroy(gameObject);
        }

}

