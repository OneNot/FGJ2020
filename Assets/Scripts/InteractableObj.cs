using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObj : MonoBehaviour
{
    public int PuzzleId;
    public GameObject connectedDoor;

    public void Interact()
    {
        print("INTERACT");
        PuzzleController.defaultInstance.SetPuzzleActive(PuzzleId, true, this);
    }

    public void PuzzleFinished()
    {
        if(connectedDoor != null)
            connectedDoor.GetComponent<DoorScript>().OpenDoor();
    }
}
