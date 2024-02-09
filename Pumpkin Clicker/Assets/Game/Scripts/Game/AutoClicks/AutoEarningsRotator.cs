using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoEarningsRotator : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Transform rotator;

    [Header(" Settings ")]
    [SerializeField] private float rotatorSpeed;


    private void Update() 
    {
        rotator.Rotate(Vector3.forward * Time.deltaTime * rotatorSpeed);
    }
}
