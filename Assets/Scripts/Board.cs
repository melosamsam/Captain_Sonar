using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    #region Attributes

    private const string BOARD_PATH = "Assets/UI/Boards/";
    static private string mapName;

    public string boardName;
    public bool realTime; //True if realtime, false if turn by turn
    static string gameMode;

    #endregion

    #region Get
    public string GetNameBoard() { return boardName; }
    public bool GetGameMode() { return realTime; }

    #endregion

    #region Set

    public void SetNameBoard(string name) { boardName = name; }
    public void SetGameMode(bool mode) { realTime = mode; }

    #endregion

    #region Public methods

    /// <summary>
    /// Loads the correct image on a board of the game
    /// </summary>
    /// <param name="map">Map object corresponding to the current game's map</param>
    /// <param name="submarine">The team</param>
    /// <param name="role">The role for whom to load the Board</param>
    /// <param name="isTurnBased">If the game is currently played turn by turn, or in real time</param>
    static public void Initialize(Map map, Submarine submarine, Role role, bool isTurnBased=true)
    {
        gameMode = isTurnBased ? "Turn_by_Turn" : "Realtime";
        switch (role)
        {
            case Captain:
                InitializeCaptainBoard(map, submarine.Color, (Captain)role);
                break;

            case FirstMate:
                InitializeFirstMateBoard(submarine.Color, (FirstMate)role);
                break;

            case Engineer:
                InitializeEngineerBoard(submarine.Color, (Engineer)role);
                break;

            case RadioDetector:
                InitializeRadioDetector(map, submarine.Color, (RadioDetector)role);
                break;
        }
    }

    #endregion

    #region Private methods

    //sert à distribuer la carte / initialiser le visuel
    static public void InitializeCaptainBoard(Map map, string team, Captain role)
    {
        // the board to assign to the player
        Transform renderCamera = GameObject.Find($"{team} Captain").transform;
        GameObject capBoard = renderCamera.GetChild(0).Find("Board").gameObject;
        GameObject overlay = capBoard.transform.parent.GetChild(3).GetChild(2).gameObject;
        GameObject notification = capBoard.transform.parent.GetChild(4).gameObject;

        // the path where the correct sprite is
        string spritePath = $"{BOARD_PATH}Captain/{map.GetNameMap()}_Captain_{team}_{gameMode}.png";

        // path for the overlay background
        string overlayBgPath = $"Assets/UI/Buttons/Background_{ team.ToLower() }.png";

        // replace the default sprite by the correct sprite according to the game's settings and the Role
        capBoard.GetComponent<Image>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);

        // set the overlay background to correspond the team's color
        overlay.transform.Find("Background").GetComponent<Image>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>(overlayBgPath);

        // set the notification GameObject
        role.Notification = notification;
        role.Notification.GetComponent<Image>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>(overlayBgPath);

        // add event listener to input field
        TMP_InputField inputField = role.Notification.GetComponentInChildren<TMP_InputField>();
        inputField.onEndEdit.AddListener((value) => role.ChooseInitialPosition());

        UpdateUIEvents(renderCamera.GetChild(0).gameObject, role);
    }

    static void InitializeFirstMateBoard(string team, FirstMate role)
    {
        // the board to assign to the player
        Transform renderCamera = GameObject.Find($"{team} First Mate").transform;
        GameObject firstMateBoard = renderCamera.GetChild(0).Find("Board").gameObject;

        // the path where the correct sprite is
        string spritePath = $"{BOARD_PATH}First_Mate/First_Mate_{team}_{gameMode}.png";

        // replace the default sprite by the correct sprite according to the game's settings and the Role
        firstMateBoard.GetComponent<Image>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);

        UpdateUIEvents(firstMateBoard.transform.parent.gameObject, role);
    }

    static void InitializeEngineerBoard(string team, Engineer role)
    {
        // the board to assign to the player
        Transform renderCamera = GameObject.Find($"{team} Engineer").transform;
        GameObject engineerBoard = renderCamera.GetChild(0).Find("Board").gameObject;

        // the path where the correct sprite is
        string spritePath = $"{BOARD_PATH}Engineer/Engineer_{team}_{gameMode}.png";

        // replace the default sprite by the correct sprite according to the game's settings and the Role
        engineerBoard.GetComponent<Image>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);

        UpdateUIEvents(engineerBoard.transform.parent.gameObject, role);
    }

    static void InitializeRadioDetector(Map map, string team, RadioDetector role)
    {
        // the board to assign to the player
        Transform renderCamera = GameObject.Find($"{team} Radio Detector").transform;
        GameObject detectorBoard = renderCamera.GetChild(0).Find("Board").gameObject;

        // the path where the correct sprite is
        string spritePath = $"{BOARD_PATH}Radio_Detector/{map.GetNameMap()}_Detector_{team}_{gameMode}.png";

        // replace the default sprite by the correct sprite according to the game's settings and the Role
        detectorBoard.GetComponent<Image>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);

        UpdateUIEvents(renderCamera.GetChild(0).gameObject, role);
    }

    static void UpdateUIEvents(GameObject board, Role role)
    {
        role.Board = board.transform;

        switch (role)
        {
            case Captain:
                Captain captain = (Captain)role;

                // getting the interactable elements of the 'Actions' GameObject
                GameObject actions = board.transform.Find("Actions").gameObject;

                GameObject captainLogo = actions.transform.GetChild(0).gameObject;
                GameObject windRose = actions.transform.GetChild(1).gameObject;
                GameObject overlay = actions.transform.GetChild(2).gameObject;


                // configuring "Captain_logo" events
                UnityEngine.UI.Button logo = captainLogo.GetComponent<UnityEngine.UI.Button>();
                logo.onClick.AddListener(() => captain.ToggleOverlay());
                
                // configuring "WindRose" buttons events
                foreach (UnityEngine.UI.Button button in windRose.GetComponentsInChildren<UnityEngine.UI.Button>())
                    button.onClick.AddListener(() => captain.OrderSubmarineCourse(button.gameObject.name[0].ToString()));

                // configuring other powers
                GameObject background = overlay.transform.Find("Background").gameObject;

                // Surface power
                UnityEngine.UI.Button surfaceBtn = background.transform.Find("Surface").GetComponent<UnityEngine.UI.Button>();
                surfaceBtn.onClick.AddListener(() => captain.OrderSurface());

                // Activating systems power
                TMP_Dropdown dropdown = background.transform.Find("Systems dropdown").GetComponent<TMP_Dropdown>();
                captain.SystemDropdown = dropdown;
                dropdown.onValueChanged.AddListener((value) => captain.ActivateSystem());

                // configuring the "Close" button
                UnityEngine.UI.Button closeBtn = overlay.transform.Find("Close").GetComponent<UnityEngine.UI.Button>();
                closeBtn.onClick.AddListener(() => captain.ToggleOverlay());

                break;

            case FirstMate:
                FirstMate firstMate = (FirstMate)role;

                // configuring the buttons for filling systems' gauges
                GameObject systems = board.transform.GetChild(2).gameObject;
                foreach (Systems system in systems.GetComponentsInChildren<Systems>())
                {
                    UnityEngine.UI.Button systemBtn = system.gameObject.GetComponentInChildren<UnityEngine.UI.Button>();
                    systemBtn.onClick.AddListener(() => firstMate.FillGauge(system.gameObject.name.Split(' ')[0]));
                }

                break;

            case Engineer:
                break;

            case RadioDetector:
                RadioDetector radioDetector = (RadioDetector)role;

                // configuring the 'Radio_detector_logo' events
                Transform detectorActions = board.transform.Find("Actions");

                // configuring the See through parent GameObject as the Radio Detector's See Through
                GameObject seeThrough = detectorActions.GetChild(1).Find("See through").gameObject;
                radioDetector.SeeThrough = seeThrough;

                GameObject grid = detectorActions.GetChild(1).Find("TileGrid").gameObject;
                radioDetector.Grid = grid;

                GameObject dot = detectorActions.GetChild(1).Find("StartingDot").gameObject;
                radioDetector.Dot = dot;

                // Generate the grid
                GridManager gridManager = detectorActions.GetChild(1).Find("GridManager").GetComponent<GridManager>();
                gridManager.GenerateGrid(grid);

                UnityEngine.UI.Button detectorLogo = detectorActions.Find("Radio_detector_logo").GetComponent<UnityEngine.UI.Button>();
                detectorLogo.onClick.AddListener(() => radioDetector.ToggleSeeThrough());

                break;
        }
    }

    #endregion
}