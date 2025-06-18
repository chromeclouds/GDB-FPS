using UnityEngine;

public class WeaponCrate : MonoBehaviour
{
    public Transform itemHolder;
    private GameObject currentItem;

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

    public void PlaceItem(GameObject itemPrefab)
    {
        if (currentItem != null) Destroy(currentItem);

        currentItem = Instantiate(itemPrefab, itemHolder);
        currentItem.transform.localPosition = Vector3.zero;

        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;

        Collider col = currentItem.GetComponent<Collider>();
        if (col) col.isTrigger = true;

        CrateItem crateItem = currentItem.AddComponent<CrateItem>();
        crateItem.originCrate = this;
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
