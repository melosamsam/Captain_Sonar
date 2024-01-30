using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerNameInputField : MonoBehaviour
{
    const string playerNamePrefKey = "PlayerName";
    void Start()
    {
        string defaultname = string.Empty;
        InputField inputf=this.GetComponent<InputField>();
        if (inputf != null)
        {
            if(PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultname=PlayerPrefs.GetString(playerNamePrefKey);
                inputf.text= defaultname;
            }
        }
    }

    public void SetPlayerName(string value)
    {
        // #Important
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }
        PhotonNetwork.NickName = value;

        PlayerPrefs.SetString(playerNamePrefKey, value);
    }
    
}
