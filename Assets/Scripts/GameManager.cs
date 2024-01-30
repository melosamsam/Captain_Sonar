using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject submarinePrefab;
    public GameObject playerPrefab;

    private GameObject submarine1;
    private GameObject submarine2;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            submarine1 = PhotonNetwork.Instantiate(submarinePrefab.name, Vector3.zero, Quaternion.identity);
            submarine2 = PhotonNetwork.Instantiate(submarinePrefab.name, Vector3.zero, Quaternion.identity);
            submarine1.name = "RedTeam";
            submarine1.GetComponent<Submarine>().Name = "RedTeam";
            submarine2.name = "BlueTeam";
            submarine1.GetComponent<Submarine>().Name = "BlueTeam";
            //InstantiatePlayer();
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        InstantiatePlayer();
    }

    void InstantiatePlayer()
    {
        GameObject playerObject = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        Player playerScript = playerObject.GetComponent<Player>();
        playerScript.Name = "SomePlayerName";
    }
}
