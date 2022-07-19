using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTurret : Enemy
{
    public float fireDelay = 1f;
    public float bulletSpeed = 3f;
    public float rotateLerp = .2f;
    public Transform target;
    public Transform bulletTemplate;
    public Transform firePoint;
    public Transform directionPoint;
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

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (!target || currentHealth == 0 || (targetPlayer && targetPlayer.GetHealth() == 0)) {
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
            // bulletScript.enabled = true;

            // Activate bullet clone
            // bulletClone.gameObject.SetActive(true);
        }
    } 
}