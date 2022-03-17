using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine.SceneManagement;


[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]

public class SimpleShoot : MonoBehaviour
{
    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;


    public AudioSource source;
    public AudioClip gunshotSound;
    public AudioClip holdSound;
    public AudioClip reloadSound;
    public AudioClip noAmmo;
    public AudioClip slideSound;

    public Magazine magazine;
    private bool slided = false;
    private int shotCount;
    public XRBaseInteractor socketInteractor;
    

    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();

        socketInteractor.onSelectEntered.AddListener(AddMagazine);
        socketInteractor.onSelectExited.AddListener(AddMagazine);
        shotCount = 0;
    }

    void Update()
    {
        //If you want a different input, change it here
        if (Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void AddMagazine(XRBaseInteractable interactable)
    {
        source.PlayOneShot(reloadSound);
        magazine = interactable.GetComponent<Magazine>();
        slided = false;
    }

    public void RemoveMagazine(XRBaseInteractable interactable)
    {
        magazine = null;
    }

    public void slide()
    {
        source.PlayOneShot(slideSound);
        slided = true;
    }
    
    public int getShotCount()
    {
        return shotCount;
    }

    //This function creates the bullet behavior
    void Shoot()
    {
        if (magazine)
        {
            magazine.numberOfBullet--;
            shotCount += 1;
        }
        source.PlayOneShot(gunshotSound);
        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        // Create a bullet and add force on it in direction of the barrel
        Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);
    }

    public void HoldSound()
    {
        source.PlayOneShot(holdSound);
    }

    public void OnTrigger()
    {
        if (magazine && magazine.numberOfBullet > 0 && slided)
        {
            gunAnimator.SetTrigger("Fire");
        }
        else
        {
            source.PlayOneShot(noAmmo);
        }
    }

    public int getbulletCount()
    {
        if (magazine)
        {
            return magazine.numberOfBullet;
        }
        else
            return 0;
    }
    //This function creates a casing at the ejection slot
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }

}
