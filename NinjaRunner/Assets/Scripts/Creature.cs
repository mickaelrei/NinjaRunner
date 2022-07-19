using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public float maxHealth = 10f;
    [SerializeField] private protected float currentHealth;
    private bool isDead;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage) {
        if (isDead) {
            return;
        }

        // Calculate damage
        float damageTaken = Mathf.Min(currentHealth, damage);
        currentHealth -= damageTaken;
        // Debug.Log(gameObject.name + " took " + damageTaken + " damage.");

        // If took enough damage to die, call method
        if (currentHealth == 0f) {
            Die();
        }
    }

    public float GetHealth() {
        return currentHealth;
    }

    protected virtual void Heal(float health) {
        if (isDead) {
            return;
        }

        // Heal
        currentHealth = Mathf.Min(maxHealth, currentHealth + health);
    }

    protected virtual void Die() {
        isDead = true;
        Debug.Log(gameObject.name + " died!");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
