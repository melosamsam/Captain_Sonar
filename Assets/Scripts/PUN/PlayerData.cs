using Photon.Pun;
using Photon.Voice.Unity;
using UnityEngine;

public class PlayerData : MonoBehaviourPun
{
    public Enums.Team Team;
    public string Nickname = "P0";
    public int score = 0;

    private Recorder voiceRecorder;
    private Speaker voiceSpeaker;

    private Color myButtonColor = Color.white;
    void Start()
    {
        if (photonView.IsMine)
        {
            AssignTeam();
            InitializeVoiceChat();
        }
    }
    void InitializeVoiceChat()
    {
        voiceRecorder = gameObject.AddComponent<Recorder>();
        voiceRecorder.TransmitEnabled = false; // Disable by default

        voiceSpeaker = gameObject.AddComponent<Speaker>();
        voiceSpeaker.enabled = false; // Disable by default

    }
    [PunRPC]
    void SyncVoiceChatState(bool enable)
    {
        // Synchronize the voice chat state to other players
        voiceSpeaker.enabled = enable;
    }
    public void ToggleVoiceChat(bool enable)
    {
        // Enable or disable voice chat based on the input
        if (photonView.IsMine)
        {
            voiceRecorder.TransmitEnabled = enable;
            voiceSpeaker.enabled = enable;

            // You can also broadcast this information to teammates
            photonView.RPC("SyncVoiceChatState", RpcTarget.Others, enable);
        }
    }
    [PunRPC]
    void SetTeamRPC(Enums.Team newTeam)
    {
        Team = newTeam;
    }

    [PunRPC]
    void SyncTeamRPC(Enums.Team newTeam)
    {
        Team = newTeam;
    }

    [PunRPC]
    public void SetColorRPC(Color newColor)
    {
        myButtonColor = newColor;
        UpdateColorLocally();
    }

    public void ChangeColorLocally(Color newColor)
    {
        myButtonColor = newColor;
        UpdateColorLocally();

        if (photonView.IsMine)
        {
            photonView.RPC("SyncColorRPC", RpcTarget.Others, newColor);
        }
    }

    [PunRPC]
    void SyncColorRPC(Color newColor)
    {
        myButtonColor = newColor;
        UpdateColorLocally();
    }

    void UpdateColorLocally()
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material.color = myButtonColor;
        }
        else
        {
            Debug.LogError("Renderer component not found on the object.");
        }
    }

    public void ChangeEveryoneButtonColor(Color newColor)
    {
        myButtonColor = newColor;
        UpdateColorLocally();

        if (photonView.IsMine)
        {
            photonView.RPC("SyncEveryoneButtonColor", RpcTarget.Others, newColor);
        }
    }

    [PunRPC]
    void SyncEveryoneButtonColor(Color newColor)
    {
        myButtonColor = newColor;
        UpdateColorLocally();
    }

    // New method to set the team locally and synchronize it across the network
    public void SetTeamAndSync(Enums.Team newTeam)
    {
        SetTeamRPC(newTeam);

        if (photonView.IsMine)
        {
            photonView.RPC("SyncTeamRPC", RpcTarget.Others, newTeam);
        }
    }

    

    void AssignTeam()
    {
        // Generate a random team for the player
        Enums.Team newTeam = (Enums.Team)Random.Range(0, 2); // Assumes Enums.Team has two values (0: Red, 1: Blue)
        Debug.Log("new team i :" + newTeam.ToString());
        // Set the team locally
        SetTeamAndSync(newTeam);
    }
}
