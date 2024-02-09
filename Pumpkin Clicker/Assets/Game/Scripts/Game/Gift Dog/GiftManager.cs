using System;
using UnityEngine;

public class GiftManager : MonoBehaviour
{
    public static GiftManager Instance{get; private set;}

    [Header(" Elements ")]
    [SerializeField] GiftButtonShaker giftButton;
    [SerializeField] private RectTransform _giftPopupRect;
    [SerializeField] private GiftPopup giftPopup;

    [Header(" Settings ")]
    private float timeThreshold = 180f;
    private float currentTime = 0f;
    private bool _isButtonActive;

    private Vector2 _panelOpenedPosition;
    private Vector2 _panelClosedPosition;

    [Header(" Data ")]
    private float maxMultiplier = 600f; // 1 hour
    private float minMultiplier = 1200f; // 30 min

    [Header(" Actions ")]
    public static Action OnPanelOpened;


    private void Awake() 
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _panelOpenedPosition = Vector2.zero;
        _panelClosedPosition = new Vector2(0, _giftPopupRect.rect.height);

        giftButton.gameObject.SetActive(false);

        _giftPopupRect.anchoredPosition = _panelClosedPosition;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime >= timeThreshold && !_isButtonActive)
        {            
            giftButton.gameObject.SetActive(true);
            _isButtonActive = true;
            currentTime = 0f;
        }

        if(currentTime >= timeThreshold / 2 && _isButtonActive)
        {
            giftButton.gameObject.SetActive(false);
            _isButtonActive = false;
            currentTime = 0;
        }

    }

    private void CalculateGiftAmount()
    {
        float cps = DataManager.Instance.CurrentCps;
        
        float randomMultiplier = UnityEngine.Random.Range(maxMultiplier, minMultiplier);

        float giftAmount = cps * randomMultiplier;
        Debug.Log("Gift Amount " + giftAmount);

        giftPopup.Configure((double)giftAmount);
    }


    public void OpenPanel()
    {
        CalculateGiftAmount();
       
        LeanTween.cancel(_giftPopupRect);
        OnPanelOpened?.Invoke();
        LeanTween.move(_giftPopupRect, _panelOpenedPosition, .3f).setEase(LeanTweenType.easeOutBack);
    }

    public void ClosePanel()
    {
        giftButton.gameObject.SetActive(false);
        _isButtonActive = false;

        LeanTween.cancel(_giftPopupRect);
        OnPanelOpened?.Invoke();
        LeanTween.move(_giftPopupRect, _panelClosedPosition, .2f).setEase(LeanTweenType.easeOutBack);        
    }
}
