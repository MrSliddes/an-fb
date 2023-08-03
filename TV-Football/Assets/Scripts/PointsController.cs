using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsController : MonoBehaviour
{
    public int points;
    public TextMeshProUGUI pointsField;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePoints();   
    }

    public void AddPoints(int amount)
    {
        points += amount;
    }

    private void UpdatePoints()
    {
        pointsField.text = points.ToString();
    }
    
}
