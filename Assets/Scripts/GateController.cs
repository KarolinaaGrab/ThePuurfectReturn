using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class GateController : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private GameObject gateCollider;
    private Animator anim;
    private bool gateOpened = false;

    private void Start()
    {
        anim = this.transform.parent.GetComponent<Animator>();
        infoText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player.score == player.totalScore)
            {
                infoText.text = "You have collected all the coins! Enter the gate to end the game.";
            }
            else
            {
                int coinsNeeded = player.totalScore - player.score;
                infoText.text = $"You need {coinsNeeded} more coins to enter!";
            }
            infoText.gameObject.SetActive(true);
            
            if (player.score == player.totalScore && !gateOpened)
            {
                OpenGate();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            infoText.gameObject.SetActive(false);
        }
    }

    private void OpenGate()
    {
        if (gateCollider.activeSelf)
        {
            gateCollider.SetActive(false);
        }

        anim.SetBool("Open", true);
    }
}
