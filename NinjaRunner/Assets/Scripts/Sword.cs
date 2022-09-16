using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    
    public Transform attackPoint;
    public float attackRadius = 1f;
    public float attackDuration = .3f;
    private bool isAttacking = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void ChangeAvailableJumps(int newAvailableJumps)
    {
        base.ChangeAvailableJumps(newAvailableJumps);
    }

    // Update is called once per frame
    protected override void Update()
    {
        // Check if is currently attacking
        if (Time.time < lastAttackTime + attackDuration) {
            isAttacking = true;
        } else {
            isAttacking = false;

            // Clear last attack enemies list
            for (int i = 0; i < lastAttackEnemies.Count; i++)
            {
                lastAttackEnemies.RemoveAt(i);
            }
        }

        base.Update();
    }

    private void FixedUpdate() {
        if (!isAttacking) {
            return;
        }

        // Check for overlaps
        Collider[] colliders = Physics.OverlapSphere(attackPoint.position, attackRadius, enemyLayer);
        foreach (Collider coll in colliders)
        {
            Transform root = coll.transform.root;
            // Try to find enemy component
            Enemy enemy = root.GetComponent<Enemy>();
            if (!enemy) {
                continue;
            }

            // Try to apply damage on hit enemy
            if (enemy.GetHealth() > 0 && !lastAttackEnemies.Contains(enemy)) {
                Debug.Log("Attacked");
                lastAttackEnemies.Add(enemy);
                enemy.TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
