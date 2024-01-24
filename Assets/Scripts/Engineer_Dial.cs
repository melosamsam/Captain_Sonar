using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class Engineer_Dial : MonoBehaviour
{
    #region attributs
    public string nameDial;
    private object [,] systems; 
    //Format is [5,4] with a int (id), a string, a bool and an int
    //string is either a system color or radioactivity, bool is whether there is failure, and int for the path on which the object is (if any) to cancel the failures
    #endregion

    #region Get
    public string GetNameDial() { return nameDial; }
    public object[,] GetSystemsInDial() { return systems; }
    #endregion

    #region Set
    public void SetNameDial(string name) { nameDial = name; }
    public void SetSystemsInDial(object[,] systemsInDial) { systems = systemsInDial; }
    #endregion

    #region Constructeurs
    void Awake()
    {
        string name = nameDial;

        if (name == "west")
        {
            systems = new object[,]
            {
                {0, "red", false, 1 },
                {1,"yellow", false, 1 },
                {2, "green", false, 1 },
                {3, "radioactivity", false, 0 },
                {4, "radioactivity", false, 0 }
            };
        }
        if (name == "north")
        {
            systems = new object[,]
            {
                {0, "yellow", false, 2 },
                {1, "red", false, 2 },
                {2, "green", false, 2 },
                {3, "yellow", false, 0 },
                {4, "radioactivity", false, 0 }
            };
        }
        if (name == "south")
        {
            systems = new object[,]
            {
                {0, "green", false, 3 },
                {1, "yellow", false, 3 },
                {2, "red", false, 3 },
                {3, "red", false, 0 },
                {4, "radioactivity", false, 0 }
            };
        }
        if (name == "east")
        {
            systems = new object[,]
            {
                {0, "green", false, 2},
                {1, "yellow", false, 3 },
                {2, "red", false, 1 },
                {3, "radioactivity", false, 0 },
                {4, "green", false, 0 }
            };
        }
    }
    #endregion


    #region Fonctions test
    //If every dial retruns true, submarine takes a damage
    public bool DialFull()
    {
        bool result = false;
        for (int i = 0;i < systems.GetLength(0) && !result; i++) 
        { 
            if ((bool)systems[i,2]==true) { result = true;}
        }
        return result;
    }
    
    /// Check if  all the radioactives object of this dial have suffered from failure. Returns a boolea
    bool ThisDialRadioactive()
    {
        bool result = true;
        for (int i=0; i<systems.GetLength(0) && result; i++)
        {
            if ((string)systems[i,1]=="radioactivity" && (bool)systems[i,2]==false)
            {
                result = false;
            }
        }
        return result;
    }

    //From Systems class, should check the color_system and see if you can activate it (check all dials)
    bool CheckColorFailure(string color)
    {
        bool result = false;
        for (int i = 0; i < systems.GetLength(0) && !result; i++)
        {
            if ((string)systems[i, 1] == color && (bool)systems[i, 2] == true)
            {
                result = true;
            }
        }
        return result; //True if failure, false if okay
    }

    //Should replace id by the unity game object/button on the board
    //(Can I cross that case?) Returns true if not yet failure
    bool CrossThatFailure(int id)
    {
        return !(bool)systems[id, 2];
    }

    #endregion

    void CrossFailure(int id)
    {
        if (CrossThatFailure(id))
        {
            systems[id, 2] = true;
            //Do something to the board
        }
        //Else send a message that tells the engineer they already crossed that case
    }

    void ClearThePath(int path)
    {
        //If all cases of the path are crossed
        for (int i=0; i<systems.GetLength(0); i++)
        {
            if ((int)systems[i,3] == path) systems[i, 2]= false;
        }
    }

    void ClearDial()
    {
        //If Surface (Do this to all dial
        for (int i = 0; i < systems.GetLength(0); i++)
        {
            systems[i, 2] = false;
        }
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
