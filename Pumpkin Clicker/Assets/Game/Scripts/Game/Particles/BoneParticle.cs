using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoneParticle : MonoBehaviour
{

    [Header(" ## Elements ## ")]
    [SerializeField] private TMP_Text bonusText;
    
    private void Start()
    {
        
    }

    public void Configure(float coinMultiplier)
    {
        bonusText.text = "+" + coinMultiplier;
    }

    public void Destroy()
    {
        BoneParticlePool.Instance.Set(this);
    }
}
