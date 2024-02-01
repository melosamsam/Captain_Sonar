using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
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
    private Dictionary<EngineerDialData, Transform> crosses = new Dictionary<EngineerDialData, Transform>();

    public UnityEngine.UI.Button dialButton;
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


        // Fetch the crosses folder based on the panel
        Transform crossesFolder = panel.transform.Find("Crosses");
        crosses[dialData] = crossesFolder;

        // Erase crosses using the entire folder
        EraseCrosses(crossesFolder);
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

            Transform cross = crosses[dialData].GetChild(index);
            cross.gameObject.SetActive(true);



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
                        dialButtonsDictionary[dialData][i].interactable = true;

                        Transform cross = crosses[dialData].GetChild(i);
                        cross.gameObject.SetActive(false);
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

                Transform cross = crosses[dialData].GetChild(i);
                cross.gameObject.SetActive(false);
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

                    Transform cross = crosses[dialData].GetChild(i);
                    cross.gameObject.SetActive(false);
                }
            }
        }
    }

    string MatchCourse(Direction course)
    {
        string courseStr = "";
        switch (course)
        {
            case Direction.North:
                courseStr = "north";
                break;
            case Direction.South:
                courseStr = "south";
                break;
            case Direction.East:
                courseStr = "east";
                break;
            case Direction.West:
                courseStr = "west";
                break;
        }
        return courseStr;
    }

    //Disable dials that aren't on the current course
    void FollowTheCourse(Direction course)
    {
        string nameDial = MatchCourse(course);
        foreach (var dialData in new[] { engineerDialDataW, engineerDialDataE, engineerDialDataS, engineerDialDataN })
        {
            if (dialData.name != nameDial)
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
            if (dialData.name.ToLower() != course.ToLower())
            {
                for (int i = 0; i < dialData.tab.Length; i++)
                {
                    dialButtonsDictionary[dialData][i].interactable = false;
                }
            }
        }
    }

    private void EraseCrosses(Transform crossesFolder)
    {
        foreach (Transform cross in crossesFolder)
            cross.gameObject.SetActive(false);
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