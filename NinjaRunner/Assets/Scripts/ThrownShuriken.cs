using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownShuriken : MonoBehaviour
{
    public float lifeTime;
    public float speed;
    public float attackRadius = 1f;
    public float raycastDistance;
    public LayerMask enemyLayer;
    private float startTime;
    private Vector3 up;
    private Vector3 direction;
    
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        direction = transform.forward;

        // Rotate sideways randomly
        transform.Rotate(transform.forward, Random.Range(-20f, 20f), Space.World);
        up = transform.up;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > startTime + lifeTime) {
            Destroy(gameObject);
            return;
        }
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRadius, enemyLayer);
        foreach (Collider coll in colliders)
        {
            // Try to find enemy component
            Enemy enemy = coll.GetComponent<Enemy>();
            if (!enemy) {
                continue;
            }


            // // Try to apply damage on hit enemy
            // if (enemy.GetHealth() > 0 && !lastAttackEnemies.Contains(enemy)) {
            //     lastAttackEnemies.Add(enemy);
            //     enemy.TakeDamage(attackDamage);
            // }
        }

        // Move
        transform.position += direction * speed * Time.deltaTime;
        transform.Rotate(up, speed, Space.World);
    }
}
