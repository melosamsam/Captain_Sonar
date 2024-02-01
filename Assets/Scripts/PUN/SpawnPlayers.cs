using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviourPun
{
    public GameObject buttonPrefab;
    public float minx;
    public float maxx;
    public float z;

    private float buttonSpacingY = 1.0f;
    private List<GameObject> spawnedButtons = new List<GameObject>();
    private Enums.Team playerTeam;

    private void Start()
    {
        if (photonView != null)
        {
            if (buttonPrefab != null)
            {
                SpawnButtonAtPosition(Vector3.zero, playerTeam);
                SpawnButtonAtPosition(new Vector3(-5.75f, GetNextButtonYPosition()), Enums.Team.Red);

                if (PhotonNetwork.IsMasterClient)
                {
                    SpawnButtonAtPosition(new Vector3(5.75f, GetNextButtonYPosition()), playerTeam);
                }
            }
            else
            {
                Debug.LogError("Button prefab is not assigned in the inspector.");
            }
        }
        else
        {
            Debug.LogError("PhotonView is not properly initialized.");
        }
    }

    void SpawnButtonAtPosition(Vector3 position, Enums.Team team)
    {
        if (buttonPrefab != null)
        {
            GameObject spawnedButton = PhotonNetwork.Instantiate(buttonPrefab.name, position, Quaternion.identity);
            PlayerData playerData = spawnedButton.GetComponent<PlayerData>();

            if (playerData != null)
            {
                playerData.Team = team;
                photonView.RPC("SetTeamRPC", RpcTarget.AllBuffered, team);
            }
            else
            {
                Debug.LogError("Spawned button is missing PlayerData component.");
            }

            spawnedButtons.Add(spawnedButton);
        }
        else
        {
            Debug.LogError("Button prefab is not assigned in the inspector.");
        }
    }

    [PunRPC]
    void SetTeamRPC(Enums.Team newTeam)
    {
        playerTeam = newTeam;
    }

    float GetNextButtonYPosition()
    {
        return spawnedButtons.Count * buttonSpacingY;
    }
}
