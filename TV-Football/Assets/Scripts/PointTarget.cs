using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Target that can be hit with a ball to get bonus points
/// </summary>
public class PointTarget : MonoBehaviour
{
    public Values values;

    // Start is called before the first frame update
    void Start()
    {
        transform.DOMove(values.targetPoint, values.targetReachTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>
    /// Callback when a point target is hit by ball
    /// </summary>
    public void GotHit()
    {
        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;
        transform.DOShakeRotation(1, 0.5f);
        transform.DOShakeScale(1, 0.5f);
    }

    [System.Serializable]
    public class Values
    {
        [Tooltip("Time it takes to reach target")]
        public float targetReachTime = 3;
        [Tooltip("The world position to reach")]
        public Vector3 targetPoint;
    }
}
