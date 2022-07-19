using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTurret : Enemy
{
    public float fireDelay = 1f;
    public float bulletSpeed = 3f;
    public float rotateLerpY = .01f;
    public float rotateLerpX = .01f;
    public Transform target;
    public Transform bulletTemplate;
    public Transform firePoint;
    public Transform directionPoint;
    private float lastFireTime;
    private Transform rotateY;
    private Transform rotateX;
    private float lastAngleY;
    private float lastAngleX;
    private Transform rotatePointX;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        // Find player if target is not specified
        if (!target) {
            target = GameObject.FindWithTag("Player").transform.Find("Head");
        }

        // Set some variables
        rotateX = transform.Find("RotateX");
        rotateY = rotateX.Find("RotateY");
        rotatePointX = rotateX.Find("RotatePoint");
        lastFireTime = -fireDelay;
        bulletTemplate.gameObject.SetActive(false);
        lastAngleY = rotateY.eulerAngles.y;
        lastAngleX = rotateX.eulerAngles.x;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (!target || currentHealth == 0) {
            return;
        }

        // Rotate towards player
        Vector3 direction = target.position - firePoint.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // float targetAngleY = Mathf.LerpAngle(lastAngleY, lookRotation.eulerAngles.y, rotateLerp);
        float targetAngleX = lookRotation.eulerAngles.x;
        // Debug.Log(targetAngleX);
        // rotateX.Rotate(targetAngleX - lastAngleX, 0f, 0f, Space.World);
        rotateX.RotateAround(rotatePointX.position, rotateX.right, targetAngleX - lastAngleX);
        lastAngleX = targetAngleX;

        float targetAngleY = lookRotation.eulerAngles.y + 180f;
        rotateY.Rotate(0f, targetAngleY - lastAngleY, 0f, Space.Self);
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
            bulletScript.enabled = true;

            // Activate bullet clone
            bulletClone.gameObject.SetActive(true);
        }
    }
}
