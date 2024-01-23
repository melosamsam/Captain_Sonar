using System.Collections;
using System.Collections.Generic;
//using System.Media;
using System.Security;
using UnityEditor;
using UnityEngine;

public class Board : MonoBehaviour
{
    #region Attributes
    private const string boardPath = "Assets/UI/Boards/";
    static private string mapName;

    [SerializeField] static private GameObject captainBoardPrefab;
    [SerializeField] static private GameObject firstMateBoardPrefab;
    [SerializeField] static private GameObject engineerBoardPrefab;
    [SerializeField] static private GameObject radioDetectorBoardPrefab;


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

    private void Awake()
    {
        gameMode = realTime ? "Realtime" : "Turn_by_Turn";
    }

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

    //sert à distribuer la carte / initialiser le visuel
    static void Initialize_captainmap(int chosenMap, bool gameMode)
    {
        //Avoir une database des maps avec iles avant
        Map currentMap = new Map(chosenMap, gameMode);
    }

    static public GameObject InitializeCaptainBoard(int teamNb)
    {
        // the board to assign to the player
        GameObject capBoard = captainBoardPrefab;
        string team = teamNb == 0 ? "Blue" : "Red";

        // the path where the sprite is
        string spritePath = $"{boardPath}Captain/{mapName}_Captain_{team}_{gameMode}.png";

        // replace the default sprite by the correct sprite according to the game's settings and the Role
        capBoard.GetComponent<SpriteRenderer>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);

        return capBoard;
    }

    static void Initialize_firstMateBoard()
    {
        //Créer les systèmes et jauges avant
        //sert à distribuer la carte / initialiser le visuel
    }
    static void Initialize_engineerBoard(bool gameMode)
    {
        //Créer les élements (radioactivité,…) avant
        //Le gameMode influe que pour faire surface je crois, ce serait bien de trouver autre chose qu'entourer le bateau parce que c'est pas pratique avec la souris

    }
    static void Initialize_SeeThrough()
    {
        //Dimension x4 taille de la map ou juste très grand 
        //Avec même quadrillage que map, option d’effacement/dessin/...
    }

    // Espace commentaire disponible pour tous les joueurs avec un panel qu’on clique (flèche,…) pour prendre des notes 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
