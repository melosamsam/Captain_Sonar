using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogItem : MonoBehaviour
{

    #region Attributes

    [SerializeField] private TMP_Text _text;

    [SerializeField] private Button _editButton;
    [SerializeField] private Button _deleteButton;

    private LogManager _logManager;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the text associated with this log item
    /// </summary>
    public string Text 
    { 
        get => _text.text; 
        set { _text.text = value != "" ? value : ""; }
    }

    #endregion

    #region Unity methods

    private void Awake()
    {
        _logManager = FindObjectOfType<LogManager>();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Removes this log item from the UI
    /// </summary>
    public void DeleteLog()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Starts editing this log item in the UI
    /// </summary>
    public void EditLog()
    {
        _logManager.EditMode = true;
        _logManager.InputField.text = _text.text;
        _logManager.CurrentLogItem = this;
    }

    #endregion
}
