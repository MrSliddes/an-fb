using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains data for remote configuration
/// </summary>
[System.Serializable]
public class RemoteConfigData
{
    [Tooltip("How long the ball can be alive for")]
    public float ballMaxAliveTime = 3;
    [Tooltip("Default score per goal")]
    public int scorePerGoal = 500;
    [Tooltip("Gravity of the ball")]
    public float ballGravity = -18;
    [Tooltip("How long 1 game takes in seconds")]
    public float gameTime = 30;
}
