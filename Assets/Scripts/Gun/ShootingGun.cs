using UnityEngine;

public class ShootingGun : MonoBehaviour
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

    int bulletsLeft;

    // Recoil
    [SerializeField]
    private Rigidbody playerRb;
    [SerializeField]
    private float recoilForce;

    // Indicator variables
    private bool currentlyShooting, shootingEnabled, currentlyReloading;
    private bool invokeEnabled = true;

    // References
    [SerializeField]
    private Camera Camera;
    [SerializeField]
    private Transform gunPoint;

    // Audio
    [SerializeField]
    private GameObject ShootingAudio; // Reference to the GameObject with AudioSource
    private AudioSource shootingAudioSource; // Reference to the AudioSource component

    private void Awake()
    {
        // Fill up magazine
        bulletsLeft = magazineSize;
        shootingEnabled = true;

        // Initialize the audio source and make sure it doesn't play on start
        shootingAudioSource = ShootingAudio.GetComponent<AudioSource>();
        shootingAudioSource.playOnAwake = false; // Ensure it doesn't play automatically
        shootingAudioSource.Stop(); // Make sure it's stopped at the beginning
    }

    private void Update()
    {
        PlayerInput();
    }

    private void PlayerInput()
    {
        // Check for shooting (mouse clicks)
        currentlyShooting = Input.GetKeyDown(KeyCode.Mouse0);

        // Check for attempt to reload (R clicks)
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !currentlyReloading) Reload();

        // Auto Reload (Attempted shooting but no bullets left)
        if (shootingEnabled && currentlyShooting && !currentlyReloading && bulletsLeft <= 0) Reload();

        // Shooting (Attempted shooting with bullets left)
        if (shootingEnabled && currentlyShooting && !currentlyReloading && bulletsLeft > 0)
        {
            Shoot();
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

        // Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // Calculate new direction, considering spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        // Instantiate the bullet
        GameObject currentBullet = Instantiate(bullet, gunPoint.position, Quaternion.identity);

        // Rotate bullet to correct shooting direction according to direction calculated earlier
        currentBullet.transform.forward = directionWithSpread.normalized;

        // Give bullet force
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * bulletForce, ForceMode.Impulse);

        // Deduct ammo accordingly
        bulletsLeft--;

        // Play shooting audio
        PlayShootingAudio();

        if (invokeEnabled)
        {
            // After a certain delay/cooldown, allow shooting again
            Invoke("AllowShootAgain", shootingCooldown);

            // Indicator to avoid the delay/cooldown from stacking before being fulfilled
            invokeEnabled = false;

            // Apply recoil to player
            playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
        }
        // Destroy the projectile after it has traveled its range
        Destroy(currentBullet, bulletRange / bulletForce);
    }

    private void PlayShootingAudio()
    {
        // Play shooting audio only when firing
        if (!shootingAudioSource.isPlaying)
        {
            shootingAudioSource.Play();  // Start the audio when shooting
        }
    }

    private void AllowShootAgain()
    {
        // Allows shooting again
        shootingEnabled = true;
        invokeEnabled = true;

        // Stop the audio after the cooldown period
        shootingAudioSource.Stop();  // Stop the audio after shooting
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
}
