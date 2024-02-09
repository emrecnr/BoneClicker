using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftButtonShaker : MonoBehaviour
{
    public Button buttonToShake;
    public float shakeDuration = 0.3f;
    public float shakeIntensity = 0.3f;
    public float shakeInterval = 2;

    private float timer = 0f;

    private void OnEnable()
    {
        buttonToShake.onClick.AddListener(()=> GiftManager.Instance.OpenPanel());

    }

    private void OnDisable()
    {
        buttonToShake.onClick.RemoveAllListeners();
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Belirlenen aralıkta titreme işlemini gerçekleştir
        if (timer >= shakeInterval)
        {
            ShakeButton();
            timer = 0f; // Zamanlayıcıyı sıfırla
        }
    }

    void ShakeButton()
    {
        LeanTween.scale(buttonToShake.gameObject, Vector3.one * (1 + shakeIntensity), shakeDuration / 2f)
                    .setEase(LeanTweenType.easeInOutQuad)
                    .setLoopPingPong(1);
    }
    
}
