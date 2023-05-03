using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UniRx;

public class IapModel : MonoBehaviour
{
    private const string ITEM1 = "com.kihonsyugisya.moetalk.consumable.item1";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IntReactiveProperty amount;

    public void OnPurchaseComplete(Product product)
    {
        if (product.definition.id == ITEM1)
        {
            amount.SetValueAndForceNotify(5);
        }
    }

    public void OnPurchaseField(Product product,PurchaseFailureReason failureReason)
    {
        Debug.Log(product.definition.id + ": failureReason " + failureReason);
    }
}
