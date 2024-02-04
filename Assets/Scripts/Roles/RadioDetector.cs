using UnityEngine;
using UnityEngine.SceneManagement;

public class RadioDetector : Role
{

    #region Attributes

    private bool _isSeeThroughOpen;
    private bool _isGridOpen;
    private bool _isDotOpen;

    [SerializeField] private GameObject _seeThrough;
    [SerializeField] private GameObject _Grid;
    [SerializeField] private GameObject _Dot;

    #endregion

    #region Properties

    /// <summary>
    /// 
    /// </summary>
    public GameObject SeeThrough { get => _seeThrough; set { _seeThrough = value; } }

    public GameObject Grid { get => _Grid; set { _Grid = value; } }

    public GameObject Dot { get => _Dot; set { _Dot = value; } }

    #endregion

    #region Overridden methods

    /// <summary>
    /// Method called when the turn of the Captain finishes or ends.
    /// </summary>
    protected override void OnActionStatusChanged()
    {
        ToggleUI();
    }

    protected override void SetDescription()
    {
        Name = "Radio Detector";
        Description =
            "The captain is the central element of the entire crew.\n" +
            "In addition to being responsible for the trajectory taken by the submarine, they must be the link between all other posts."
            ;
    }

    /// <summary>
    /// Method enabling/disabling the UI elements used to perform role actions according to whether the turn is done or not.
    /// </summary>
    protected override void ToggleUI()
    {
        GameObject canvas = GameObject.Find("Canvas");
        canvas.transform.localScale = !IsTurnOver ? Vector3.one : Vector3.zero;
    }


    #endregion

    #region Unity methods

    private void Awake()
    {
        SetDescription();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("TestRadioDetector"))
        {
            _seeThrough = GameObject.Find("See through");
            _Grid = GameObject.Find("TileGrid");
            _Dot = GameObject.Find("StartingDot");
        }

        _isSeeThroughOpen = false;
        _isGridOpen = false;
        _isDotOpen = false;
        _Grid.transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isSeeThroughOpen)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                ToggleSeeThrough();
            }
        }
    }

    #endregion

    #region Methods

    public void ToggleSeeThrough()
    {
        _isSeeThroughOpen = !_isSeeThroughOpen;
        Debug.Log($"See through visible: {_isSeeThroughOpen}");
        _seeThrough.transform.localScale = _isSeeThroughOpen ? new Vector3(1.2f, 1.7f, 1) : Vector3.zero;
        _isGridOpen = !_isGridOpen;
        _Grid.transform.localScale = _isGridOpen ? Vector3.one : Vector3.zero;
        _isDotOpen = !_isDotOpen;
        _Dot.transform.localScale = _isDotOpen ? new Vector3(30, 30, 5) : Vector3.zero;
    }

    #endregion

}
