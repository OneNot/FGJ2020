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

    private void OnTriggerEnter(Collider other)
    {
        other.transform.GetComponent<PlayerController>().interactable = interactableObj;
        print("can interact");
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.GetComponent<PlayerController>().interactable = null;
        print("cannot interact");
    }

}
