using UnityEngine;
using System.Collections;

public class staticObjLogic : MonoBehaviour, IDamage, ICost
{
    [SerializeField] Renderer model;
    [SerializeField] int layer;
    [SerializeField] int HP;
    [SerializeField] int price;
    int layerOrig;
    Color colorOrig;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        layerOrig = gameObject.layer;
        changeLayer();
        changeTransparency();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void takeDamage(int amount)
    {
        if(model.tag == "Bought")
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
        // Changes the objects layer to the layer chosen by the designer
        // Can make the object solid or walk through so long as the layer settings are set to not interact
        gameObject.layer = layer;
    }
    public void buy()
    {
        if (gameManager.instance.walletAmount() - price >= 0)
        {
            gameObject.layer = layerOrig;
            model.material.color = colorOrig;
            gameManager.instance.reduceWallet(price);
            model.tag = "Bought";
        }
    }
}
