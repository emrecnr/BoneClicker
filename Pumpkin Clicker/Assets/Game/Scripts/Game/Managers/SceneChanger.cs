using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private void Start() 
    {
        StartCoroutine(ChangeScene());
    }

    private IEnumerator ChangeScene()
    {
        yield return new WaitUntil(()=> DataManager.Instance.IsDataLoaded);

        yield return SceneManager.LoadSceneAsync(1).isDone;
    }
}
