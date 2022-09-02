using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : Weapon
{
    public Transform shurikenTransform;
    public float shurikenLifeTime = 3f;
    public float shurikenSpeed = 1f;
    private Transform cam;
    [SerializeField] private float raycastDistance = 5f;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    protected override void ChangeAvailableJumps(int newAvailableJumps)
    {
        base.ChangeAvailableJumps(newAvailableJumps);
    }

    // Throw a shuriken and add to the list
    private void ThrowShuriken() {
        Transform newShuriken = Instantiate(shurikenTransform, shurikenTransform.position, Quaternion.LookRotation(cam.forward, cam.up));
        ThrownShuriken script = newShuriken.GetComponent<ThrownShuriken>();
        script.lifeTime = shurikenLifeTime;
        script.enemyLayer = enemyLayer;
        script.speed = shurikenSpeed;
        script.raycastDistance = raycastDistance;
        script.enabled = true;
    }

    protected override void Fire()
    {
        ThrowShuriken();
        lastAttackTime = Time.time;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
