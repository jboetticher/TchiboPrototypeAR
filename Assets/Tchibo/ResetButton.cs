using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    public void ResetScenes()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetHard()
    {
        Destroy(FindObjectOfType<TchiboDataManager>());

        SceneManager.LoadScene(0);
    }
}
