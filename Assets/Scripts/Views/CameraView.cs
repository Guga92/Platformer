using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraView : MonoBehaviour
{
    [field: SerializeField] public Camera Camera { get; private set; }
    [field: SerializeField] public float Smoothing { get; private set; }
}
