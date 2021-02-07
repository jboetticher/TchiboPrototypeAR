using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class RefreshCartItems : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TchiboDataManager m = FindObjectOfType<TchiboDataManager>();
        m.cartText = GetComponent<Text>();

        m.cartText.text = m.cartCount == 1 ?
            "You have 1 item in your cart." :
            $"You have {m.cartCount} items in your cart.";
    }
}
