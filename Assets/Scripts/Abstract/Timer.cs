using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    #region Attributes

    bool _isCountingDown; // Flag to indicate whether the countdown is active or not
    bool _isTimeElapsed;
    [SerializeField] List<TMP_Text> _timerText = new();
    float _remainingTime; // The remaining time for the countdown

    #endregion

    #region Properties

    /// <summary>
    /// Property to get or set the timer text components
    /// </summary>
    public List<TMP_Text> TimerText {  get => _timerText; }

    /// <summary>
    /// Property to get the status of whether the countdown's time has elapsed
    /// </summary>
    public bool IsTimeElapsed { get => _isTimeElapsed; }

    #endregion

    #region Unity methods

    // Start is called before the first frame update
    void Start()
    {
        _isCountingDown = false;
        _isTimeElapsed = false;
    }

    // Update is called once per frame
    void Update()
    {
        // If the countdown is active
        if (_isCountingDown)
        {
            // Decrease the remaining time based on deltaTime
            if (_remainingTime > 0) { _remainingTime -=  Time.deltaTime; }
            else 
            {
                // If the remaining time is zero or less, set it to zero and mark the time as elapsed
                _remainingTime = 0;
                _isTimeElapsed = true;
            }

            // Update the displayed time
            DisplayTime();
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Sets the starting time to count down from.
    /// </summary>
    /// <param name="time">Time to set in seconds</param>
    public void SetStartingTime(float time)
    {
        _remainingTime = time;
        DisplayTime(); // Display the initial time
    }

    /// <summary>
    /// Starts the countdown.
    /// </summary>
    public void StartCountdown()
    {
        _isCountingDown = true;
    }

    /// <summary>
    /// Stops the countdown.
    /// </summary>
    public void StopCountdown()
    {
        _isCountingDown = false;
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Updates the displayed time on the assigned Text components
    /// </summary>
    void DisplayTime()
    {
        int minutes = Mathf.FloorToInt(_remainingTime / 60);
        int seconds = Mathf.FloorToInt(_remainingTime % 60);

        // Format the time and update each assigned Text component
        foreach ( TMP_Text text in _timerText )
            text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    #endregion
}
