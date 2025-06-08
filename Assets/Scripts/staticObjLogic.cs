using UnityEngine;
using System.Collections;

public class staticObjLogic : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] int layer;
    [SerializeField] int HP;
    Color colorOrig;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;

        changeTransparency();
        changeLayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void takeDamage(int amount)
    {
        HP -= amount;

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(flashRed());
        }
    }
    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }
    void changeTransparency()
    {
        // Stores wall color and sets the transparency to half
        // The material color must use transparent renduring
        Color temp = colorOrig;
        temp.a = 0.5f;
        model.material.color = temp;
    }
    void changeLayer()
    {
        gameObject.layer = layer;
    }
}
