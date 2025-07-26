using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimationTrigger : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void SetTrigger(string trigger)
    {
        _animator.SetTrigger(trigger);
    }
}
