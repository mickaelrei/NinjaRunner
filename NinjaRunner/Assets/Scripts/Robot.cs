using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Robot : Enemy
{
    public Transform playerTransform;
    public Player player;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    public Transform center;
    private float currentSpeed;
    public float speedLerp = .03f;
    public float lockDistance = 30f;
    public float attackDistance = 1.5f;
    private float attackStartTime;
    private bool isAttacking = false;
    public float attackTime = 1.5f;
    public float sizeOnDeath = 5f;
    public LayerMask playerLayer;


    private void Awake() {
        base.Start();
        Debug.Log("Health: " + currentHealth);

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (!playerTransform) {
            playerTransform = GameObject.FindWithTag("Player").transform;
        }
        if (!player) {
            player = GameObject.FindObjectOfType<Player>();
        }
    }

    protected override void Update() {
        base.Update();

        if (player.GetHealth() == 0f || isDead) {
            currentSpeed = 0f;
            navMeshAgent.isStopped = true;
            transform.localScale = new Vector3(sizeOnDeath, sizeOnDeath, sizeOnDeath);
            HandleAnimations();
            return;
        }

        // Set destination
        float distance = Vector3.Distance(center.position, playerTransform.position);
        if (distance <= lockDistance) {
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = playerTransform.position;
        } else {
            navMeshAgent.isStopped = true;
        }

        // Check if can attack
        if (distance <= attackDistance) {
            if (isAttacking && Time.time > attackStartTime + attackTime) {
                // Kill player
                isAttacking = false;
                player.TakeDamage(player.GetHealth());
            } else if (!isAttacking) {
                isAttacking = true;
                attackStartTime = Time.time;
            }
        } else if (isAttacking) {
            isAttacking = false;
        }

        // Change animations
        HandleAnimations();
    }

    private void HandleAnimations() {
        // Debug.Log(navMeshAgent.velocity.magnitude);
        if (navMeshAgent.velocity.magnitude > 0.1f) {
            currentSpeed = Mathf.Lerp(currentSpeed, 1f, speedLerp);
        } else {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, speedLerp);
        }
        animator.SetFloat("Speed", currentSpeed);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(center.position, attackDistance);
    }
}
