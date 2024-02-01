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

    #region Constructor

    public Board (string boardName, bool realTime)
    {
        this.boardName = boardName;
        this.realTime = realTime;
        if (boardName == "captain")
        {
            //Initialize_captainmap(realTime, chosenMap)
            //chosenMap choisi dans le script du jeu (main)
        }
        else if (boardName == "firstMate") Initialize_firstMateBoard();
        else if (boardName == "engineer") Initialize_engineerBoard(realTime);
        else if (boardName=="detector")
        {
            Initialize_SeeThrough();
            //Initialize_captainmap(realTime, chosenMap)
            //chosenMap choisi dans le script du jeu (main)
        }
    }

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
                InitializeFirstMateBoard(submarine.Color);
                break;

            case Engineer:
                InitializeEngineerBoard(submarine.Color);
                break;

            case RadioDetector:
                InitializeRadioDetector(map, submarine.Color, (RadioDetector)role);
                break;
        }
    }

    #endregion

    #region Private methods

    //sert � distribuer la carte / initialiser le visuel
    static void Initialize_captainmap(int chosenMap, bool gameMode)
    {
        //Avoir une database des maps avec iles avant
        Map currentMap = new Map(chosenMap, gameMode);
    }

    static public void InitializeCaptainBoard(Map map, string team, Captain role)
    {
        // the board to assign to the player
        Transform renderCamera = GameObject.Find($"{team} Captain").transform;
        GameObject capBoard = renderCamera.GetChild(0).Find("Board").gameObject;

        // the path where the correct sprite is
        string spritePath = $"{BOARD_PATH}Captain/{map.GetNameMap()}_Captain_{team}_{gameMode}.png";

        // replace the default sprite by the correct sprite according to the game's settings and the Role
        capBoard.GetComponent<Image>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);

        UpdateUIEvents(renderCamera.GetChild(0).gameObject, role);
    }

    static void Initialize_firstMateBoard()
    {
        //Cr�er les syst�mes et jauges avant
        //sert � distribuer la carte / initialiser le visuel
    }

    static void InitializeFirstMateBoard(string team)
    {
        // the board to assign to the player
        Transform renderCamera = GameObject.Find($"{team} First Mate").transform;
        GameObject firstMateBoard = renderCamera.GetChild(0).Find("Board").gameObject;

        // the path where the correct sprite is
        string spritePath = $"{BOARD_PATH}First_Mate/First_Mate_{team}_{gameMode}.png";

        // replace the default sprite by the correct sprite according to the game's settings and the Role
        firstMateBoard.GetComponent<Image>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
    }

    static void Initialize_engineerBoard(bool gameMode)
    {
        //Cr�er les �lements (radioactivit�,�) avant
        //Le gameMode influe que pour faire surface je crois, ce serait bien de trouver autre chose qu'entourer le bateau parce que c'est pas pratique avec la souris

    }

    static void InitializeEngineerBoard(string team)
    {
        // the board to assign to the player
        Transform renderCamera = GameObject.Find($"{team} Engineer").transform;
        GameObject engineerBoard = renderCamera.GetChild(0).Find("Board").gameObject;

        // the path where the correct sprite is
        string spritePath = $"{BOARD_PATH}Engineer/Engineer_{team}_{gameMode}.png";

        // replace the default sprite by the correct sprite according to the game's settings and the Role
        engineerBoard.GetComponent<Image>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
    }

    static void Initialize_SeeThrough()
    {
        //Dimension x4 taille de la map ou juste tr�s grand 
        //Avec m�me quadrillage que map, option d�effacement/dessin/...
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

            case RadioDetector:
                RadioDetector radioDetector = (RadioDetector)role;

                // configuring the 'Radio_detector_logo' events
                Transform detectorActions = board.transform.Find("Actions");

                UnityEngine.UI.Button detectorLogo = detectorActions.Find("Radio_detector_logo").GetComponent<UnityEngine.UI.Button>();
                detectorLogo.onClick.AddListener(() => radioDetector.ToggleSeeThrough());

                // configuring the See through parent GameObject as the Radio Detector's See Through
                GameObject seeThrough = detectorActions.Find("SeeThroughParent").gameObject;
                radioDetector.SeeThrough = seeThrough;

                break;
        }
    }

    #endregion
}