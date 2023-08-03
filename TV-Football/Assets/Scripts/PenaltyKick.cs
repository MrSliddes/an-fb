using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PenaltyKick : MonoBehaviour
{
    public Values values;
    public Components components;

    private bool allowInteraction;
    private bool clickedBall;
    private bool showTouchTrail;
    private Vector2 mousePosition;
    private Coroutine coroutineBallExceedTime;

    // Start is called before the first frame update
    void Start()
    {
        SetupBall();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
            Debug.Log("Pressed ball");
        }
    }

    private void SetupBall()
    {
        Vector3 pos = components.footballStartTransform.position + new Vector3(0, 0, -2);
        components.football.ResetBall(pos);
        components.footballTransform.DOMove(components.footballStartTransform.position, 1f);
        components.footballTransform.DORotate(new Vector3(360, 0, 0), 1f, RotateMode.FastBeyond360);
    }

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

    private IEnumerator BallExceedTime()
    {
        yield return new WaitForSeconds(values.ballMaxAliveTime);
        ResetBall();
    }

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
        /// <summary>
        /// Transform of the football
        /// </summary>
        public Transform footballTransform;
        /// <summary>
        /// Start position of football before kick
        /// </summary>
        public Transform footballStartTransform;
        /// <summary>
        /// The target to shoot at in the goal
        /// </summary>
        public Transform goalTargetTransform;
        /// <summary>
        /// The football script
        /// </summary>
        public Football football;
        /// <summary>
        /// Linerenderer for visualizing aim
        /// </summary>
        public TrailRenderer touchTrailRenderer;
    }

    [System.Serializable]
    public class Values
    {
        [Tooltip("Time in seconds to wait after shooting to reset ball")]
        public float ballMaxAliveTime = 3f;

        public float touchTrailOffset = 1;
    }
}
