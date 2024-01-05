using System.Collections;
using System.Collections.Generic;
using System.Media;
using System.Security;
using UnityEngine;

public class Board : Object
{    
    public string boardName;
    public string gameMode;

    public Board (string boardName, string gameMode)
    {
        this.boardName = boardName;
        this.gameMode=gameMode;
    }

    static void Initialize_captainmap(string gameMode, int chosenMap)
    {
        //Avoir une database des maps avec iles avant
    }
    static void Initialize_FirstMateCard()
    {
        //Créer les systèmes et jauges avant
        Systems mine = new Systems(mine);
        Systems torpedo = new Systems(torpedo);
        Systems drone = new Systems(drone);
        Systems sonar = new Systems(sonar);
        Systems silence = new Systems(silence);
        Systems scenario = new Systems(scenario);
    }
    static void initialize_engineer(string gameMode)
    {
        //Créer les élements (radioactivité,…) avant
        //Le gameMode influe que pour faire surface je crois, ce serait bien de trouver autre chose qu'entourer le bateau parce que c'est pas pratique avec la souris

    }
    static void initialize_SeeThrough()
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
