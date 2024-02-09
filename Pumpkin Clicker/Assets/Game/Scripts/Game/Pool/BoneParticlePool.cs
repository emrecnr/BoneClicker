using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneParticlePool : GenericPool<BoneParticle>
{
    public static BoneParticlePool Instance;

    protected override void SingletonObject()
    {
        if (Instance == null)
            Instance = this;

        else Destroy(this.gameObject);
    }


}
