using UnityEngine;

public class PusherTrap : MonoBehaviour, ITrapToggle
{
    public Transform pusher;
    public Vector3 pushDirection = Vector3.forward;
    public float pushDistance = 3f;
    public float pushSpeed = 4f;
    private Vector3 originalPos;
    private bool isActive = true;

    private void Awake()
    {
        originalPos = pusher.localPosition;

    }

    public void SetTrapActive(bool active)
    {
        isActive = active;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;
        StartCoroutine(PushRoutine());
    }

    private System.Collections.IEnumerator PushRoutine()
    {
        Vector3 target = originalPos + pushDirection.normalized * pushDistance;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * pushSpeed;
            pusher.localPosition = Vector3.Lerp(originalPos, target, t);
            yield return null;

        }
        yield return new WaitForSeconds(0.5f);

        t = 0f;
        while(t < 1f)
        {
            t+= Time.deltaTime * pushSpeed;
            pusher.localPosition = Vector3.Lerp(target, originalPos, t);
            yield return null;
        }
    }
}
