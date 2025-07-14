using UnityEngine;

public class MeleePickupTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var controller = other.GetComponent<unifiedPlayerController>();
        if (controller != null && TryGetComponent(out MeleeWeaponWorld mw))
        {
            controller.PickupMeleeWeapon(mw.weaponData);
            Destroy(gameObject);
        }
    }
}
