using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public float closedYPos, openedYPos;
    public bool open;
    public float DoorSpeed;
    public int LevelToOpen;

    public void OpenDoor()
    {
        if(!open)
        {
            StopAllCoroutines();
            StartCoroutine(OpenDoorCoRo());
            open = true;
            FindObjectOfType<EnemyHandler>().ActivateLevelEnemies(LevelToOpen);
        }
    }
    public void CloseDoor()
    {
        if(open)
        {
            StopAllCoroutines();
            StartCoroutine(CloseDoorCoRo());
            open = false;
        }
    }

    IEnumerator CloseDoorCoRo()
    {
        while(transform.position.y > closedYPos)
        {
            transform.position += Vector3.down * DoorSpeed * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator OpenDoorCoRo()
    {
        while(transform.position.y < openedYPos)
        {
            transform.position += Vector3.up * DoorSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
