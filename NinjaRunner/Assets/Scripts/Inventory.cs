using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Transform> weaponsList = new List<Transform>();
    public List<Vector3> weaponsPosition = new List<Vector3>();
    public float changeDelay = .5f;
    public Player player;
    public Transform cam;
    private List<Transform> weaponsClones = new List<Transform>();
    private Transform currentEquipped;
    private int equippedIdx = -1;
    private float lastChangeTime;

    // Start is called before the first frame update
    void Start()
    {
        if (!cam) {
            cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
        // Initialize weapons list
        foreach (Transform weapon in weaponsList) {
            Transform clone = Instantiate(weapon);
            clone.gameObject.SetActive(false);
            clone.name = weapon.name;
            clone.parent = cam;
            weaponsClones.Add(clone);
        }

        // Equip first weapon
        lastChangeTime = -changeDelay - 1;
        EquipWeapon(0);
    }

    // Update is called once per frame
    void Update()
    {
        // Check for number inputs (1 to 9 + 0)
        for (int i = (int) KeyCode.Alpha0 ; i <= (int) KeyCode.Alpha9; i++)
        {
            if (Input.GetKeyDown((KeyCode) i)) {
                int keyCodeNumber = i - (int) KeyCode.Alpha0;
                EquipWeapon(HotbarNumberToListIndex(keyCodeNumber));
            }
        }

        // Check for mouse scroll
        if (Input.mouseScrollDelta.y != 0) {
            int newIdx = (int)(equippedIdx - Input.mouseScrollDelta.y);
            if (newIdx >= weaponsList.Count) {
                newIdx = 0;
            } else if (newIdx < 0) {
                newIdx = weaponsList.Count - 1;
            }
            EquipWeapon(newIdx);
        }
    }

    // Method to convert from hotbar keycode to list index
    private int HotbarNumberToListIndex(int number) {
        if (number != 0) {
            return number - 1;
        } else {
            return 9;
        }
    }

    private void EquipWeapon(int weaponIdx) {
        // Check if equipping the same weapon
        if (equippedIdx == weaponIdx) {
            Debug.Log("Tried to equip the same weapon");
            return;
        }

        // Check if changing weapons too fast
        if (Time.time > lastChangeTime + changeDelay) {
            lastChangeTime = Time.time;
        } else {
            Debug.Log("Changing too quick");
            return;
        }

        // Unequip currently equipped
        if (currentEquipped) {
            currentEquipped.gameObject.SetActive(false);
        }

        // Equip new weapon
        equippedIdx = weaponIdx;
        currentEquipped = weaponsClones[equippedIdx];
        currentEquipped.localPosition = weaponsPosition[equippedIdx];
        currentEquipped.gameObject.SetActive(true);

        // Send message to player script to equip the new weapon
        player.EquipWeapon(currentEquipped.GetComponent<Weapon>());
    }
}
