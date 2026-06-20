using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerSelectionManager : MonoBehaviour
{
    public Transform playerSwitcherTransform;
    public GameObject[] spinnerTopModels;
    public int playerSelectionNumber;

    [Header("UI")]
    public Button nextButton;
    public Button PreviousButton;
    public TextMeshProUGUI playerModelType_Text;
    public GameObject UI_selection;
    public GameObject UI_AfterSelection;


    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        playerSelectionNumber = 0;
        UI_selection.SetActive(true);
        UI_AfterSelection.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region UI Callback Methods

    public void NextPlayer()
    {
        playerSelectionNumber++;
        if (playerSelectionNumber >= spinnerTopModels.Length)
        {
            playerSelectionNumber = 0;
        }
        Debug.Log(playerSelectionNumber);
        nextButton.enabled = false;
        PreviousButton.enabled = false;
        StartCoroutine(RotateSpinners(Vector3.up, playerSwitcherTransform, 90, 1.0f));

        if (playerSelectionNumber == 0 || playerSelectionNumber == 1)
        {
            playerModelType_Text.text = "Attack";
        }
        else
        {
            playerModelType_Text.text = "Defend";
        }
    }

    public void PreviousPlayer()
    {
        playerSelectionNumber--;
        if (playerSelectionNumber < 0)
        {
            playerSelectionNumber = spinnerTopModels.Length - 1;
        }
        Debug.Log(playerSelectionNumber);
        nextButton.enabled = false;
        PreviousButton.enabled = false;
        StartCoroutine(RotateSpinners(Vector3.up, playerSwitcherTransform, -90, 1.0f));

        if (playerSelectionNumber == 0 || playerSelectionNumber == 1)
        {
            playerModelType_Text.text = "Attack";
        }
        else
        {
            playerModelType_Text.text = "Defend";
        }
    }

    public void OnSelectButtonClicked()
    {
        UI_selection.SetActive(false);
        UI_AfterSelection.SetActive(true);
        ExitGames.Client.Photon.Hashtable playerSelectionProp = new ExitGames.Client.Photon.Hashtable { { MulitplayerARSpinnerTopGame.PLAYER_SELECTION_NUMBER, playerSelectionNumber } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProp);
    }

    public void OnReselectButtonClicked()
    {
        UI_selection.SetActive(true);
        UI_AfterSelection.SetActive(false);
    }

    public void OnBattleButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Gameplay");
    }

    public void OnBackButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }

    #endregion

    #region Private Methods

    IEnumerator RotateSpinners(Vector3 axis, Transform transformToRotate, float angle, float duration = 1.0f)
    {
        Quaternion originalRotation = transformToRotate.rotation;
        Quaternion finalRotation = transformToRotate.rotation * Quaternion.Euler(axis * angle);

        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            transformToRotate.rotation = Quaternion.Slerp(originalRotation, finalRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transformToRotate.rotation = finalRotation;
        nextButton.enabled = true;
        PreviousButton.enabled = true;
    }
    #endregion
}
