using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class Systems : MonoBehaviour
{
    #region attributs
    private string NameSystem;
    private string ColourSystem; //for mechanic 
    private int[] QuotaJauge; //if jauge full, can't fill it anymore 
    //warn opposite team if system is ready (full)
    private bool Failure; //if mechanic circuit has crossed out colour
    #endregion

    #region Get
    public string GetNameSystem() {  return NameSystem; }
    public string GetColourSystem() {  return ColourSystem; }
    public int[] GetQuotaJauge() { return QuotaJauge; }
    public bool GetFailure() { return Failure; }
    #endregion

    #region Set
    public void SetNameSystem(string name) { NameSystem = name; }
    public void SetColourSystem(string colourSystem) {  ColourSystem = colourSystem; }
    public void SetQuotajauge(int[] quota) {  QuotaJauge = quota; }
    public void SetFailure(bool fail) { Failure = fail; }
    #endregion

    #region Constructeurs
    public Systems(string name)
    {
        Failure= false;
        NameSystem = name;
        
        
        if(name == "torpedo" || name == "mine")
        {
            ColourSystem = "red";
            QuotaJauge = new int[3];
        }
        else
        {
            if (name == "silence" || name == "scenario")
            {
                ColourSystem = "yellow";
                QuotaJauge = new int[6];
            }
            else
            {
                if (name == "drone")
                {
                    ColourSystem = "green";
                    QuotaJauge = new int[3];
                }
                else
                {
                    if (name == "sonar")
                    {
                        ColourSystem = "green";
                        QuotaJauge = new int[4];
                    }
                }
            }
        }

    }
    #endregion

    
    //pas sur qu'on a un systeme en parametre mais plutot qu'on devraut utiliser celui-ci
    static bool CheckJauge(Systems chosensystem)
    {
        bool result = true;
        for(int i=0;i<chosensystem.GetQuotaJauge().Length;i++)
        {
            if (chosensystem.GetQuotaJauge()[i] == 0) {  result = false; break; }
        }
        return result;
    }

    //a completer
    //!!! Voir si static est nécessaire pour ces fonctions !!!

    bool No_Failure(string color)
    {
        bool result = true;
        //if mecano okay 
        if (ColourSystem==color && Failure)
        {
            result = false; 
            //empêche joueur d'activer le system
        }
        return result;
    }

    //creer class position nan ?
    //mauvaise manipulation de position
    //pas sur qu'on a un systeme en parametre mais plutot qu'on devraut utiliser celui-ci
    Position initialize_Mine(Position pos, Systems mine)
    {
        Position retour = new Position(-1, -1);
        if (CheckJauge(mine) && No_Failure("red"))
        {
            //alerte opponents mine was dropped 
            //Position de dépôt de la mine(range de dépôt)
            //Marquer la position sur la map équipe dépôt
            //Vider Jauge

        }
        return retour;
    }

    bool Torpedo(Position pos, Systems torpedo)
    {
        bool impact = false;
        if (CheckJauge(torpedo) && No_Failure("red"))
        {
            //Position de lancement de la torpille (range de dépôt)
            //Marquer la position sur la map équipe dépôt
            //Bool impact = Activate_Red (position lancement, position_submarine_Gentil, position_submarine_ennemi )
            //Vider Jauge

        }
        return impact;
    }

    static bool Activate_Red(Position posimpact, Position possubgentil, Position possubennemi) 
    {
        bool activated = false;
        //Alerte les 2 équipes de l’activation
        //Range d’explosion
        //Bool impact selon positions impact et positions des 2 submarines
        //Alerte si impact ou pas
        return activated;
    }

    void Activate_Silence(Systems silence)
    {
        if(CheckJauge(silence) && No_Failure("yellow")){
            //Faire un move (entre 0 et 4 unidirectionnel) je pense pas c’est à nous de gérer et plutôt on appelle une autre fonction de déplacement
            //Vider Jauge

        }
    }

    void Sonar(Systems sonar) 
    {
        if(CheckJauge(sonar) && No_Failure("green"))
        {
            //Appel Fonction qui demande à l’autre équipe de choisir ses coordonnées et check si une est bonne et l’autre fausse
            //vider la jauge
        }
    }

    bool Drone(Systems drone) 
    {
        bool used = false;
        if (CheckJauge(drone) && No_Failure("green"))
        {
            //bool bon secteur deviné ? je pense pas vu que ca c’est juste eux a prendre en note si oui ou non parce que non peut les aider autant que oui
            //pas de verif que info rep bonne vu que dans chat vocal
            //Aaah c’est vrai. Triche trop facile en fait
            //Vider la jauge

        }
        return used; 

    } 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
