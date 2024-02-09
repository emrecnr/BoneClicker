using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneParticlesManager : MonoBehaviour
{
    [Header(" ## Elements ## ")]
    [SerializeField] private BoneManager boneManager;

    private void OnEnable()
    {
        InputManager.OnCoinClickedPosition += CoinClickedPositionHandler;
    }

    private void OnDisable()
    {
        InputManager.OnCoinClickedPosition -= CoinClickedPositionHandler;
    }

    private void CoinClickedPositionHandler(Vector2 clickedPosition)
    {
        // Get Pool Object:
        BoneParticle boneParticle = BoneParticlePool.Instance.Get();

        boneParticle.transform.position = clickedPosition;
        boneParticle.gameObject.SetActive(true);

        boneParticle.Configure(boneManager.GetBoneIncrement());

        // Destroy:
        LeanTween.delayedCall(1, () => boneParticle.Destroy());
    }
}
