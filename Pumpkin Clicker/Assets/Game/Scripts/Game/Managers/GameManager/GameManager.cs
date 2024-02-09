using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void OnApplicationPause(bool pauseStatus)
    {
        if(pauseStatus)
        {
            Debug.Log("Game is stopped.");
        }
    }
}
