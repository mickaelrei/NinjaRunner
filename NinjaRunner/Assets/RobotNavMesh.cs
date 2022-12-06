using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotNavMesh : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (!player) {
            player = GameObject.FindWithTag("Player").transform;
        }

    }

    private void Update() {
        // Set destination
        navMeshAgent.destination = player.position;

        // Change animations
        HandleAnimations();
    }

    private void HandleAnimations() {
        Debug.Log(navMeshAgent.velocity);
    }
}
