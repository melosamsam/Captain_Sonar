using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    #region Attributes

    bool _isCountingDown;
    [SerializeField] List<TMP_Text> _timerText = new();
    float _remainingTime;

    #endregion

    public List<TMP_Text> timerText {  get { return _timerText; } set { _timerText = value; } }

    #region Unity methods

    // Start is called before the first frame update
    void Start()
    {
        _isCountingDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isCountingDown)
        {
            if (_remainingTime > 0) { _remainingTime -=  Time.deltaTime; }
            else { _remainingTime = 0; }
            DisplayTime();
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Sets the starting time to count down from
    /// </summary>
    /// <param name="time">Time to set in minutes</param>
    public void SetStartingTime(float time)
    {
        _remainingTime = time * 60;
        DisplayTime();
    }

    public void StartCountdown()
    {
        _isCountingDown = true;
    }

    public void StopCountdown()
    {
        _isCountingDown = false;
    }

    #endregion

    #region Private methods

    void DisplayTime()
    {
        int minutes = Mathf.FloorToInt(_remainingTime / 60);
        int seconds = Mathf.FloorToInt(_remainingTime % 60);
        foreach ( TMP_Text text in _timerText )
            text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    #endregion
}
