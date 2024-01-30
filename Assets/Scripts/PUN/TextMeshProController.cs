using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMeshProController : MonoBehaviour
{
    public PlayerData player; // Reference to the PlayerData script
    private TextMeshPro textMeshPro;

    void Start()
    {
        Debug.Log("Initial Time Scale: " + Time.timeScale);
        // Attempt to get the TextMeshPro component
        textMeshPro = GetComponent<TextMeshPro>();

        // Check if the TextMeshPro component is found
        if (textMeshPro != null)
        {
            player = GetComponentInParent<PlayerData>();

            // Set default values
            textMeshPro.color = Color.black;
            textMeshPro.text = "playername";

            // Update the text
            UpdateText();
        }
        else
        {
            // Log a warning if TextMeshPro is not found
            Debug.LogWarning("TextMeshPro component not found on GameObject: " + gameObject.name);
        }
    }

    void Update()
    {
        Debug.Log("U Time Scale: " + Time.timeScale);
        UpdateText();
    }
    private void OnMouseDown()
    {
        player.Team = Enums.Team.Blue;

    }
    // Update is called once per frame
    void UpdateText()
    {
        if (player != null)
        {
            TextMeshPro textMeshPro = GetComponent<TextMeshPro>(); // Get TextMeshPro component again

            if (textMeshPro != null)
            {
                textMeshPro.text = "Name: " + player.Nickname.ToString();
                textMeshPro.color = (player.Team == Enums.Team.Red) ? Color.red : Color.blue;
            }
        }
    }
}
