using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public Player player;
    public Transform spawnPoint;

    public void OnPlay() {
        // Disable canvas
        GetComponent<Canvas>().enabled = false;

        // Change cursor lock state
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        // Tell player to start moving
        player.transform.position = spawnPoint.transform.position;
        player.SendMessage("OnPlay");
    }
}
