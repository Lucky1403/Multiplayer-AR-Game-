using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Photon.Realtime;
using TMPro;

public class SpinningTopsGameManager : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    public GameObject UI_InformPanelGameobject;
    public TextMeshProUGUI UI_InformText;
    public GameObject searchForGamesButtonGameObject;
    // Start is called before the first frame update
    void Start()
    {
        UI_InformPanelGameobject.SetActive(true);
        UI_InformText.text = "Search for Games to BATTLE!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region UI Callback Methods

    public void JoinRandomRoom()
    {
        UI_InformText.text = "Searching for Available Rooms....";
        PhotonNetwork.JoinRandomRoom();
        searchForGamesButtonGameObject.SetActive(false);

    }
    #endregion

    #region Photon Callback Methods

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        UI_InformText.text = message;
        CreateAndJoinRoom();
    }

    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            UI_InformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name + " .Waiting for the Other Players....";
        }
        else
        {
            UI_InformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name;
            StartCoroutine(DeactivateAfterSeconds(UI_InformPanelGameobject, 2.0f));
        }
        Debug.Log(PhotonNetwork.NickName + "Joined to " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + "Joined to " + PhotonNetwork.CurrentRoom.Name + " Player count " + PhotonNetwork.CurrentRoom.PlayerCount);
        UI_InformText.text = newPlayer.NickName + "Joined to " + PhotonNetwork.CurrentRoom.Name + " Player count " + PhotonNetwork.CurrentRoom.PlayerCount;
        
        StartCoroutine(DeactivateAfterSeconds(UI_InformPanelGameobject, 2.0f));
    }
    #endregion

    #region Unity Private Methods
    private void CreateAndJoinRoom()
    {
        string RandomRoomName = "Room " + UnityEngine.Random.Range(0,1000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(RandomRoomName, roomOptions);
    }

    IEnumerator DeactivateAfterSeconds(GameObject gameObject, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }
    #endregion
}