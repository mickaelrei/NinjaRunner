using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature
{
    public float damage = 3f;
    private MeshRenderer meshRenderer;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        // meshRenderer = GetComponent<MeshRenderer>();
        // meshRenderer.material.color = Color.HSVToRGB(Random.Range(0f, 1f), 1f, .5f);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
