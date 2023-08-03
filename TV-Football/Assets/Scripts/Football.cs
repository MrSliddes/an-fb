using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles the football physics
/// </summary>
public class Football : MonoBehaviour
{
    public Values values;
    public Components components;
    public Events events;

    /// <summary>
    /// Stored height value of football for calculating inital velocity when shooting
    /// </summary>
    private float height;

    /// <summary>
    /// Shoot the ball towords a target position
    /// </summary>
    /// <param name="target"></param>
    public void ShootToTarget(Vector3 target) 
    {
        Physics.gravity = Vector3.up * values.gravity;
        components.rigidbody.useGravity = true;
        height = Mathf.Clamp(target.y, 1, 99);
        components.rigidbody.velocity = CalculateLaunchData(target).initialVelocity;
        events.onShoot.Invoke();
    }

    /// <summary>
    /// Reset the ball towords a position
    /// </summary>
    /// <param name="position"></param>
    public void ResetBall(Vector3 position)
    {
        components.rigidbody.velocity = Vector3.zero;
        components.rigidbody.angularVelocity = Vector3.zero;
        transform.position = position;
        components.trailRenderer.Clear();
    }

    /// <summary>
    /// Calculate the lauch data (velocity) for the ball needed to reach target
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private LaunchData CalculateLaunchData(Vector3 target)
    {
        // Get velocity with fancy kinematic equation calculation
        float displacementY = target.y - components.rigidbody.position.y;
        Vector3 displacementXZ = new Vector3(target.x - components.rigidbody.position.x, 0, target.z - components.rigidbody.position.z);
        float time = Mathf.Sqrt(-2 * height / values.gravity) + Mathf.Sqrt(2 * (displacementY - height) / values.gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * values.gravity * height);
        Vector3 velocityXZ = displacementXZ / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(values.gravity), time);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Get callback when ball hit something
        events.onHitSomething.Invoke();
    }

    [System.Serializable]
    public class Components
    {
        [Tooltip("The balls rigidbody")]
        public Rigidbody rigidbody;
        [Tooltip("Trailrenderer of the ball to visualize cool velocity")]
        public TrailRenderer trailRenderer;
    }

    [System.Serializable]
    public class Events
    {
        [Tooltip("When the ball gets shot")]
        public UnityEvent onShoot;
        [Tooltip("When the ball hits a collider")]
        public UnityEvent onHitSomething;
    }

    [System.Serializable]
    public class Values
    {
        [Tooltip("The gravity applied to the ball")]
        public float gravity = -18;
    }

    /// <summary>
    /// Launch data struct for storing values for shooting
    /// </summary>
    private struct LaunchData
    {
        /// <summary>
        /// The velocity the ball gets after calculation
        /// </summary>
        public readonly Vector3 initialVelocity;
        /// <summary>
        /// The estimated time to reach target
        /// </summary>
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }
    }
}
