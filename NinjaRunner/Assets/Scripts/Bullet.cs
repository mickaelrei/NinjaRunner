using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform cosmeticTransform;
    public Vector3 direction;
    public float speed;
    public float damage;
    public LayerMask hitLayerMask;
    public int numCosmetics;
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

        for (int i = 0; i < numCosmetics; i++) {
            Transform cosmetic = Instantiate(cosmeticTransform);
            cosmetic.parent = transform;
            BulletCosmetic bulletCosmetic = cosmetic.GetComponent<BulletCosmetic>();
            bulletCosmetic.offset = i * 1.15f;
            bulletCosmetic.bullet = transform;
        }
    }

    void FixedUpdate()
    {
        // Check if lifetime has passed
        if (Time.time > startTime + lifetime) {
            Destroy(gameObject);
            return;
        }

        // Check for collisions
        Collider[] colliders = Physics.OverlapSphere(transform.position, .1f, hitLayerMask);
        foreach (Collider coll in colliders) {
            // Check for player hit
            if (coll.gameObject.tag == "Player") {
                Player player = coll.gameObject.GetComponent<Player>();
                // Check for damage
                if (!hitPlayers.Contains(player)) {
                    hitPlayers.Add(player);
                    if (player.GetHealth() > 0) {
                        // Debug.Log("Hit player on object " + coll.gameObject.name);
                        player.TakeDamage(damage);
                    }
                }
            }
        }

        // Move bullet
        transform.position += direction * speed * Time.fixedDeltaTime;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, .1f);
        // Gizmos.DrawCube(transform.position, Vector3.one * 3f);
    }
}
