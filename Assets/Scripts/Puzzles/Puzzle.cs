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
        switch (puzzleID)
        {
            case 0:
                print("puzzle onnistu jeeje");
                break;

            default:
                break;
        }
    }
}
