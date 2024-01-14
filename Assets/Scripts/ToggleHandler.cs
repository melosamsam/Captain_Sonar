using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleHandler : MonoBehaviour
{
    public Toggle toggle1;
    public Toggle toggle2;
    public Toggle toggle3;
    public Toggle toggle4;

    private GameObject playerObject;
    private Player playerScript;

    // Start is called before the first frame update
    void Start()
    {
        // Assuming your Player script is attached to a GameObject named "TestPlayer"
        playerObject = GameObject.Find("TestPlayer");

        if (playerObject != null)
            playerScript = playerObject.GetComponent<Player>();
        else
            Debug.LogError("TestPlayer GameObject not found!");

        // Add listeners to toggles
        toggle1.onValueChanged.AddListener((value) => ToggleValueChanged("Captain", value));
        toggle2.onValueChanged.AddListener((value) => ToggleValueChanged("First Mate", value));
        toggle3.onValueChanged.AddListener((value) => ToggleValueChanged("Engineer", value));
        toggle4.onValueChanged.AddListener((value) => ToggleValueChanged("Radio Detector", value));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ToggleValueChanged(string role, bool value)
    {
        if (playerScript != null)
        {
            if (value)
            {
                playerScript.AssignRole(role);
            }
            else
            {
                playerScript.RemoveRole(role);
            }
        }
    }
}
