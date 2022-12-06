using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Player player;
    public Transform spawnPoint;
    public GameObject titleBackground;
    public GameObject playButton;
    public GameObject deathText;
    private Canvas canvas;

    private void Start() {
        canvas = GetComponent<Canvas>();
    }

    public void OnPlay() {
        // Disable canvas
        canvas.enabled = false;

        // Change cursor lock state
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        // Tell player to start moving
        // player.transform.position = spawnPoint.transform.position;
        player.SendMessage("OnPlay");

        // Hide title and play button
        playButton.SetActive(false);
        titleBackground.SetActive(false);
    }

    public void OnDeath() {
        deathText.SetActive(true);

        canvas.enabled = true;
    }
}
