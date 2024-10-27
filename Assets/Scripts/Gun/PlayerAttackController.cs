using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum WeaponType { Nothing, Pistol, Shotgun, Melee }

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

    // Sound references
    [SerializeField]
    private PistolAudio pistolAudio;
    [SerializeField]
    private ShotgunAudio shotgunAudio;
    [SerializeField]
    private MeleeAudio meleeAudio;

    // Current active weapon
    private WeaponType currentWeapon;

    // Dictionary to keep track of slot status (locked/unlocked)
    private Dictionary<WeaponType, bool> weaponSlots;

    // Switching delay
    [SerializeField] private float switchDelay = 0.5f; // Time in seconds
    private bool canSwitchWeapon = true;

    // Ammo UI
    [SerializeField]
    private TextMeshProUGUI ammoDisplay;

    private void Awake()
    {
        // Get references to weapon scripts
        pistol = GetComponent<Pistol>();
        shotgun = GetComponent<Shotgun>();
        melee = GetComponent<Melee>();

        ammoDisplay.text = "";

        // Initialise slots as locked
        weaponSlots = new Dictionary<WeaponType, bool>()
        {
            { WeaponType.Nothing, true },
            { WeaponType.Melee, true },
            { WeaponType.Pistol, true },
            { WeaponType.Shotgun, true }
        };

        // Start with Melee as default
        SwitchWeapon(WeaponType.Nothing);

    }

    private void Update()
    {
        // Handle input for switching weapons
        if (canSwitchWeapon && !IsCurrentWeaponBusy())
        {
            if (Input.GetKey(KeyCode.Alpha1) && currentWeapon != WeaponType.Nothing)
                StartCoroutine(SwitchWeaponWithDelay(WeaponType.Nothing));
            else if (Input.GetKey(KeyCode.Alpha2) && currentWeapon != WeaponType.Melee && weaponSlots[WeaponType.Melee])
                StartCoroutine(SwitchWeaponWithDelay(WeaponType.Melee));
            else if (Input.GetKey(KeyCode.Alpha3) && currentWeapon != WeaponType.Pistol && weaponSlots[WeaponType.Pistol])
                StartCoroutine(SwitchWeaponWithDelay(WeaponType.Pistol));
            else if (Input.GetKey(KeyCode.Alpha4) && currentWeapon != WeaponType.Shotgun && weaponSlots[WeaponType.Shotgun])
                StartCoroutine(SwitchWeaponWithDelay(WeaponType.Shotgun));
        }

        // Handle input for attack based on current weapon
        switch (currentWeapon)
        {
            case WeaponType.Nothing:
                // No attack input to handle
                break;

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

        // Set visibility based on slot status
        meleeWeaponAsset.SetActive(!weaponSlots[WeaponType.Melee] || weaponType == WeaponType.Melee);
        pistolWeaponAsset.SetActive(!weaponSlots[WeaponType.Pistol] || weaponType == WeaponType.Pistol);
        shotgunWeaponAsset.SetActive(!weaponSlots[WeaponType.Shotgun] || weaponType == WeaponType.Shotgun);

        // Reset states to ensure no cool downs or reloads are still in process (precaution)
        pistol.ResetState();
        shotgun.ResetState();
        melee.ResetState();

        // Enable the selected weapon script
        currentWeapon = weaponType;
        switch (weaponType)
        {
            case WeaponType.Nothing:
                ammoDisplay.text = "";
                Debug.Log("Switch to nothing");
                break;

            case WeaponType.Pistol:
                pistol.enabled = true;
                pistol.UpdateAmmoDisplay();
                pistolAudio.PlayArmingSound();
                Debug.Log("Switch to pistol");
                break;

            case WeaponType.Shotgun:
                shotgun.enabled = true;
                shotgun.UpdateAmmoDisplay();
                shotgunAudio.PlayArmingSound();
                Debug.Log("Switch to shotgun");
                break;

            case WeaponType.Melee:
                melee.enabled = true;
                melee.UpdateAmmoDisplay();
                meleeAudio.PlayArmingSound();
                Debug.Log("Switch to melee");
                break;
        }
    }

    // Check if the current weapon is busy (reloading or on cool down)
    private bool IsCurrentWeaponBusy() => currentWeapon switch
    {
        WeaponType.Nothing => false,
        WeaponType.Pistol => pistol.IsBusy(),
        WeaponType.Shotgun => shotgun.IsBusy(),
        WeaponType.Melee => melee.IsBusy(),
        _ => false,
    };

    public bool IsThisCurrentWeapon(WeaponType weapon)
    {
        return weapon == currentWeapon;
    }

    public void OnWeaponPickUp(WeaponType weaponType)
    {
        // Unlock the slot for the picked-up weapon
        weaponSlots[weaponType] = true;

        // Switch to weapon just picked up
        StartCoroutine(SwitchWeaponWithDelay(weaponType));
    }

    public void OnWeaponDrop(WeaponType weaponType)
    {
        // Lock the slot for the dropped weapon
        weaponSlots[weaponType] = false;

        // Switch back to empty hand
        StartCoroutine(SwitchWeaponWithDelay(WeaponType.Nothing));
        
    }

}