using UnityEngine;
using TMPro;

public class HeartTrackerManager : MonoBehaviour
{
    public static HeartTrackerManager Instance;
    public TextMeshProUGUI uiText;

    private int heartsDestroyed = 0;
    public int totalHearts = 3;

    public delegate void OnHeartChange();
    public event OnHeartChange HeartUpdate;

    private void Awake()
    {
        uiText.text = $"{heartsDestroyed}/{totalHearts}";
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterHeartDestroyed(int index)
    {
        heartsDestroyed++;
        uiText.text = $"{heartsDestroyed}/{totalHearts}";
        HeartUpdate?.Invoke();
    }

    public int GetDestroyedCount() => heartsDestroyed;
    public bool AllHeartsDestroyed() => heartsDestroyed >= totalHearts;

}
