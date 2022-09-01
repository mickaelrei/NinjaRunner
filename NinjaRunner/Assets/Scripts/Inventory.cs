using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Transform> weaponsList = new List<Transform>();
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = (int) KeyCode.Alpha0 ; i <= (int) KeyCode.Alpha9; i++)
        {
            if (Input.GetKeyDown( (KeyCode) i)) {
                // Debug.Log("Pressed number " + (i - (int) KeyCode.Alpha0).ToString());
                EquipWeapon(i - (int) KeyCode.Alpha0);
            }
        }
    }

    void EquipWeapon(int weaponIndex) {
        
    }
}
