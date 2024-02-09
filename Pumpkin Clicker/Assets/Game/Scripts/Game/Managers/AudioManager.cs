
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private SoundSO clickSound;
    [SerializeField] private SoundSO upgradeSound;
    [SerializeField] private SoundSO panelOpen;
    [SerializeField] private SoundSO pawmodeStarted;

    private void OnEnable()
    {
        InputManager.OnBoneClicked += Sound_Click;
        UpgradeManager.OnUpgradePurchased += Sound_Upgrade;
        OfflineEarningsManager.OnPanelOpened += Sound_Open;
        PawModeManager.OnPawModeStarted += Sound_PawMode;
        GiftManager.OnPanelOpened += Sound_Open;
        LeaderBoardUI.OnPanelOpened += Sound_Open;
        TimeWrapManager.OnPanelOpened += Sound_Open;
        UIManager.OnPanelOpened += Sound_Open;
    }

    private void OnDisable()
    {
        InputManager.OnBoneClicked -= Sound_Click;
        UpgradeManager.OnUpgradePurchased -= Sound_Upgrade;
        OfflineEarningsManager.OnPanelOpened -= Sound_Open;
        PawModeManager.OnPawModeStarted -= Sound_PawMode;
        GiftManager.OnPanelOpened -= Sound_Open;
        LeaderBoardUI.OnPanelOpened -= Sound_Open;
        TimeWrapManager.OnPanelOpened -= Sound_Open;
        UIManager.OnPanelOpened -= Sound_Open;
    }

    private void PlaySound(SoundSO soundSO)
    {   
        AudioSource auidoSource = AudioPool.Instance.Get();
        auidoSource.clip = soundSO.Clip;
        auidoSource.gameObject.SetActive(true);
        auidoSource.Play();

        if(!soundSO.Loop)
            LeanTween.delayedCall(soundSO.Clip.length,()=> DestroyObject(auidoSource));
            
    }

    private void Sound_Click()
    {
        PlaySound(clickSound);
    }

    private void Sound_Upgrade()
    {
        PlaySound(upgradeSound);
    }

    private void Sound_Open()
    {
        PlaySound(panelOpen);

    }

    private void Sound_PawMode()
    {
        PlaySound(pawmodeStarted);
    }

    private void DestroyObject(AudioSource audioSource)
    {
        AudioPool.Instance.Set(audioSource);
    }
}
