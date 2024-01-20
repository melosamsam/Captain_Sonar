using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class Button : MonoBehaviourPun
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on the object.");
            return;
        }

        // Register UnityEngine.Color type for network synchronization
        PhotonPeer.RegisterType(typeof(Color), (byte)'C', ColorSerialization.SerializeColor, ColorSerialization.DeserializeColor);

        // Assign a unique ID to each player (for testing purposes)
        int playerId = Random.Range(1, 1000);

        // Initialize color for all players
        photonView.RPC(nameof(RPC_InitializeColor), RpcTarget.AllBuffered, spriteRenderer.color, playerId);
    }

    private void OnMouseDown()
    {
        Debug.Log("Mouse Clicked on Circle!");
        ChangeColor();
        photonView.RPC(nameof(RPC_UpdateColor), RpcTarget.All, spriteRenderer.color);
    }

    void ChangeColor()
    {
        Debug.Log("Changing Color!");
        Color currentColor = spriteRenderer.color;

        // Example logic for changing color
        if (currentColor.Equals(Color.red))
        {
            spriteRenderer.color = Color.green;
        }
        else if (currentColor.Equals(Color.green))
        {
            spriteRenderer.color = Color.blue;
        }
        else if (currentColor.Equals(Color.blue))
        {
            spriteRenderer.color = Color.red;
        }
        else
        {
            spriteRenderer.color = Color.red;
        }
    }

    [PunRPC]
    private void RPC_InitializeColor(Color initialColor, int playerId)
    {
        spriteRenderer.color = initialColor;

        // Log the initialization for debugging
        Debug.Log($"Player {playerId} initialized color to {initialColor}");
    }

    [PunRPC]
    private void RPC_UpdateColor(Color newColor)
    {
        spriteRenderer.color = newColor;
    }
}
