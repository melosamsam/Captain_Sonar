using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        //Connect to lobby
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        //Loads the Lobby Scene when finding the Photon Lobby
        SceneManager.LoadScene("Lobby");
        Debug.Log("Joined Lobby Scene");
    }
}
