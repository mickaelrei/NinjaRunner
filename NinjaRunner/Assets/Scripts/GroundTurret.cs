using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTurret : Enemy
{
    public float fireDelay = 1f;
    public float bulletSpeed = 3f;
    public float rotateLerp = .2f;
    public float findDistance = 15f;
    public Transform target;
    public Transform bulletTemplate;
    public Transform firePoint;
    public Transform directionPoint;
    public LayerMask raycastMask;
    private float lastFireTime;
    private Transform rotateY;
    private float lastAngleY;
    private Player targetPlayer;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        // Find player if target is not specified
        if (!target) {
            target = GameObject.FindWithTag("Player").transform.Find("Head");
        }

        // Set some variables
        rotateY = transform.Find("RotateY");
        lastFireTime = -fireDelay;
        // bulletTemplate.gameObject.SetActive(false);
        lastAngleY = rotateY.eulerAngles.y;
        targetPlayer = target.GetComponentInParent<Player>();
    }

    bool CanSeeTarget() {
        Vector3 direction = (target.position - firePoint.position).normalized;
        RaycastHit[] hits = Physics.RaycastAll(firePoint.position, direction, findDistance + 10f, raycastMask);
        Transform closest = target;
        float minDistance = Mathf.Infinity;
        foreach (RaycastHit hit in hits) {
            float distance = (hit.transform.position - firePoint.position).magnitude;
            if (distance < minDistance) {
                minDistance = distance;
                closest = hit.transform;
            }
        }

        if (hits.Length > 0) {
            // Debug.Log("Hit something");
            if (closest.tag == "Player") {
                // Debug.Log("Hit player");
                return true;
            } else {
                // Debug.Log("Hit not player");
                return false;
            }
        } else {
            // Debug.Log("Hit nothing");
            return true;
        }
    }

    bool CanShoot() {
        if (!(target && targetPlayer))
            return false;

        float distance = (target.position - firePoint.position).magnitude;
        
        return currentHealth != 0 && targetPlayer.GetHealth() != 0 && distance < findDistance && CanSeeTarget();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (isDead) {
            transform.localScale = new Vector3(.5f, .5f, .5f);
        }

        if (!CanShoot()) {
            return;
        }

        // Rotate towards player
        Vector3 direction = target.position - directionPoint.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // firePoint.position += firePoint.forward * Time.deltaTime;

        float targetAngleY = Mathf.LerpAngle(lastAngleY, lookRotation.eulerAngles.y, rotateLerp);
        rotateY.Rotate(0f, -lastAngleY, 0f, Space.World);
        rotateY.Rotate(0f, targetAngleY, 0f, Space.World);
        lastAngleY = targetAngleY;

        // Check if can fire
        if (Time.time > lastFireTime + fireDelay) {
            lastFireTime = Time.time;

            // The turret shoots forward, so create a direction
            Vector3 shootDirection = firePoint.forward;
            Quaternion shootRotation = Quaternion.LookRotation(shootDirection);

            // Create bullet clone
            Transform bulletClone = Instantiate(bulletTemplate, firePoint.position, shootRotation * Quaternion.Euler(90f, 0f, 0f));
            bulletClone.GetComponent<MeshRenderer>().enabled = false;

            // Change attributes of bullet
            Bullet bulletScript = bulletClone.GetComponent<Bullet>();
            bulletScript.speed = bulletSpeed;
            bulletScript.direction = Vector3.Normalize(shootDirection);
            bulletScript.damage = damage;
        }
    }

    protected override void Die() {
        base.Die();
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(firePoint.position, findDistance);
    }
}