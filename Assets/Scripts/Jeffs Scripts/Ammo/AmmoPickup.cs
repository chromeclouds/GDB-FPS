using UnityEngine;

public class AmmoPickup : MonoBehaviour
{

    [SerializeField] private AmmoType ammoType;
    [SerializeField] private int amount = 30;
    private CrateItem crateOrigin;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AmmoManager ammoManager = other.GetComponent<AmmoManager>();
            if (ammoManager != null)
            {
                ammoManager.AddAmmo(ammoType, amount);

                if (crateOrigin != null && crateOrigin.originCrate != null)
                {
                    crateOrigin.originCrate.ClearItem();
                }

                Destroy(gameObject);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        crateOrigin = GetComponent<CrateItem>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
