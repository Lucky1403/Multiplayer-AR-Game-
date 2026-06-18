using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Login UI")]
    public InputField playerNameInputField;
    #region UNITY Methods
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region UI Callback methods

    public void OnEnterGameButtonClicked()
    {
        string playername = playerNameInputField.text;

        if(! string.IsNullOrEmpty(playername))
        {
            if(!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playername;
                
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        else
        {
            Debug.Log("Player name is invalid or empty!!");
        }
    }
    #endregion

    #region Photon Callback Methods

    public override void OnConnected()
    {
        Debug.Log("We connected to Internet");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon Server.");
    } 
    #endregion
}
