using UnityEngine;


public class WeaponCrate : MonoBehaviour
{
    [SerializeField] private GameObject defaultItemToSpawn;

    public Transform itemHolder;
    private GameObject currentItem;


    private void Start()
    {
        if (defaultItemToSpawn != null)
        {
            PlaceItem(defaultItemToSpawn);
        }
    }

    private void OnDrawGizmos()
    {
        if (itemHolder != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(itemHolder.position, Vector3.one * 0.5f);
        }
    }

    private void Update()
    {
        if (currentItem != null)
        {
            float bob = Mathf.Sin(Time.time * 2f) * 0.05f;
            currentItem.transform.localPosition = Vector3.up * bob;
            currentItem.transform.Rotate(Vector3.up, 30f * Time.deltaTime, Space.World);
        }
    }

    public void PlaceItem(GameObject item)
    {
        if (currentItem != null)
            Destroy(currentItem);

        if (item.scene.IsValid()) // Placing a scene object (e.g. from player drop)
        {
            currentItem = item;
            currentItem.transform.SetParent(itemHolder);
            currentItem.transform.localPosition = Vector3.zero;
        }
        else // item is a prefab asset, must instantiate
        {
            currentItem = Instantiate(item, itemHolder);
            currentItem.transform.localPosition = Vector3.zero;
        }

        // Safe rigidbody/collider handling
        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;

        foreach (Collider col in currentItem.GetComponentsInChildren<Collider>())
            col.isTrigger = true;

        // Attach crate marker if missing
        CrateItem crateItem = currentItem.GetComponent<CrateItem>();
        if (crateItem == null)
            crateItem = currentItem.AddComponent<CrateItem>();

        crateItem.originCrate = this;
    }

    public void ClearItemWithoutDestroy()
    {
        currentItem = null;
    }


    public void ClearItem()
    {
        if (currentItem != null)
        {
            Destroy(currentItem);
            currentItem = null;
        }
    }
    public GameObject GetCurrentItem()
    {
        return currentItem;
    }

    public GameObject GetcurrentItem() => currentItem;
}
