using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleController : MonoBehaviour
{
    public List<Puzzle> puzzles = new List<Puzzle>();
    public Dictionary<Puzzle, bool> puzzlesCompleted = new Dictionary<Puzzle, bool>();
    public static PuzzleController defaultInstance;
    public Puzzle activePuzzle;
    public GameObject notificationWindow;

    private PlayerController playerController;

    public InteractableObj whoYouGonnaCall;

    public Puzzle FindPuzzle(int _puzzleID)
    {
        Puzzle puzzle = null;
        foreach(Puzzle p in puzzles)
        {
            if(p.puzzleID == _puzzleID)
            {
                return p;
            }
        }

        return puzzle;
    }

    public void SetPuzzleActive(int _puzzleID, bool _activate = true, InteractableObj _whoYouGonnaCall = null)
    {
        whoYouGonnaCall = _whoYouGonnaCall;
        Puzzle p = FindPuzzle(_puzzleID);

        if (p != null)
        {
            p.gameObject.SetActive(_activate);
            if (_activate == true)
            {
                activePuzzle = p;
                playerController.enabled = false;
            }
            else if(activePuzzle == p)
            {
                activePuzzle = null;
            }
        }
        else
            print("Puzzle " + _puzzleID + " not found");
        
    }

    public void CloseCurrentPuzzle(bool _success = false, string _notificationText = "")
    {
        if (activePuzzle != null)
        {
            SetPuzzleCompleted(activePuzzle.puzzleID, _success);
            activePuzzle.gameObject.SetActive(false);

            if (_notificationText != "")
            {
                notificationWindow.SetActive(true);
                notificationWindow.GetComponentInChildren<Text>().text = _notificationText;
            }
            playerController.enabled = true;
        }
    }

    public void CloseCurrentPuzzle_Button()
    {
        CloseCurrentPuzzle();
    }

    public bool PuzzleCompleted(int _puzzleID)
    {
        if (puzzlesCompleted.ContainsKey(FindPuzzle(_puzzleID)))
        {
            return puzzlesCompleted[FindPuzzle(_puzzleID)];
        }
        else
            return false;
    }

    public void SetPuzzleCompleted(int _puzzleID, bool _completed = true)
    {
        if (puzzlesCompleted.ContainsKey(FindPuzzle(_puzzleID)))
        {
            puzzlesCompleted[FindPuzzle(_puzzleID)] = _completed;
        }

        if(_completed == true)
        FindPuzzle(_puzzleID).SuccessEvent();
    }


    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        defaultInstance = this;

        foreach (Puzzle p in puzzles)
        {
            puzzlesCompleted.Add(p, false);
        }

        // SetPuzzleActive(2);
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.P))
        //     CloseCurrentPuzzle(true, "jippii");
    }
}
