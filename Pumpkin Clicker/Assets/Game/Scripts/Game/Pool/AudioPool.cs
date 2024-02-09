using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPool : GenericPool<AudioSource>
{
    public static AudioPool Instance {get; private set;}

    protected override void SingletonObject()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
