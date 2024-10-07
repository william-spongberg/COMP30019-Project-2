using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Shotgun : MonoBehaviour
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

    [SerializeField]
    private int pelletCount = 10; // Number of pellets per shotgun blast

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
        if (shootingEnabled && currentlyShooting && !currentlyReloading && bulletsLeft <= 0) Reload();

        // Shooting (Attempted shooting with bullets left)
        if (shootingEnabled && currentlyShooting && !currentlyReloading && bulletsLeft > 0)
        {
            Shoot();
            Debug.Log("Shooting shotgun");
        }
    }

    private void Shoot()
    {
        // Prevent firing again until shot cooldown expires
        shootingEnabled = false;

        // Find shooting position using a ray cast to the center of the screen
        Ray ray = Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Deploy ray for as long as bulletRange allows and check if it hits any colliders
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, bulletRange))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(bulletRange);

        // Calculate direction from gunPoint to the target
        Vector3 directionWithoutSpread = targetPoint - gunPoint.position;

        // Fire multiple pellets with spread
        for (int i = 0; i < pelletCount; i++)
        {
            // Calculate random spread for each pellet
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);

            // Calculate new direction, considering spread
            Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

            // Instantiate the bullet
            GameObject currentPellet = Instantiate(bullet, gunPoint.position, Quaternion.identity);

            // Rotate bullet to correct shooting direction according to direction calculated earlier
            currentPellet.transform.forward = directionWithSpread.normalized;

            // Give bullet force
            currentPellet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * bulletForce, ForceMode.Impulse);

            // Destroy the pellet after it has traveled its range
            Destroy(currentPellet, bulletRange / bulletForce);
        }

        // Deduct one ammo per shotgun blast
        bulletsLeft--;

        // Apply camera recoil
        ApplyCameraRecoil();

        if (invokeEnabled)
        {
            // After a certain delay/cooldown, allow shooting again
            Invoke("AllowShootAgain", shootingCooldown);

            // Indicator to avoid the delay/cooldown from stacking before being fulfilled
            invokeEnabled = false;

            // Apply recoil to player
            playerRb.AddForce(-directionWithoutSpread.normalized * recoilForce, ForceMode.Impulse);
        }
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
        Invoke("ReloadComplete", reloadTime);
    }

    private void ReloadComplete()
    {
        // Fill magazine
        bulletsLeft = magazineSize;
        currentlyReloading = false;
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

        // Cancel any scheduled invokes (cooldowns or reloads)
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
}


