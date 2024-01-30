using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EngineerBoardManager : MonoBehaviour
{
    #region attributes
    public EngineerDialData engineerDialDataW;
    public EngineerDialData engineerDialDataN;
    public EngineerDialData engineerDialDataS;
    public EngineerDialData engineerDialDataE;

    private Dictionary<EngineerDialData, UnityEngine.UI.Button[]> dialButtonsDictionary = new Dictionary<EngineerDialData, UnityEngine.UI.Button[]>();

    public Button dialButton;
    #endregion

    #region Initialisation
    void Start()
    {
        InitializeBoard(engineerDialDataW);
        InitializeBoard(engineerDialDataN);
        InitializeBoard(engineerDialDataS);
        InitializeBoard(engineerDialDataE);
    }

    private void InitializeBoard(EngineerDialData dialData)
    {
        // reference to panel's GameObject
        GameObject panel = GameObject.Find($"{dialData.name}Panel");

        UnityEngine.UI.Button[] buttons = panel.GetComponentsInChildren<UnityEngine.UI.Button>();
        dialButtonsDictionary[dialData] = buttons;
    }

    #endregion

    #region Click
    public void OnButtonClickW(int index)
    {
        OnButtonClick(engineerDialDataW, index);
    }

    public void OnButtonClickN(int index)
    {
        OnButtonClick(engineerDialDataN, index);
    }

    public void OnButtonClickS(int index)
    {
        OnButtonClick(engineerDialDataS, index);
    }

    public void OnButtonClickE(int index)
    {
        OnButtonClick(engineerDialDataE, index);
    }

    private void OnButtonClick(EngineerDialData dialData, int index)
    {
        UnityEngine.UI.Button currentButton = dialButtonsDictionary[dialData][index];

        // Check if the button is already disabled
        if (!currentButton.interactable)
        {
            Debug.Log("Button already disabled for dialData: " + dialData.name + ", index: " + index);
            return;
        }

        if (dialData.CrossThatFailure(index))
        {
            dialData.tab[index].failureFlag = true;

            currentButton.interactable = false;
            currentButton.GetComponent<UnityEngine.UI.Button>().interactable = false;


            Debug.Log("Button disabled for dialData: " + dialData.name + ", index: " + index);
        }
        else
        {
            Debug.Log("Button not disabled for dialData: " + dialData.name + ", index: " + index);
        }
    }


    #endregion

    #region Tests
    bool PathFull(int path)
    {
        bool full = true;
        foreach (var dialData in new[] { engineerDialDataW, engineerDialDataE, engineerDialDataS, engineerDialDataN })
        {
            for (int i = 0; i < dialData.tab.Length && full; i++)
            {
                if (dialData.tab[i].pathIndex == path && !dialData.tab[i].failureFlag)
                {
                    full = false;
                }
            }
        }
        return full;
    }

    public bool CheckColorFailure(string colorSystem)
    {
        bool result = false;
        foreach (var dialData in new[] { engineerDialDataW, engineerDialDataE, engineerDialDataS, engineerDialDataN })
        {
            for (int i = 0; i < dialData.tab.Length && !result; i++)
            {
                if (dialData.tab[i].color == colorSystem && dialData.tab[i].failureFlag)
                {
                    result = true;
                }
            }
        }
        return result; // True if failure, false if okay
    }
    #endregion 

    #region actions
    void ClearThePath(int path)
    {
        if (PathFull(path))
        {
            Debug.Log("Path Full");
            foreach (var dialData in new[] { engineerDialDataW, engineerDialDataE, engineerDialDataS, engineerDialDataN })
            {
                for (int i = 0; i < dialData.tab.Length; i++)
                {
                    if (dialData.tab[i].pathIndex == path)
                    {
                        dialData.tab[i].failureFlag = false;
                        //enable button again
                        dialButtonsDictionary[dialData][i].interactable = true;
                    }
                }
            }
        }
    }

    // If Surface
    void ClearDials()
    {
        foreach (var dialData in new[] { engineerDialDataW, engineerDialDataE, engineerDialDataS, engineerDialDataN })
        {
            for (int i = 0; i < dialData.tab.Length; i++)
            {
                dialData.tab[i].failureFlag = false;
                //enable button again
                dialButtonsDictionary[dialData][i].interactable = true;
            }
        }
    }

    //If full
    //Submarine submarine in parameter
    void ClearOneDial()
    {
        foreach (var dialData in new[] { engineerDialDataW, engineerDialDataE, engineerDialDataS, engineerDialDataN })
        {
            if (dialData.DialFull())
            {
                Debug.Log("dial Full");
                //submarine.TakeDamage(1);
                for (int i = 0; i < dialData.tab.Length; i++)
                {
                    dialData.tab[i].failureFlag = false;
                    dialButtonsDictionary[dialData][i].interactable = true;
                }
            }
        }
    }

    string MatchCourse (Captain.Direction course)
    {
        string courseStr = "";
        switch (course)
        {
            case Captain.Direction.North:
                courseStr = "north";
                break;
            case Captain.Direction.South:
                courseStr = "south";
                break;
            case Captain.Direction.East:
                courseStr = "east";
                break;
            case Captain.Direction.West:
                courseStr = "west";
                break;
        }
        return courseStr;
    }

    //Disable dials that aren't on the current course
    void FollowTheCourse(Captain.Direction course)
    {
        string nameDial=MatchCourse(course);
        foreach (var dialData in new[] { engineerDialDataW, engineerDialDataE, engineerDialDataS, engineerDialDataN })
        {
            if (dialData.name!=nameDial)
            {
                for (int i = 0; i < dialData.tab.Length; i++)
                {
                    dialButtonsDictionary[dialData][i].interactable = false;
                }
            }
        }
    }

    void FollowTheCourseMockUp(string course)
    {
        foreach (var dialData in new[] { engineerDialDataW, engineerDialDataE, engineerDialDataS, engineerDialDataN })
        {
            if (dialData.name != course)
            {
                for (int i = 0; i < dialData.tab.Length; i++)
                {
                    dialButtonsDictionary[dialData][i].interactable = false;
                }
            }
        }
    }

    #endregion

    void Update()
    {
        ClearThePath(1);
        ClearThePath(2);
        ClearThePath(3);
        ClearOneDial();
        FollowTheCourseMockUp("south");
    }

}