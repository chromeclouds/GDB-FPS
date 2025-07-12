using UnityEngine;

[CreateAssetMenu(menuName = "Secrets/HeartSecretData")]
public class HeartSecretData : ScriptableObject
{
    public int mapID;
    public bool[] heartsDestroyed = new bool[3];
}