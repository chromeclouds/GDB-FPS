using UnityEngine;
using UnityEngine.UI;

public class buttonEnabledSelect : MonoBehaviour
{
    [SerializeField] Button selected;
    private void OnEnable()
    {
        selected.Select();
    }
}
