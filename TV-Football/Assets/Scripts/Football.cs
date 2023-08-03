using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Football : MonoBehaviour
{
    public Values values;
    public Components components;

    private float height;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootToTarget(Vector3 target) 
    {
        Physics.gravity = Vector3.up * values.gravity;
        components.rigidbody.useGravity = true;
        height = Mathf.Clamp(target.y, 1, 99);
        components.rigidbody.velocity = CalculateLaunchData(target).initialVelocity;
    }

    public void ResetBall(Vector3 position)
    {
        components.trailRenderer.Clear();
        components.rigidbody.velocity = Vector3.zero;
        components.rigidbody.angularVelocity = Vector3.zero;
        transform.position = position;
    }

    private LaunchData CalculateLaunchData(Vector3 target)
    {
        float displacementY = target.y - components.rigidbody.position.y;
        Vector3 displacementXZ = new Vector3(target.x - components.rigidbody.position.x, 0, target.z - components.rigidbody.position.z);
        float time = Mathf.Sqrt(-2 * height / values.gravity) + Mathf.Sqrt(2 * (displacementY - height) / values.gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * values.gravity * height);
        Vector3 velocityXZ = displacementXZ / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(values.gravity), time);
    }

    [System.Serializable]
    public class Components
    {
        public Rigidbody rigidbody;
        public TrailRenderer trailRenderer;
    }

    [System.Serializable]
    public class Values
    {
        public float gravity = -18;
        public float speed = 1;

        public bool debugPath;
    }

    private struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }
    }
}
