using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MinigunAudioController : MonoBehaviour
{
    private WeaponFire weaponFire;
    private AudioSource audioSource;

    private bool isPlayerHoldingFire = false;
    private bool wasPlayingLastFrame = false;

    void Start()
    {
        weaponFire = GetComponent<WeaponFire>();
        audioSource = GetComponent<AudioSource>();

        if (weaponFire == null || audioSource == null)
        {
            enabled = false;
            return;
        }

        audioSource.loop = true;
        audioSource.clip = weaponFire.weaponData.FireSound;
    }

    void Update()
    {
        if (weaponFire.weaponData.FireMode == FireMode.FullAuto && Input.GetButton("Fire1") && weaponFire.CurrentAmmo > 0)
        {
            isPlayerHoldingFire = true;
        }
        else
        {
            isPlayerHoldingFire = false;
        }

        if (isPlayerHoldingFire && !wasPlayingLastFrame)
        {
            audioSource.Play();
            wasPlayingLastFrame = true;
        }
        else if (!isPlayerHoldingFire && wasPlayingLastFrame)
        {
            audioSource.Stop();
            wasPlayingLastFrame = false;
        }
    }
}
