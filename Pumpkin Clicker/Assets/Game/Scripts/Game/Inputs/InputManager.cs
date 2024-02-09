using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Camera _camera;

    [Header(" ## Actions ## ")]
    public static Action OnBoneClicked;
    public static Action<Vector2> OnCoinClickedPosition;
    public static Action OnNoBoneClicked;

    private void Awake() 
    {
        _camera = Camera.main;
    }

    private void Start() {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if(Input.touchCount > 0)
        {
            ManageTouches();
            Debug.Log("Click");
        }
    }

    private void ManageTouches()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if(touch.phase == TouchPhase.Began)
            {
                ThrowRaycast(touch.position);
            }
        }
    }

    private void ThrowRaycast(Vector2 touchPosition)
    {
       RaycastHit2D hit = Physics2D.GetRayIntersection(_camera.ScreenPointToRay(touchPosition));

       if(hit.collider == null) return;

       Debug.Log("We hit a Coin !");
       OnBoneClicked?.Invoke();
       OnCoinClickedPosition?.Invoke(hit.point);
    }
}
