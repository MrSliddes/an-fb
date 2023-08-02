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

    private Vector2 mouseNormalizedPosition;
    private Vector3 ballTargetPosition;

    // Start is called before the first frame update
    void Start()
    {
        components.ballTransform.position = components.ballRestTransform.position;
        ballTargetPosition = components.ballRestTransform.position;
        Debug.Log(GetComponent<RectTransform>().rect.width);
        Debug.Log(Screen.width);
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

        // Check if can shoot
        if(mouseNormalizedPosition.y >= values.touchBorderHeight.x && mouseNormalizedPosition.y <= values.touchBorderHeight.y)
        {
            ballTargetPosition = components.goalTarget.position;
        }
        else
        {
            ballTargetPosition = components.ballRestTransform.position;
        }

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

        // Set ball target
        mouseNormalizedPosition = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
        Vector2 normalizedPosition = mouseNormalizedPosition;
        Debug.Log($"n: {normalizedPosition}");
        normalizedPosition = new Vector2(Mathf.Clamp(normalizedPosition.x, values.touchBorderWidth.x, values.touchBorderWidth.y), Mathf.Clamp(normalizedPosition.y, values.touchBorderHeight.x, values.touchBorderHeight.y));
        Debug.Log($"clamped n pos: {normalizedPosition}");
        normalizedPosition = new Vector2(Map(normalizedPosition.x, values.touchBorderWidth.x, values.touchBorderWidth.y, 0, 1), Map(normalizedPosition.y, values.touchBorderHeight.x, values.touchBorderHeight.y, 0, 1));

        Vector2 goalPositionsWidth = new Vector2(components.goalBounds.transform.position.x - components.goalBounds.bounds.extents.x, components.goalBounds.transform.position.x + components.goalBounds.bounds.extents.x);
        Vector2 goalPositionHeight = new Vector2(components.goalBounds.transform.position.y - components.goalBounds.bounds.extents.y, components.goalBounds.transform.position.y + components.goalBounds.bounds.extents.y);

        components.goalTarget.position = new Vector3(Mathf.Lerp(goalPositionsWidth.x, goalPositionsWidth.y, normalizedPosition.x), Mathf.Lerp(goalPositionHeight.x, goalPositionHeight.y, normalizedPosition.y), components.goalTarget.position.z);
    }

    private void UpdateBallMovement()
    {        
        components.ballTransform.position = Vector3.MoveTowards(components.ballTransform.position, ballTargetPosition, values.ballDragSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Maps a value between 2 new values
    /// </summary>
    /// <param name="valueToMap">The value to map</param>
    /// <param name="oldMin">The old minimum value the valueToMap could be</param>
    /// <param name="oldMax">The old maximum value the valueToMap could be</param>
    /// <param name="newMin">The new minimum value the valueToMap can be</param>
    /// <param name="newMax">The new maximum value the valueToMap can be</param>
    /// <returns></returns>
    private float Map(float valueToMap, float oldMin, float oldMax, float newMin, float newMax)
    {
        float oldRange = oldMax - oldMin;
        float newRange = newMax - newMin;
        return (((valueToMap - oldMin) * newRange) / oldRange) + newMin;
    }

    private enum BallState
    {
        none,
        aiming,
        shooting
    }

    [System.Serializable]
    public class Components
    {
        public Transform ballTransform;
        public Transform ballRestTransform;
        public BoxCollider goalBounds;
        public Transform goalTarget;
    }

    [System.Serializable]
    public class Values
    {
        public float ballDragSpeed = 1;
        public float ballDistanceFromCamera = 4;
        public Vector2 touchBorderWidth = new Vector2(0.1f, 0.9f);
        public Vector2 touchBorderHeight = new Vector2(0.2f, 0.5f);
    }
}
