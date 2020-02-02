using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObj : MonoBehaviour
{
    public DoorScript connectedDoor;

    public void Interact()
    {
        print("INTERACT");
        PuzzleController.defaultInstance.SetPuzzleActive(0, true, this);
    }

    public void PuzzleFinished()
    {
        connectedDoor.OpenDoor();
    }
}
