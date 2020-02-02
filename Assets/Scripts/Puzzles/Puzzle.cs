using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle : MonoBehaviour
{
    public int puzzleID;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SuccessEvent()
    {
        if(PuzzleController.defaultInstance.whoYouGonnaCall != null)
            PuzzleController.defaultInstance.whoYouGonnaCall.PuzzleFinished();

        switch (puzzleID)
        {
            case 0:
                print("puzzle onnistu jeeje");
                break;
            case 1:
                print("puzzle2 onnistu jeeje");
                break;

            default:
                break;
        }
    }
}
