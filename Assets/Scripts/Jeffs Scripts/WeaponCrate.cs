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
            GameObject spawned = Instantiate(defaultItemToSpawn);
            PlaceItem(spawned);
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
        {
            Destroy(currentItem);
        }

        currentItem = item;

        //reparent to crates itemHolder
        currentItem.transform.SetParent(itemHolder);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.transform.localRotation = Quaternion.identity;

        //disable physics while hovering
        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Collider col = currentItem.GetComponent<Collider>();
        if (col != null) col.isTrigger = true;

        //ensure CrateItem script exists and is linked
        CrateItem crateItem = currentItem.GetComponent<CrateItem>();
        if (crateItem == null)
        {
            crateItem = currentItem.AddComponent<CrateItem>();
        }
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
            currentItem.transform.SetParent(null);
            currentItem = null;
        }
    }
    public GameObject GetCurrentItem()
    {
        return currentItem;
    }

    public GameObject GetcurrentItem() => currentItem;
}
