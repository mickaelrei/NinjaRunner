using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform cube;
    public Vector3 direction;
    public float speed;
    public float damage;
    public LayerMask hitLayerMask;
    public float lifetime = 15f;
    private float startTime;
    private List<Player> hitPlayers;
    private Transform player;
    // private ProBuilderMesh mesh;
    private float bulletRadius;

    private void Start() {
        startTime = Time.time;
        hitPlayers = new List<Player>();
        player = GameObject.FindWithTag("Player").transform;
        // mesh = GetComponent<ProBuilderMesh>();
        bulletRadius = transform.lossyScale.y * .1f;

        // Disable collision between bullet and player
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("Player"));
    }

    void AddCube() {
        Transform cubeClone = Instantiate(cube);
        cubeClone.position = transform.position;
    }

    void FixedUpdate()
    {
        // Check if lifetime has passed
        if (Time.time > startTime + lifetime) {
            Destroy(gameObject);
            return;
        }

        // Check for collisions
        Collider[] colliders = Physics.OverlapSphere(transform.position, bulletRadius, hitLayerMask);
        foreach (Collider coll in colliders) {
            // Check for player hit
            if (coll.gameObject.tag == "Player") {
                Player player = coll.gameObject.GetComponent<Player>();
                // Check for damage
                if (!hitPlayers.Contains(player)) {
                    hitPlayers.Add(player);
                    if (player.GetHealth() > 0) {
                        Debug.Log("Hit player on object " + coll.gameObject.name);
                        AddCube();
                        player.TakeDamage(damage);
                    }
                }
            }
        }

        // Move bullet
        transform.position += direction * speed * Time.fixedDeltaTime;
    }

    private void OnDrawGizmos() {
        // Gizmos.DrawWireSphere(transform.position, 4f);
        Gizmos.DrawCube(transform.position, Vector3.one * 3f);
    }
}
