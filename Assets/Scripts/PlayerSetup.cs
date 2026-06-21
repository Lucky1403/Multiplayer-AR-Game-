using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using TMPro;
using Photon.Realtime;

public class PlayerSetup : MonoBehaviourPun
{
    public TextMeshProUGUI playerNameText;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            transform.GetComponent<MovementController>().enabled = true;
            transform.GetComponent<MovementController>().joystick.gameObject.SetActive(true);
        }
        else
        {
            transform.GetComponent<MovementController>().enabled = false;
            transform.GetComponent<MovementController>().joystick.gameObject.SetActive(false);
        }

        SetPlayerName();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetPlayerName()
    {
        if (playerNameText != null)
        {
            if (photonView.IsMine)
            {
                playerNameText.text = "YOU";
                playerNameText.color = Color.red;
            }
            else
            {
                playerNameText.text = photonView.Owner.NickName;
            }
        }
    }
}
