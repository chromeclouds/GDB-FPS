using UnityEngine;

public class torchStats : MonoBehaviour
{
    [SerializeField] bool isHardModeTorch;
    string name;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(isHardModeTorch)
        {
            name = "Hard mode torch";
        }
        else
        {
            name = "Easy mode torch";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetName()
    {
        return name;
    }
}
