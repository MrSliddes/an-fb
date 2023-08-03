using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderHandler : MonoBehaviour
{
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
