using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCollider : MonoBehaviour
{

    private InteractableObj interactableObj;

    private void Awake()
    {
        interactableObj = GetComponentInParent<InteractableObj>();
    }

    //TODO: FIX: if you go in with a weapon, changing to empty inside the trigger will not work. You need to ENTER the trigger empty handed

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.transform.GetComponent<PlayerController>();
        if(playerController.UpperBodyMode == PlayerController.UpperBodyModes.Empty)
        {
            playerController.interactable = interactableObj;
            playerController.InteractPrompt.SetActive(true);
            //print("can interact");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.GetComponent<PlayerController>().interactable = null;
        other.transform.GetComponent<PlayerController>().InteractPrompt.SetActive(false);
        //print("cannot interact");
    }

}
