using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles the interaction with the ball and the user
/// </summary>
public class PenaltyKick : MonoBehaviour
{
    public Values values;
    public Components components;

    /// <summary>
    /// Is the user allowed to interact with the ball?
    /// </summary>
    private bool allowInteraction;
    /// <summary>
    /// Has the ball been clicked
    /// </summary>
    private bool clickedBall;
    /// <summary>
    /// Show the touch trail of the user
    /// </summary>
    private bool showTouchTrail;
    /// <summary>
    /// Current mouse (vinger) position of user
    /// </summary>
    private Vector2 mousePosition;
    /// <summary>
    /// Coroutine for recalling ball after x time
    /// </summary>
    private Coroutine coroutineBallExceedTime;

    // Start is called before the first frame update
    void Start()
    {
        SetupBall();
    }

    /// <summary>
    /// Is the user allowed to interact with ball?
    /// </summary>
    /// <param name="value"></param>
    public void AllowInteraction(bool value)
    {
        allowInteraction = value;
    }

    /// <summary>
    /// Reset the ball
    /// </summary>
    public void ResetBall()
    {
        if(coroutineBallExceedTime != null) StopCoroutine(coroutineBallExceedTime);
        SetupBall();
    }


    /// <summary>
    /// Check for a ball at mousePosition
    /// </summary>
    private void CheckForBall()
    {
        // Check for a ball at the mouse position
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if(Physics.Raycast(ray, out hit))
        {
            // Get ball
            Football football = hit.transform.GetComponent<Football>();
            if(football == null) return;
            clickedBall = true;
            // Touch Trail
            showTouchTrail = true;
            Vector3 input = mousePosition;
            input.z = values.touchTrailOffset;
            components.touchTrailRenderer.transform.position = Camera.main.ScreenToWorldPoint(input);
            components.touchTrailRenderer.Clear();
        }
    }

    /// <summary>
    /// Setup the ball
    /// </summary>
    private void SetupBall()
    {
        Vector3 pos = components.footballStartTransform.position + new Vector3(0, 0, -2);
        components.football.ResetBall(pos);
        components.footballTransform.DOMove(components.footballStartTransform.position, 1f);
        components.footballTransform.DORotate(new Vector3(360, 0, 0), 1f, RotateMode.FastBeyond360);
    }

    /// <summary>
    /// Shoot the ball towords where the user released input
    /// </summary>
    private void ShootBall()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if(Physics.Raycast(ray, out hit))
        {
            components.goalTargetTransform.position = hit.point;
            components.football.ShootToTarget(hit.point);
            coroutineBallExceedTime = StartCoroutine(BallExceedTime());
        }
    }

    /// <summary>
    /// Reset ball after exceeding alive time
    /// </summary>
    /// <returns></returns>
    private IEnumerator BallExceedTime()
    {
        yield return new WaitForSeconds(values.ballMaxAliveTime);
        ResetBall();
    }

    /// <summary>
    /// Update the touch trail of the user
    /// </summary>
    private void UpdateTouchTrail()
    {
        if(!showTouchTrail) return;
        if(components.touchTrailRenderer == null) return;

        Vector3 input = mousePosition;
        input.z = values.touchTrailOffset;
        components.touchTrailRenderer.transform.position = Camera.main.ScreenToWorldPoint(input);
    }

    #region Input

    /// <summary>
    /// When the user presses the screen
    /// </summary>
    /// <param name="context"></param>
    public void OnInputClick(InputAction.CallbackContext context)
    {

        if(context.ReadValue<float>() > 0)
        {
            if(!allowInteraction) return;
            // Press down
            CheckForBall();
        }
        else
        {
            // Press up
            if(clickedBall)
            {
                ShootBall();
                components.touchTrailRenderer.Clear();
                showTouchTrail = false;
            }

            clickedBall = false;
        }
    }

    /// <summary>
    /// The position of users finger
    /// </summary>
    /// <param name="context"></param>
    public void OnInputPoint(InputAction.CallbackContext context)
    {
        if(!allowInteraction) return;

        mousePosition = context.ReadValue<Vector2>();

        UpdateTouchTrail();
    }

    #endregion

    [System.Serializable]
    public class Components
    {
        [Tooltip("Transform of the football")]
        public Transform footballTransform;
        [Tooltip("Start position of the football before kick")]
        public Transform footballStartTransform;
        [Tooltip("Target to shoot at in the goal")]
        public Transform goalTargetTransform;
        [Tooltip("Link to football script")]
        public Football football;
        [Tooltip("Trailrenderer used for visualizing user aim")]
        public TrailRenderer touchTrailRenderer;
    }

    [System.Serializable]
    public class Values
    {
        [Tooltip("Time in seconds to wait after shooting to reset ball")]
        public float ballMaxAliveTime = 3f;
        [Tooltip("Offset of visual touch trail of user")]
        public float touchTrailOffset = 1;
    }
}
