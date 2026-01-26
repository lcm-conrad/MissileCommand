using UnityEngine;

public class PlayerMissileController : MonoBehaviour
{
    private Vector2 target;
    [SerializeField] private float speed = 5f;
    private bool hasTarget;
    private float stoppingDistance = 0.01f;

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
            hasTarget = false;
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
}
