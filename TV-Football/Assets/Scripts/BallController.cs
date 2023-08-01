using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Controlls the ball
/// </summary>
public class BallController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Values values;
    public Components components;

    private Vector3 ballTargetPosition;

    // Start is called before the first frame update
    void Start()
    {
        components.ballTransform.position = components.ballRestTransform.position;
        ballTargetPosition = components.ballRestTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBallMovement();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"Down {Input.mousePosition}");
        MoveBall();
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Up");
        ballTargetPosition = components.ballRestTransform.position;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        MoveBall();
    }

    private void MoveBall()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = Camera.main.nearClipPlane + values.ballDistanceFromCamera;
        ballTargetPosition = Camera.main.ScreenToWorldPoint(screenPos);
    }

    private void UpdateBallMovement()
    {        
        components.ballTransform.position = Vector3.MoveTowards(components.ballTransform.position, ballTargetPosition, values.ballDragSpeed * Time.deltaTime);
    }

    [System.Serializable]
    public class Components
    {
        public Transform ballTransform;
        public Transform ballRestTransform;
    }

    [System.Serializable]
    public class Values
    {
        public float ballDragSpeed = 1;
        public float ballDistanceFromCamera = 4;
    }
}
