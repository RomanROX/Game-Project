using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax_Bacground : MonoBehaviour
{
    [SerializeField] private float ParallaxEffectMultiplier;

    private Transform CameraTransform;
    private Vector3 LastCameraPos;

    // Start is called before the first frame update
    void Start()
    {
        CameraTransform = Camera.main.transform;
        LastCameraPos = CameraTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 DeltaMovement = CameraTransform.position - LastCameraPos;
        transform.position += DeltaMovement * ParallaxEffectMultiplier;
        LastCameraPos = CameraTransform.position;
    }
}
