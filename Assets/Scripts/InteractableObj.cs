using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObj : MonoBehaviour
{
    public GameObject connectedDoor;

    public void Interact()
    {
        print("INTERACT");
        PuzzleController.defaultInstance.SetPuzzleActive(0, true, this);
    }

    public void PuzzleFinished()
    {
        //print("DOOR OPENING???");
        connectedDoor.GetComponent<DoorScript>().OpenDoor();
    }
}
