using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour, ICameraShake
{
    [SerializeField] private Animator _cameraAnimator;

    public void Shake()
    {
        _cameraAnimator = GameObject.Find("CM Follow Cam").GetComponent<Animator>();
        if (_cameraAnimator != null)
        {
            _cameraAnimator.SetTrigger("Shake");
        }
    }
}
