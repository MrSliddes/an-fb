using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

/// <summary>
/// Keeps track of points
/// </summary>
public class PointsController : MonoBehaviour
{
    public Values values;
    public Components components;

    /// <summary>
    /// Current amount of points
    /// </summary>
    private int points;

    /// <summary>
    /// Add points to point controller
    /// </summary>
    /// <param name="amount"></param>
    public void AddPoints(int amount)
    {
        if(amount == 0) amount = values.defaultAddScore;
        points += amount;
        UpdatePoints();
    }

    /// <summary>
    /// Reset the score
    /// </summary>
    public void ResetScore()
    {
        points = 0;
        UpdatePoints();
    }

    /// <summary>
    /// Update the visual points
    /// </summary>
    private void UpdatePoints()
    {
        components.pointsField.text = points.ToString();
    }

    [System.Serializable]
    public class Components
    {
        [Tooltip("TMP text field for displaying the points")]
        public TextMeshProUGUI pointsField;
    }

    [System.Serializable]
    public class Values
    {
        [Tooltip("If score isnt set add this")]
        public int defaultAddScore = 1000;
    }
}
