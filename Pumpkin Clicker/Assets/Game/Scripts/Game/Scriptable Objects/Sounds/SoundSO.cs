using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects" , menuName = "Create a New Sound",order = 0)]
public class SoundSO : ScriptableObject
{
    public AudioClip Clip;
    public bool Loop = false;
    public float Volume = 1f;
    public float Pitch = 1f;
}
