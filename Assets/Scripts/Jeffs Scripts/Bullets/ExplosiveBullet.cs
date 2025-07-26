using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class ExplosiveBullet : MonoBehaviour
{
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private float explosionVolume = 1f;

    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public int damage = 100; //now gets damage from weaponfire then weapondata
    public GameObject explosionEffectPrefab;
    public LayerMask scatterMask; //assign to groundscatter or default w/e you want to move

    private bool hasExploded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //explode incase it doesnt hit anything
        Invoke(nameof(Explode), 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasExploded) return;
        hasExploded = true; //should prevent multi hit per frame
        Explode();
    }

    void Explode()
    {
        if(explosionEffectPrefab)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

        }
        if(explosionSound != null)
        {
            //AudioSource.PlayClipAtPoint(explosionSound, transform.position, explosionVolume);

            GameObject gameObject = new GameObject("One shot audio");
            gameObject.transform.position = transform.position;
            AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
            audioSource.outputAudioMixerGroup = gameManager.instance.mixerSFX;
            audioSource.clip = explosionSound;
            audioSource.spatialBlend = 1f;
            audioSource.volume = explosionVolume;
            audioSource.Play();
            Object.Destroy(gameObject, explosionSound.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider col in colliders)
        {
            if(((1 << col.gameObject.layer)& scatterMask) != 0)
            {
                Rigidbody rb = col.attachedRigidbody;
                if (rb != null && !rb.isKinematic)
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);

                }
            }
            IDamage dmg = col.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(damage);
            }
        }
        Destroy(gameObject);
    }

    
}
