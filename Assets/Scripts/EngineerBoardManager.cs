using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EngineerBoardManager : MonoBehaviour
{
    public EngineerDialData engineerDialData;
    public Button[] buttons;

    public Button dialButton;

    void Start()
    {
        InitializeBoard();
    }

    void InitializeBoard()
    {
        buttons = new Button[engineerDialData.tab.Length];
        for (int i = 0; i < engineerDialData.tab.Length; i++)
        {
            // Capture the current value of i in a local variable
            int currentIndex = i;

            Button button = Instantiate(GetButtonPrefab(engineerDialData.tab[i].color), transform);

            buttons[i] = button;
            button.onClick.AddListener(() => OnButtonClick(currentIndex));
        }
    }

    Button GetButtonPrefab(string color)
    {
        string prefabName = color + "Button";
        return Resources.Load<Button>("Path/To/" + prefabName);
    }

    void OnButtonClick(int i)
    {
        engineerDialData.tab[i].failureFlag = true;
        // You might want to update UI here or perform other actions
    }
}
