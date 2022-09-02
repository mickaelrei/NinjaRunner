using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Player player;
    public float attackDamage = 3f;
    public float attackDelay = .5f;
    public LayerMask enemyLayer;
    public float jumpLerp = .05f;
    public float speedLerp = .2f;
    private Animator animator;
    private int availableJumps;
    private float targetSpeed;
    private float lerpThreshold = .01f;
    protected float lastAttackTime;
    protected List<Enemy> lastAttackEnemies;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (!player) {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }

        animator = GetComponent<Animator>();
        player.currentWeapon = this;
        availableJumps = player.maxJumps;
        
        lastAttackTime = -attackDelay;
        lastAttackEnemies = new List<Enemy>();
    }

    protected virtual void ChangeAvailableJumps(int newAvailableJumps) {
        availableJumps = newAvailableJumps;
    }

    private void LerpSpeed() {
        float speed = Mathf.Lerp(animator.GetFloat("Speed"), targetSpeed, speedLerp);
        if (speed < lerpThreshold) {
            speed = 0f;
        } else if (Mathf.Abs(speed - targetSpeed) < lerpThreshold) {
            speed = targetSpeed;
        }
        animator.SetFloat("Speed", speed);
    }

    private void LerpJump() {
        float jump = Mathf.Lerp(animator.GetFloat("Jump"), 0f, jumpLerp);
        if (jump < lerpThreshold) {
            jump = 0f;
        }
        animator.SetFloat("Jump", jump);
    }

    protected virtual void Fire() {
        lastAttackTime = Time.time;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // Check for mouse click
        if (Input.GetButtonDown("Fire1") && Time.time > lastAttackTime + attackDelay) {
            Fire();
            animator.SetTrigger("Attack");
        }

        // Check for jumping
        if (Input.GetKeyDown(KeyCode.Space)) {
            // Check if has available jumps (might put into other script or make that a public method)
            if (availableJumps > 0) {
                animator.SetFloat("Jump", 1f);
            }
        } else {
            // Decrease jump
            LerpJump();
        }

        // Change target speed based on movement
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
            // Moving
            if (Input.GetKey(KeyCode.LeftShift)) {
                // Running
                targetSpeed = 1f;
            } else {
                // Only walking
                targetSpeed = .1f;
            }
        } else {
            // Not moving
            targetSpeed = 0f;
        }
        LerpSpeed();
    }
}