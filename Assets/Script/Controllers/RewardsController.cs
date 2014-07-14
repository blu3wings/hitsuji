using UnityEngine;
using System.Collections;

public class RewardsController : MonoBehaviour 
{
    public Rating[] ratingConfig;

    private int _goalAchievedValue;
    private int _maxRatingValue;
    private float _fraction; 

    public void setupRating()
    {

    }

    /// <summary>
    /// Get the highes rating requirement value.
    /// </summary>
    private void getMaxRatingValue()
    {
        if(ratingConfig.Length > 0)
        {
            foreach(Rating r in ratingConfig)
            {
                if (_maxRatingValue < r.target)
                    _maxRatingValue = r.target;
            }
        }        
    }

    /// <summary>
    /// Get the total amount of notches required
    /// </summary>
    private void getFraction()
    {
        if (_maxRatingValue > 0)
            _fraction = 1.0f / _maxRatingValue;
    }

    public void updateValue()
    {
        _goalAchievedValue++;
    }
}
