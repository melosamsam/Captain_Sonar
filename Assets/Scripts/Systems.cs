using System;
using System.Collections.Generic;
using UnityEngine;

public class Systems : MonoBehaviour
{
    #region Attributs

    public string NameSystem;
    private string ColourSystem; //for mechanic 
    [SerializeField] private int[] QuotaJauge; //if jauge full, can't fill it anymore //warn opposite team if system is ready (full)
    //I am thinking the failure attribute shouldn't be in this class

    [SerializeField] private List<GameObject> GaugeList;
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

    #region Unity method
    void Awake()
    {
        Failure= false;
        string name = NameSystem;
        
        switch (name)
        {
            case "mine":
                ColourSystem = "red";
                QuotaJauge = new int[2];
                break;

            case "torpedo":
                ColourSystem = "red";
                QuotaJauge = new int[3];
                break;

            case "drone":
                ColourSystem = "green";
                QuotaJauge = new int[3];
                break;

            case "sonar":
                ColourSystem = "green";
                QuotaJauge = new int[2];
                break;

            case "silence" or "scenario":
                ColourSystem = "yellow";
                QuotaJauge = new int[5];
                break;

        }

        Transform crosses = transform.Find("Crosses");
        GaugeList = new List<GameObject>();
        for (int i = 0; i < crosses.childCount; i++)
            GaugeList.Add(crosses.GetChild(i).gameObject);

        EraseCrosses();
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

    bool NoFailure(string color)
    {
        bool result = true;
        //If mecano ok
        //if (!westDial.CheckColorFailure(color) && !northDial.CheckColorFailure(color) && !southDial.CheckColorFailure(color) && !eastDial.CheckColorFailure(color))
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
        int index = Array.LastIndexOf(QuotaJauge, 1);
        QuotaJauge[index] = 0;
        GaugeList[index].SetActive(false);
    }

    public void FillGauge()
    {
        if (!GaugeFull()) {
            int i = Array.IndexOf(QuotaJauge, 0, 0);
                
            QuotaJauge[i] = 1;
            GaugeList[i].SetActive(true);
        }
    }

    private void EraseCrosses()
    {
        foreach (GameObject cross in GaugeList)
            cross.SetActive(false);
    }

    #region Fonctions syst�mes
    //Je crois qu'on devrait prendre le gentil submarine en param�tre
    bool initialize_Mine(Position positionDrop, Submarine goodSubmarine)
    {
        bool dropped = false;
        if (GaugeFull() && NoFailure("red"))
        {
            int from = goodSubmarine.CurrentPosition.x + goodSubmarine.CurrentPosition.y;
            int chosenPosition = positionDrop.x + positionDrop.y;
            //Position de d�p�t de la mine(range de d�p�t)
            if (Math.Abs(from-chosenPosition)<=4) 
            {
                dropped = true;
                EmptyGauge();
                UnityEngine.Debug.Log("Mine was dropped."); //To both team or specified message "you dropped a mine in Position" to goodSubmarine
                //Marquer la position sur la map �quipe d�p�t
            }
            else UnityEngine.Debug.Log("The chosen position is out of range.");

        }
        else UnityEngine.Debug.Log("system can\'t be used. Please check gauge and failures.");
        return dropped;
    }

    bool Torpedo(Position positionDrop, Submarine goodSubmarine, Submarine enemy)
    {
        bool impact = false;
        if (GaugeFull() && NoFailure("red"))
        {
            int from = goodSubmarine.CurrentPosition.x + goodSubmarine.CurrentPosition.y;
            int chosenPosition = positionDrop.x + positionDrop.y;
            //Position de lancement de la torpille (range de d�p�t)
            if (Math.Abs(from- chosenPosition) <=4) 
            {
                impact = Activate_Red(positionDrop, goodSubmarine, enemy);
                EmptyGauge();
            }
            else UnityEngine.Debug.Log("The chosen position is out of range.");
        }
        else UnityEngine.Debug.Log("system can\'t be used. Please check gauge and failures.");
        return impact;
    }

    static bool Activate_Red(Position posimpact, Submarine goodSubmarine, Submarine enemy) 
    {
        bool impact = false;

        //Alerte les 2 �quipes de l�activation
        //Range d�explosion

        int to = enemy.CurrentPosition.x + enemy.CurrentPosition.y;
        int from = goodSubmarine.CurrentPosition.x + goodSubmarine.CurrentPosition.y;
        int chosenPosition = posimpact.x + posimpact.y;

        if (posimpact==enemy.CurrentPosition)
        {
            enemy.TakeDamage(2);
            impact=true;
            UnityEngine.Debug.Log("Your aim is incredible. Enemy suffered two damages."); //To goodSubmarine
            UnityEngine.Debug.Log("The enemy aimed right at you. You suffered two damages."); //To enemy
        }
        else if (Math.Abs(to- chosenPosition) <=2) 
        {
            enemy.TakeDamage();
            impact=true;
            UnityEngine.Debug.Log("You touched the enemy. Enemy suffered one damage."); //To goodSubmarine
            UnityEngine.Debug.Log("The enemy managed to touch you. You suffered one damage."); //To enemy
        }

        if (posimpact == goodSubmarine.CurrentPosition)
        {
            goodSubmarine.TakeDamage(2);
            impact = true;
            UnityEngine.Debug.Log("Man... You shot yourself... You took two damages (and I think you deserve much more)."); //To goodSubmarine
            UnityEngine.Debug.Log("Jeez that other team really be playing Among us. They took two damages from their own attack."); //To enemy
        }
        else if (Math.Abs(from - chosenPosition) <= 2)
        {
            goodSubmarine.TakeDamage();
            impact = true;
            UnityEngine.Debug.Log("Man... You shot yourself... You took one damage (and I think you deserve much more)."); //To goodSubmarine
            UnityEngine.Debug.Log("Jeez that other team really be playing Among us. They took one damage from their own attack."); //To enemy
        }

        else
        {
            UnityEngine.Debug.Log("Gotta get a better aim man"); //To goodSubmarine
            UnityEngine.Debug.Log("Lucky you, the other team needs glasses (and a brain)"); //To enemy
        }

        return impact;
    }

    void Activate_Silence()
    {
        if(GaugeFull() && NoFailure("yellow")){
            //Faire un move (entre 0 et 4 unidirectionnel) je pense pas c�est � nous de g�rer et plut�t on appelle une autre fonction de d�placement
            //Vider Jauge
            EmptyGauge();
        }
    }

    void Sonar() 
    {
        if(GaugeFull() && NoFailure("green"))
        {
            //Appel Fonction qui demande � l�autre �quipe de choisir ses coordonn�es et check si une est bonne et l�autre fausse
            //vider la jauge
            EmptyGauge() ;
        }
    }

    bool Drone(int sector, Submarine enemy) 
    {
        bool used = false;
        if (GaugeFull() && NoFailure("green"))
        {
            //bool bon secteur devin� ? je pense pas vu que ca c�est juste eux a prendre en note si oui ou non parce que non peut les aider autant que oui
            //pas de verif que info rep bonne vu que dans chat vocal
            //Aaah c�est vrai. Triche trop facile en fait
            //Vider la jauge
            EmptyGauge();

        }
        else UnityEngine.Debug.Log("system can\'t be used. Please check gauge and failures.");
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
