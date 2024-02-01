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
                InitializeCaptainBoard(map, submarine.Color);
                break;

            case FirstMate:
                InitializeFirstMateBoard(submarine.Color);
                break;

            case Engineer:
                InitializeEngineerBoard(submarine.Color);
                break;

            case RadioDetector:
                InitializeRadioDetector(map, submarine.Color);
                break;
        }
    }

    #endregion

    #region Private methods

    //sert à distribuer la carte / initialiser le visuel
    static void Initialize_captainmap(int chosenMap, bool gameMode)
    {
        //Avoir une database des maps avec iles avant
        Map currentMap = new Map(chosenMap, gameMode);
    }

    static public void InitializeCaptainBoard(Map map, string team)
    {
        // the board to assign to the player
        Transform renderCamera = GameObject.Find($"{team} Captain").transform;
        GameObject capBoard = renderCamera.GetChild(0).Find("Board").gameObject;

        // the path where the correct sprite is
        string spritePath = $"{BOARD_PATH}Captain/{map.GetNameMap()}_Captain_{team}_{gameMode}.png";

        // replace the default sprite by the correct sprite according to the game's settings and the Role
        capBoard.GetComponent<Image>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
    }

    static void Initialize_firstMateBoard()
    {
        //Créer les systèmes et jauges avant
        //sert à distribuer la carte / initialiser le visuel
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
        //Créer les élements (radioactivité,…) avant
        //Le gameMode influe que pour faire surface je crois, ce serait bien de trouver autre chose qu'entourer le bateau parce que c'est pas pratique avec la souris

    }

    static void InitializeEngineerBoard(string team)
    {
        // the board to assign to the player
        Transform renderCamera = GameObject.Find($"{team} First Mate").transform;
        GameObject engineerBoard = renderCamera.GetChild(0).Find("Board").gameObject;

        // the path where the correct sprite is
        string spritePath = $"{BOARD_PATH}Engineer/Engineer_{team}_{gameMode}.png";

        // replace the default sprite by the correct sprite according to the game's settings and the Role
        engineerBoard.GetComponent<Image>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
    }

    static void Initialize_SeeThrough()
    {
        //Dimension x4 taille de la map ou juste très grand 
        //Avec même quadrillage que map, option d’effacement/dessin/...
    }

    static void InitializeRadioDetector(Map map, string team)
    {
        // the board to assign to the player
        Transform renderCamera = GameObject.Find($"{team} Radio Detector").transform;
        GameObject detectorBoard = renderCamera.GetChild(0).Find("Board").gameObject;

        // the path where the correct sprite is
        string spritePath = $"{BOARD_PATH}Radio_Detector/{map.GetNameMap()}_Detector_{team}_{gameMode}.png";

        // replace the default sprite by the correct sprite according to the game's settings and the Role
        detectorBoard.GetComponent<Image>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
    }

    #endregion
}
