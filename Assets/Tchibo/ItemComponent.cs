using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ItemComponent : MonoBehaviour
{
    public Image image;
    public Text title, description;
    public string mobileURL = "https://www.tchibo.de/";

    public void GoToStoreURL()
    {
        Application.OpenURL(mobileURL);
    }
    
    public void AddToCart()
    {
        FindObjectOfType<TchiboDataManager>().AddToCart();
    }
}

