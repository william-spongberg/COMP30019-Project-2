using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    // References to weapon scripts
    private Pistol pistol;
    private Shotgun shotgun;
    private Melee melee;

    // References to weapon models/assets
    [SerializeField] private GameObject meleeWeaponAsset;
    [SerializeField] private GameObject pistolWeaponAsset;
    [SerializeField] private GameObject shotgunWeaponAsset;

    // Current active weapon
    private enum WeaponType { Pistol, Shotgun, Melee }
    private WeaponType currentWeapon;

    // Switching delay
    [SerializeField] private float switchDelay = 0.5f; // Time in seconds
    private bool canSwitchWeapon = true;

    private void Awake()
    {
        // Get references to weapon scripts
        pistol = GetComponent<Pistol>();
        shotgun = GetComponent<Shotgun>();
        melee = GetComponent<Melee>();

        // Start with Melee as default
        SwitchWeapon(WeaponType.Melee);
    }

    private void Update()
    {
        // Handle input for switching weapons
        if (canSwitchWeapon && !IsCurrentWeaponBusy())
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && currentWeapon != WeaponType.Melee)
                StartCoroutine(SwitchWeaponWithDelay(WeaponType.Melee));
            else if (Input.GetKeyDown(KeyCode.Alpha2) && currentWeapon != WeaponType.Pistol)
                StartCoroutine(SwitchWeaponWithDelay(WeaponType.Pistol));
            else if (Input.GetKeyDown(KeyCode.Alpha3) && currentWeapon != WeaponType.Shotgun)
                StartCoroutine(SwitchWeaponWithDelay(WeaponType.Shotgun));
        }

        // Handle input for attack based on current weapon
        switch (currentWeapon)
        {
            case WeaponType.Pistol:                
                pistol.HandleInput();
                break;

            case WeaponType.Shotgun:
                shotgun.HandleInput();
                break;

            case WeaponType.Melee:
                melee.HandleInput();
                break;
        }
    }

    // Coroutine for switching weapon with delay
    private IEnumerator SwitchWeaponWithDelay(WeaponType weaponType)
    {
        canSwitchWeapon = false;

        // Wait for the switch delay
        yield return new WaitForSeconds(switchDelay);

        // Perform the weapon switch
        SwitchWeapon(weaponType);


        canSwitchWeapon = true;
    }

    private void SwitchWeapon(WeaponType weaponType)
    {
        // Disable all weapon scripts before switching
        pistol.enabled = false;
        shotgun.enabled = false;
        melee.enabled = false;

        // Hide all weapon assets
        meleeWeaponAsset.SetActive(false);
        pistolWeaponAsset.SetActive(false);
        shotgunWeaponAsset.SetActive(false);

        // Reset states to ensure no cool downs or reloads are still in process (precaution)
        pistol.ResetState();
        shotgun.ResetState();
        melee.ResetState();

        // Enable the selected weapon script
        currentWeapon = weaponType;
        switch (weaponType)
        {
            case WeaponType.Pistol:
                pistol.enabled = true;
                pistolWeaponAsset.SetActive(true);
                Debug.Log("Switch to pistol");
                break;

            case WeaponType.Shotgun:
                shotgun.enabled = true;
                shotgunWeaponAsset.SetActive(true);
                Debug.Log("Switch to shotgun");
                break;

            case WeaponType.Melee:
                melee.enabled = true;
                meleeWeaponAsset.SetActive(true);
                Debug.Log("Switch to melee");
                break;
        }
    }

    // Check if the current weapon is busy (reloading or on cool down)
    private bool IsCurrentWeaponBusy()
    {
        switch (currentWeapon)
        {
            case WeaponType.Pistol:
                return pistol.IsBusy();
            case WeaponType.Shotgun:
                return shotgun.IsBusy();
            case WeaponType.Melee:
                return melee.IsBusy();
            default:
                return false;
        }
    }
}
