using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class Systems : MonoBehaviour
{
    #region attributs
    public string NameSystem;
    private string ColourSystem; //for mechanic 
    private int[] QuotaJauge; //if jauge full, can't fill it anymore //warn opposite team if system is ready (full)
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
    void Awake()
    {
        Failure= false;
        string name = NameSystem;
        
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

    #region Fonctions test
    public bool GaugeFull()
    {
        bool result = true;
        for(int i=0;i<this.QuotaJauge.Length;i++)
        {
            if (this.QuotaJauge[i] == 0) {  result = false; break; }
        }
        return result;
    }

    bool No_Failure(string color)
    {
        bool result = true;
        //if mecano okay 
        if (ColourSystem==color && Failure)
        {
            result = false;
            //forbid the player from using the system
        }
        return result;
    }

    #endregion

    public void EmptyGauge()
    {
        for (int i = 0; i < this.QuotaJauge.Length; i++)
        {
            this.QuotaJauge[i] = 0;
        }
    }

    #region Fonctions systèmes
    bool initialize_Mine(Position pos)
    {
        bool retour = false;
        if (GaugeFull() && No_Failure("red"))
        {
            //alerte opponents mine was dropped 
                //have to wait for communication medium to be chosen

            //Position de dépôt de la mine(range de dépôt)
            //Marquer la position sur la map équipe dépôt

            //Vider Jauge
            EmptyGauge();
            retour = true;

        }
        return retour;
    }

    bool Torpedo(Position pos)
    {
        bool impact = false;
        if (GaugeFull() && No_Failure("red"))
        {
            //Position de lancement de la torpille (range de dépôt)
            //Marquer la position sur la map équipe dépôt
            //Bool impact = Activate_Red (position lancement, position_submarine_Gentil, position_submarine_ennemi )
            //Vider Jauge
            EmptyGauge();
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

    void Activate_Silence()
    {
        if(GaugeFull() && No_Failure("yellow")){
            //Faire un move (entre 0 et 4 unidirectionnel) je pense pas c’est à nous de gérer et plutôt on appelle une autre fonction de déplacement
            //Vider Jauge
            EmptyGauge();
        }
    }

    void Sonar() 
    {
        if(GaugeFull() && No_Failure("green"))
        {
            //Appel Fonction qui demande à l’autre équipe de choisir ses coordonnées et check si une est bonne et l’autre fausse
            //vider la jauge
            EmptyGauge() ;
        }
    }

    bool Drone() 
    {
        bool used = false;
        if (GaugeFull() && No_Failure("green"))
        {
            //bool bon secteur deviné ? je pense pas vu que ca c’est juste eux a prendre en note si oui ou non parce que non peut les aider autant que oui
            //pas de verif que info rep bonne vu que dans chat vocal
            //Aaah c’est vrai. Triche trop facile en fait
            //Vider la jauge
            EmptyGauge();

        }
        return used; 

    }
    #endregion

    #region Fonctions start & update
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion
}
