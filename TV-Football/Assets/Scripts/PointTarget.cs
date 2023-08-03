using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTarget : MonoBehaviour
{
    public float targetReachTime = 3;
    public Vector3 targetPoint;

    // Start is called before the first frame update
    void Start()
    {
        transform.DOMove(targetPoint, targetReachTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GotHit()
    {
        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;
        //transform.DOShakePosition(1, 0.5f);
        transform.DOShakeRotation(1, 0.5f);
        transform.DOShakeScale(1, 0.5f);
    }
}
