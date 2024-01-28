using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class UIManager : MonoBehaviourPunCallbacks
{
    public TMP_Text redSubmarinePlayersText;
    public TMP_Text blueSubmarinePlayersText;
    private PhotonView _photonView;

    private Submarine redSubmarine;
    private Submarine blueSubmarine;
    private void Update()
    {
        // Only the master client updates the submarines
        if (PhotonNetwork.IsMasterClient)
        {
            redSubmarine = GetSubmarineByName("RedTeam");
            blueSubmarine = GetSubmarineByName("BlueTeam");

            // Call an RPC to update the text on all clients
            photonView.RPC("UpdateSubmarineText", RpcTarget.All, GetPlayersText(redSubmarine), GetPlayersText(blueSubmarine));
        }
    }

    [PunRPC]
    private void UpdateSubmarineText(string redText, string blueText)
    {
        // Update the text on all clients
        redSubmarinePlayersText.text = redText;
        blueSubmarinePlayersText.text = blueText;
    }

    private Submarine GetSubmarineByName(string submarineName)
    {
        GameObject submarineObject = GameObject.Find(submarineName);
        if (submarineObject != null)
        {
            return submarineObject.GetComponent<Submarine>();
        }
        return null;
    }

    private string GetPlayersText(Submarine submarine)
    {
        if (submarine != null)
        {
            List<Player> players = submarine.Players;

            if (players != null)
            {
                string playersText = "Players: ";

                foreach (Player player in players)
                {
                    if (player != null)
                    {
                        playersText += player.Name + ", ";
                    }
                }

                return playersText.TrimEnd(',', ' ');
            }
            else
            {
                return "Players list is null";
            }
        }
        else
        {
            return "Submarine is null";
        }
    }


    public void ChooseSubmarineButtonClicked(string submarineName)
    {
        photonView.RPC("RPC_ChooseSubmarine", RpcTarget.All, photonView.ViewID, submarineName);
    }

    [PunRPC]
    private void RPC_ChooseSubmarine(int playerViewID, string submarineName)
    {
        PhotonView playerPhotonView = PhotonView.Find(playerViewID);

        if (playerPhotonView != null)
        {
            Player player = playerPhotonView.GetComponent<Player>();

            if (player == null)
            {
                // If the Player component doesn't exist, add it
                player = playerPhotonView.gameObject.AddComponent<Player>();
                Debug.Log($"RPC_ChooseSubmarine: Added Player component to {playerPhotonView.gameObject.name}");
            }

            Debug.Log($"RPC_ChooseSubmarine: Player {player.Name} is choosing submarine {submarineName}");

            Submarine submarine = GetSubmarineByName(submarineName);

            if (submarine != null)
            {
                Debug.Log($"RPC_ChooseSubmarine: Submarine {submarine.Name} found. Assigning to {player.Name}");
                player.AssignSubmarine(submarine);
            }
            else
            {
                Debug.LogWarning($"RPC_ChooseSubmarine: Submarine {submarineName} not found!");
            }
        }
        else
        {
            Debug.LogWarning($"RPC_ChooseSubmarine: Player PhotonView not found for ID {playerViewID}");
        }
    }

}
