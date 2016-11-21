using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

    public Transform weaponHolder;
    public Weapon startingWeapon;
    Weapon equippedWeapon;

    void Start()
    {

        EquipWeapon(startingWeapon);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            MainTriggerHold();
        }
        if (Input.GetMouseButtonUp(0))
        {
            MainTriggerRelease();
        }
        if (Input.GetMouseButton(1))
        {
            SecondaryTriggerHold();
        }
        if (Input.GetMouseButtonUp(1))
        {
            SecondaryTriggerRelease();
        }
    }

    public void EquipWeapon(Weapon weapon)
    {
        if (equippedWeapon != null)
        {
            Destroy(equippedWeapon.gameObject);
        }
        equippedWeapon = Instantiate(weapon, weaponHolder.position, weaponHolder.rotation) as Weapon;
        equippedWeapon.transform.parent = weaponHolder;
    }

    public void MainTriggerHold()
    {
        if (equippedWeapon != null)
        {
            equippedWeapon.MainTriggerHold();
        }
    }

    public void MainTriggerRelease()
    {
        if (equippedWeapon != null)
        {
            equippedWeapon.MainTriggerRelease();
        }
    }

    public void SecondaryTriggerHold()
    {
        if (equippedWeapon != null)
        {
            equippedWeapon.SecondaryTriggerHold();
        }
    }

    public void SecondaryTriggerRelease()
    {
        if (equippedWeapon != null)
        {
            equippedWeapon.SecondaryTriggerRelease();
        }
    }
}
