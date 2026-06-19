using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Login UI")]
    public InputField playerNameInputField;
    public GameObject UI_loginGameObject;

    [Header("Lobby UI")]
    public GameObject UI_lobbyGameObject;
    public GameObject UI_3dGameObject;

    [Header("Connection Status UI")]
    public GameObject UI_ConnectionStatusGameObject;
    public Text ConnectionStatusText;
    public bool showConnectionStatus = false;
    #region UNITY Methods
    // Start is called before the first frame update
    void Start()
    {
        UI_lobbyGameObject.SetActive(false);
        UI_3dGameObject.SetActive(false);
        UI_ConnectionStatusGameObject.SetActive(false);

        UI_loginGameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (showConnectionStatus)
        {
            ConnectionStatusText.text = "Connection Status: " + PhotonNetwork.NetworkClientState;

        }
    }
    #endregion

    #region UI Callback methods

    public void OnEnterGameButtonClicked()
    {
        string playername = playerNameInputField.text;

        if (!string.IsNullOrEmpty(playername))
        {
            UI_lobbyGameObject.SetActive(false);
            UI_3dGameObject.SetActive(false);
            UI_loginGameObject.SetActive(false);

            showConnectionStatus = false; //-> Isko true kardenge toh UI mai connection status dikhne lagega abhi off kar rakha hai kyunki font sahi nahi lag raha hai. Chahe to isko true karke status ko UI mai dikha sakte hai
            UI_ConnectionStatusGameObject.SetActive(true);
            if (!PhotonNetwork.IsConnected)
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

    public void OnQuickMatchButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_PlayerSelection");
    }
    #endregion

    #region Photon Callback Methods

    public override void OnConnected()
    {
        Debug.Log("We connected to Internet");
    }

    public override void OnConnectedToMaster()
    {
        UI_ConnectionStatusGameObject.SetActive(false);
        UI_loginGameObject.SetActive(false);

        UI_3dGameObject.SetActive(true);
        UI_lobbyGameObject.SetActive(true);
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon Server.");
    }
    #endregion
}
