using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class Pistol : MonoBehaviour

{
    [SerializeField]
    private GameObject bullet;

    // Gun stats
    [SerializeField]
    private float bulletForce;
    [SerializeField]
    private float shootingCooldown, spread, reloadTime;
    [SerializeField]
    private float bulletRange;
    [SerializeField]
    private int magazineSize;
    private int bulletsLeft;

    // Recoil
    [SerializeField]
    private Rigidbody playerRb;
    [SerializeField]
    private float recoilForce;

    // Camera recoil settings
    [SerializeField]
    private float verticalRecoilAmount; 
    [SerializeField]
    private float horizontalRecoilAmount;

    // Indicator variables
    private bool currentlyShooting, shootingEnabled, currentlyReloading;
    private bool invokeEnabled = true;

    // References
    [SerializeField]
    private Camera Camera;

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    private CinemachinePOV povComponent;

    [SerializeField]
    private Transform gunPoint;

    // Ammo UI
    [SerializeField]
    private TextMeshProUGUI ammoDisplay;

    [SerializeField]
    private ParticleSystem muzzleFlash;

    // Graphics
    // public GameObject muzzleFlash;
    // public TextMeshProUGUI ammunitionDisplay;

    private void Awake()
    {
        // Fill up magazine
        bulletsLeft = magazineSize;
        shootingEnabled = true;

        // Get the POV component from the virtual camera
        povComponent = virtualCamera.GetCinemachineComponent<CinemachinePOV>();

    }

    public void HandleInput()
    {
        // Check for shooting (mouse clicks)
        // Shoots per click, no holding down the key to auto shoot
        currentlyShooting = Input.GetKeyDown(KeyCode.Mouse0);

        // Check for attempt to reload (R clicks)
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !currentlyReloading) Reload();

        // Auto Reload (Attempted shooting but no bullets left)
        // if (shootingEnabled && currentlyShooting && !currentlyReloading && bulletsLeft <= 0) Reload();

        // Shooting (Attempted shooting with bullets left)
        if (shootingEnabled && currentlyShooting && !currentlyReloading && bulletsLeft > 0)
        {
            Shoot();
            Debug.Log("Shooting pistol");
        }
    }

    private void Shoot()
    {
        // Prevent firing again until shot cooldown expires
        shootingEnabled = false;


        // Find shooting position using a ray cast to the center of the screen
        Ray ray = Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        // Initialise hit variable to store hit information later on
        RaycastHit hit;

        // Deploy ray for as long as bulletRange allows and check if it hits any colliders
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, bulletRange))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(bulletRange);

        // Calculate direction from gunPoint to the target
        Vector3 directionWithoutSpread = targetPoint - gunPoint.position;

        // play muzzle flash
        muzzleFlash.transform.forward = directionWithoutSpread.normalized;
        muzzleFlash.transform.position = gunPoint.position;
        muzzleFlash.Play();

        //Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

         //Calculate new direction, considering spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        //Instantiate the bullet
        GameObject currentBullet = Instantiate(bullet, gunPoint.position, Quaternion.identity);

        //Rotate bullet to correct shooting direction according to direction calculated earlier
        currentBullet.transform.forward = directionWithSpread.normalized;

        //Give bullet force
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * bulletForce, ForceMode.Impulse);

        // Deduct ammo accordingly
        bulletsLeft--;

        // Update ammo display after shooting
        UpdateAmmoDisplay();

        // Apply camera recoil
        ApplyCameraRecoil();

        if (invokeEnabled)
        {
            // After a certain delay/cooldown, allow shooting again
            Invoke("AllowShootAgain", shootingCooldown);

            // Indicator to avoid the delay/cooldown from stacking before being fulfilled
            invokeEnabled = false;

            //Apply recoil to player
            playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
        }
        // Destroy the projectile after it has traveled its range
        Destroy(currentBullet, bulletRange / bulletForce);
    }

    private void AllowShootAgain()
    {
        // Allows shooting again
        shootingEnabled = true;
        invokeEnabled = true;
    }

    private void Reload()
    {
        // After a reload delay/cooldown, fill gun magazine again
        currentlyReloading = true;
        ammoDisplay.text = "Reloading";
        Invoke("ReloadComplete", reloadTime);
    }
    private void ReloadComplete()
    {
        //Fill magazine
        bulletsLeft = magazineSize;
        currentlyReloading = false;

        // Update ammo display after reloading
        UpdateAmmoDisplay();
    }

    public bool IsBusy()
    {
        return currentlyReloading || !shootingEnabled;
    }

    public void ResetState()
    {
        currentlyShooting = false;
        currentlyReloading = false;
        shootingEnabled = true;
        invokeEnabled = true;

        // Cancel any scheduled invokes (cool downs or reloads)
        CancelInvoke();
    }

    private void ApplyCameraRecoil()
    {
        // Calculate vertical and horizontal recoil
        float verticalRecoil = verticalRecoilAmount;
        float horizontalRecoil = Random.Range(-horizontalRecoilAmount, horizontalRecoilAmount);
        
         // Adjust the POV component's vertical and horizontal axis values
        povComponent.m_VerticalAxis.Value -= verticalRecoil; 
        povComponent.m_HorizontalAxis.Value += horizontalRecoil;

        Debug.Log("Recoil applied");
    }

    public void UpdateAmmoDisplay()
    {
        // Update ammo count on the UI
        if (ammoDisplay != null)
        {
            ammoDisplay.text = $"{bulletsLeft}/{magazineSize}";

            // Check if ammo is low (you can adjust the threshold as needed)
            if (bulletsLeft <= magazineSize * 0.2f) // If 20% or less ammo left
            {
                ammoDisplay.color = Color.red; // Set color to red when ammo is low
            }
            else
            {
                // Set color to light grey (you can adjust the RGB values for different shades)
                ammoDisplay.color = new Color(0.75f, 0.75f, 0.75f); // Light grey
            }
        }
        
    }
}

