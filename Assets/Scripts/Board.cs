using System.Collections;
using System.Collections.Generic;
//using System.Media;
using System.Security;
using UnityEngine;

public class Board : Object
{
    #region Attributes
    public string boardName;
    public bool realTime; //True if realtime, false if turn by turn
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

    //sert à distribuer la carte / initialiser le visuel
    static void Initialize_captainmap(int chosenMap, bool gameMode)
    {
        //Avoir une database des maps avec iles avant
        Map currentMap = new Map(chosenMap, gameMode);
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
