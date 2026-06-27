using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon.StructWrapping;
using TMPro;

public class Battle : MonoBehaviourPun
{
    public Spinner spinnerScript;
    private Rigidbody rb;
    public GameObject UI_3D_GameObject;
    public GameObject deathPanelUIPrefab;
    private GameObject deathPanelUIGameobject;
    private float startSpinSpeed;
    private float currentSpinSpeed;
    public Image spinSpeedBar_Image;
    public TextMeshProUGUI spinSpeedRatio_Text;
    public bool isAttacker;
    public bool isDefender;
    public float commonDamageCoefficient = 0.04f;

    [Header("Player Type Damage Coefficients")]
    public float do_Damage_Attacker_Coefficient = 10f;
    public float get_Damager_Attacker_Coefficient = 1.2f;
    public float do_Damage_Defender_Coefficient = 0.75f;
    public float get_Damager_Defender_Coefficient = 0.2f;

    void Awake()
    {
        startSpinSpeed = spinnerScript.spinSpeed;
        currentSpinSpeed = spinnerScript.spinSpeed;

        spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
    }
    // Start is called before the first frame update
    void Start()
    {
        CheckPlayerType();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            float mySpeed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            float otherPlayerSpeed = collision.collider.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

            Debug.Log("My speed: " + mySpeed + " ----Other Player Speed: " + otherPlayerSpeed);

            if (mySpeed > otherPlayerSpeed)
            {
                Debug.Log("You DAMAGE the other player.");
                float default_damage_amount = gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3600 * commonDamageCoefficient;

                if (isAttacker)
                {
                    default_damage_amount *= do_Damage_Attacker_Coefficient;
                }
                else if (isDefender)
                {
                    default_damage_amount *= do_Damage_Defender_Coefficient;
                }

                if (collision.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {

                    collision.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, default_damage_amount);
                }
            }
        }
    }

    [PunRPC]
    public void DoDamage(float _damageAmount)
    {
        if(isAttacker)
        {
           _damageAmount *= get_Damager_Attacker_Coefficient;
        }
        else if(isDefender)
        {
            _damageAmount *= get_Damager_Defender_Coefficient;
        }

        spinnerScript.spinSpeed -= _damageAmount;
        currentSpinSpeed = spinnerScript.spinSpeed;

        spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;

        spinSpeedRatio_Text.text = currentSpinSpeed.ToString("F0") + "/" + startSpinSpeed;

        if(currentSpinSpeed < 100)
        {
            Die();
        }
    }

    private void CheckPlayerType()
    {
        if (gameObject.name.Contains("Attacker"))
        {
            isAttacker = true;
            isDefender = false;
        }
        else if (gameObject.name.Contains("Defender"))
        {
            isDefender = true;
            isAttacker = false;

            spinnerScript.spinSpeed = 4400f;
            startSpinSpeed = spinnerScript.spinSpeed;
            currentSpinSpeed = spinnerScript.spinSpeed;

            spinSpeedRatio_Text.text = currentSpinSpeed + "/" + startSpinSpeed;
        }
    }

    void Die()
    {
        GetComponent<MovementController>().enabled = false;
        rb.freezeRotation = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        spinnerScript.spinSpeed = 0f;
        UI_3D_GameObject.SetActive(false);

        if(photonView.IsMine)
        {
            StartCoroutine(ReSpawn());
        }
    }

    IEnumerator ReSpawn()
    {
        GameObject canvasGameObject = GameObject.Find("Canvas")
        deathPanelUIGameobject = Instantiate(deathPanelUIPrefab, canvasGameObject.transform);
    }
}