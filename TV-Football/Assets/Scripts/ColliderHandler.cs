using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles collision callbacks
/// </summary>
public class ColliderHandler : MonoBehaviour
{
    [Tooltip("When something enters this collider bounds")]
    public UnityEvent onEnter;

    private void OnTriggerEnter(Collider other)
    {
        onEnter.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        onEnter.Invoke();
    }
}
