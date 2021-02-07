using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TchiboDataManager : MonoBehaviour
{
    public RectTransform addRect;
    public ItemComponent itemPrefab;
    public Text cartText;

    static TchiboDataManager data;

    public void Start()
    {
        if (data != null) Destroy(gameObject);
        else data = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Awake()
    {
        OnLevelWasLoaded(0);
    }

    private void OnLevelWasLoaded(int level)
    {
        Debug.Log(level);
        if(level == 0)
        {
            UnityWebRequest req = new UnityWebRequest("https://api.hackathon.tchibo.com/api/v1/products?per_page=9");
            DownloadHandlerBuffer dH = new DownloadHandlerBuffer();
            req.downloadHandler = dH;

            Debug.Log("main scene loaded");

            StartCoroutine(waitForReq(req));
        }
    }

    IEnumerator waitForReq(UnityWebRequest req)
    {
        yield return new WaitWhile(() => addRect == null);
        yield return req.SendWebRequest();

        if (req.isNetworkError)
        {
            Debug.Log("Error: " + req.error);
        }
        else
        {
            Debug.Log("Received: " + req.downloadHandler.text);
            string defaultReplaced = req.downloadHandler.text.Replace("default", "Default");
            TchiboItem[] items = JsonHelper.FromJson<TchiboItem>(defaultReplaced);

            foreach(TchiboItem item in items)
            {
                ItemComponent cmp = Instantiate(itemPrefab, addRect, false);
                cmp.title.text = item.title;
                cmp.description.text = item.description.Default;
                cmp.mobileURL = item.url.mobile;

                StartCoroutine(getSetImage(item.image.Default, cmp.image));
                // do coroutine
                //item.image.
            }
        }
    }

    IEnumerator getSetImage(string url, Image img)
    {
        UnityWebRequest wreq = UnityWebRequestTexture.GetTexture(url);
        UnityWebRequestAsyncOperation asyncReq = wreq.SendWebRequest();

        yield return asyncReq;

        if (img == null) yield break;

        if (wreq.result == UnityWebRequest.Result.ConnectionError || wreq.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error with downloading: " + wreq.url);
        }
        else
        {
            DownloadHandlerTexture downloadHandlerTexture = wreq.downloadHandler as DownloadHandlerTexture;
            Texture2D texture = downloadHandlerTexture.texture;
            Sprite spr = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));
            img.sprite = spr;
            img.enabled = true;
            Debug.Log("web requestoo: " + wreq.url);
        }
    }

    public int cartCount;
    public void AddToCart()
    {
        cartCount += 1;
        cartText.text = cartCount == 1 ?
            "You have 1 item in your cart." :
            $"You have {cartCount} items in your cart.";
    }
}

[Serializable]
public class TchiboItem
{
    public string title;
    public Desc description;
    public URLWrapper url;
    public Imgwrapper image;


    [Serializable]
    public class Imgwrapper
    {
        public string Default;
    }

    [Serializable]
    public class Desc
    {
        public string structured;
        public string Default;
    }

    [Serializable]
    public class URLWrapper
    {
        public string mobile;
        public string Default;
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.data;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.data = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.data = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] data;
    }
}