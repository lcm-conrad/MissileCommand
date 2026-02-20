using UnityEngine;

public class PlayerMissileController : MonoBehaviour
{
    private Vector2 target;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float stoppingDistance = 0.01f;
    private bool hasTarget;

    [SerializeField] private GameObject explosionPrefab;

    [SerializeField] private AudioClip explosionSound;
    public void SetTarget(Vector2 targetPosition)
    {
        target = targetPosition;
        hasTarget = true;
    }

    void Update()
    {
        if (!hasTarget)
            return;

        float distance = Vector2.Distance(transform.position, target);
        
        if (distance <= stoppingDistance)
        {
            OnTargetReached();
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    private void OnTargetReached()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);

        }
        else
        {
            Debug.LogWarning("Explosion prefab is not assigned on " + gameObject.name);
        }

        hasTarget = false;
        Destroy(gameObject);
    }
}
