using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogManager : MonoBehaviour
{
    #region Attributes

    [SerializeField] GameObject _logPrefab;

    private LogItem _currentLogItem;

    private bool _editMode;
    private TMP_InputField _inputField;
    private Transform _logContainer;

    private ScrollRect _scrollRect;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the currently selected or edited log item.
    /// </summary>
    public LogItem CurrentLogItem { get => _currentLogItem; set { _currentLogItem = value; } }

    /// <summary>
    /// Gets or sets the editing status, indicating whether the user is editing an existing log or not.
    /// </summary>
    public bool EditMode { get { return _editMode; } set { _editMode = value; } }

    /// <summary>
    /// Gets the text entry field associated with the current log management.
    /// </summary>
    public TMP_InputField InputField { get { return _inputField; } }

    #endregion

    #region Unity methods

    // Start is called before the first frame update
    void Start()
    {
        _editMode = false;
        _inputField = GameObject.Find("Log Input").GetComponent<TMP_InputField>();
        _logContainer = GameObject.Find("Log Panel").GetComponent<Transform>();
        _scrollRect = GameObject.Find("Scroll").GetComponent<ScrollRect>();

        _currentLogItem = null;
    }

    // Update is called every frame
    void Update()
    {
        if (_editMode)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                CancelEditMode();
        }
        // Debug.Log("Focused status: " + _inputField.isFocused);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Sends log data, adding a new log or editing the current one depending on the editing status.
    /// </summary>
    public void SendLogData()
    {
        if (!_editMode) AddLog();
        else EditLog(_inputField.text);

        ClearInputField();
    }

    #endregion

    #region Private methods

    // Adds a new log to the UI
    void AddLog()
    {
        string log = _inputField.text;
        if (log.TrimEnd(' ') != "") InstantiateLog(log);
    }

    // Cancels the edit mode
    void CancelEditMode()
    {
        Debug.Log("Cancelled edit mode");
        EditLog(_currentLogItem.Text);
        ClearInputField();
    }

    // Clears the input field
    void ClearInputField()
    {
        _inputField.text = "";
    }

    // Modifies the currently selected log
    void EditLog(string text)
    {
        _currentLogItem.Text = text;
        _editMode = false;
        _currentLogItem = null;
    }

    // Instantiates a new log item with the specified text
    void InstantiateLog(string text)
    {
        GameObject newLog = Instantiate(_logPrefab, _logContainer);
        if (newLog.TryGetComponent<LogItem>(out var logScript))
        {
            logScript.Text = text;
            ScrollToBottom();
        }
    }

    // Scrolls down the user interface to view the last added log item
    void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        _scrollRect.verticalNormalizedPosition = 0f;
    }

    #endregion
}
