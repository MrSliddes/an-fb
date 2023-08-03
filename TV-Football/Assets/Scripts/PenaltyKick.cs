using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PenaltyKick : MonoBehaviour
{
    public Values values;
    public Components components;

    private bool clickedBall;
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
            Debug.Log("Pressed ball");
        }
    }

    private void SetupBall()
    {
        Vector3 pos = components.footballStartTransform.position + new Vector3(0, 0, -2);
        components.football.ResetBall(pos);
        components.footballTransform.DOMove(components.footballStartTransform.position, 1f);
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

    #region Input

    /// <summary>
    /// When the user presses the screen
    /// </summary>
    /// <param name="context"></param>
    public void OnInputClick(InputAction.CallbackContext context)
    {
        if(context.ReadValue<float>() > 0)
        {
            // Press down
            CheckForBall();
        }
        else
        {
            // Press up
            if(clickedBall)
            {
                ShootBall();
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
        mousePosition = context.ReadValue<Vector2>();
        Debug.Log(mousePosition);
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
        public Football football;
    }

    [System.Serializable]
    public class Values
    {
        [Tooltip("Time in seconds to wait after shooting to reset ball")]
        public float ballMaxAliveTime = 3f;
    }
}
