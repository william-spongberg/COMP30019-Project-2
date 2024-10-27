using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    // Weapon details
    [SerializeField] 
    private WeaponType weapon; 
    [SerializeField] 
    private Rigidbody weaponRb; 
    [SerializeField] 
    private Collider weaponCollider;
    [SerializeField] 
    private KeyCode weaponKey; 
    [SerializeField] 
    private PlayerAttackController playerAttackController;

    // Player references
    [SerializeField] 
    private Transform playerCapsule; 
    [SerializeField] 
    private Transform weaponContainer; 
    [SerializeField] 
    private Transform virtualCamera;

    // Pickup and drop properties
    [SerializeField] 
    private float pickUpRange; 
    [SerializeField] 
    private float dropForwardForce; 
    [SerializeField] 
    private float dropUpwardForce;
    [SerializeField] 
    private bool equipped;

    private void Start()
    {
        if (equipped)
        {
            // Attach weapon to container and reset transform
            transform.SetParent(weaponContainer);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            transform.localScale = Vector3.one;

            // Set Rigidbody/collider for equipped state
            weaponRb.isKinematic = true;
            weaponCollider.isTrigger = true;
        }
        else
        {
            // Detach from container
            transform.SetParent(null);

            // Reset Rigidbody/collider for dropped state
            weaponRb.isKinematic = false;
            weaponCollider.isTrigger = false;
        }
    }

    private void Update()
    {
        Vector3 distanceToPlayer = playerCapsule.position - transform.position;

        // Check if player is in range and presses the appropriate weapon key to pick up the weapon
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(weaponKey))
        {
            PickUp();
        }

        // Drop weapon if equipped and it is the current weapon and "Q" is pressed
        if (equipped && playerAttackController.IsThisCurrentWeapon(weapon) && Input.GetKeyDown(KeyCode.Q))
        {
            Drop();
        }
    }

    private void PickUp()
    {
        equipped = true;

        // Attach weapon to container and reset transform
        transform.SetParent(weaponContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        // Set Rigidbody/collider for equipped state
        weaponRb.isKinematic = true;
        weaponCollider.isTrigger = true;

        // Notify PlayerAttackController of pickup
        playerAttackController.OnWeaponPickUp(weapon);
    }

    public void Drop()
    {
        equipped = false;

        // Detach from container
        transform.SetParent(null);

        // Reset Rigidbody/collider for dropped state
        weaponRb.isKinematic = false;
        weaponCollider.isTrigger = false;

        // Apply force to simulate a drop
        weaponRb.velocity = playerCapsule.GetComponent<Rigidbody>().velocity;
        weaponRb.AddForce(virtualCamera.forward * dropForwardForce, ForceMode.Impulse);
        weaponRb.AddForce(virtualCamera.up * dropUpwardForce, ForceMode.Impulse);

        // Add random rotation for realism
        float random = Random.Range(-1f, 1f);
        weaponRb.AddTorque(new Vector3(random, random, random) * 10);

        // Notify PlayerAttackController of drop
        playerAttackController.OnWeaponDrop(weapon);
    }
}
