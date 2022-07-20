using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCosmetic : MonoBehaviour
{
    public Transform bullet;
    public float offset = 5f;
    public float speedLat = 5f;
    public float speedLon = 5f;
    public float bulletDistance = .07f;
    private float lat, lon;
    
    // Start is called before the first frame update
    void Start()
    {
        lat = offset;
        lon = offset;
        transform.position = bullet.position;
        GetComponent<TrailRenderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bullet) return;

        Vector3 pos = SphericalToVector3(lat, lon, bulletDistance, Vector3.zero);// + Vector3.down * bulletDistance / 2f;
        transform.position = bullet.position + pos;
        
        lat = (lat + speedLat * Time.deltaTime) % (Mathf.PI * 2f);
        lon = (lon + speedLon * Time.deltaTime) % (Mathf.PI * 1f);
    }

    Vector3 SphericalToVector3(float lat, float lon, float r, Vector3 center) {
        float x = center.x + r * Mathf.Sin(lon) * Mathf.Cos(lat);
        float y = center.y + r * Mathf.Sin(lon) * Mathf.Sin(lat);
        float z = center.z + r * Mathf.Cos(lon);

        return new Vector3(x, y, z);
    }
}

// lat = 0 - 2*pi
// lon = 0 - pi

