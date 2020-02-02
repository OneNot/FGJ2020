using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Pipe : MonoBehaviour
{
    public enum PowerType { Unpowered, Red, Green, Blue, Yellow, Purple };
    public PowerType powerType;
    public enum PipeType { PowerNode, TwoWay, ThreeWay, FourWay, CornerJ, CornerL, GoalNode}
    public PipeType pipeType;
    public List<Pipe> powerNodes = new List<Pipe>();
    public Pipe pipeUp;
    public Pipe pipeRight;
    public Pipe pipeDown;
    public Pipe pipeLeft;

    public bool poweredUp;
    public bool poweredRight;
    public bool poweredDown;
    public bool poweredLeft;

    public Image coloredPart;

    public static List<Pipe> PoweredPipes = new List<Pipe>();
    public static List<Pipe> PipesToReset = new List<Pipe>();

    public List<Pipe> goalNodes = new List<Pipe>();

    [HideInInspector]
    public Quaternion startRot;

    [HideInInspector]
    public PowerType goalPower;

    public static void ResetPower()
    {
        foreach(Pipe p in PoweredPipes)
        {
            p.powerType = PowerType.Unpowered;

            p.UpdateColor(PowerType.Unpowered);
        }

        PoweredPipes.Clear();
    }

    private void Start()
    {
        startRot = gameObject.transform.rotation;
        PipesToReset.Add(this);
        ResetPower();
        CheckPower();
        /*if (pipeType == PipeType.PowerNode)
        {
            UpdatePower();
        }*/


        foreach (Pipe p in powerNodes)
        {
            p.UpdatePower();
        }

        if (pipeType == PipeType.GoalNode)
        {
            goalPower = powerType;
            powerType = PowerType.Unpowered;
            UpdateColor(goalPower);
        }

        else if (pipeType == PipeType.PowerNode)
        {
            UpdateColor(powerType);
        }

        else
            UpdateColor(powerType);

    }

    public void RotateRight()
    {
        ResetPower();
        gameObject.transform.Rotate(0, 0, 90);
        CheckPower();

        foreach(Pipe p in powerNodes)
        {
            p.UpdatePower();
        }

        CheckSuccessCondition();
    }

    public void RotateLeft()
    {
        ResetPower();
        gameObject.transform.Rotate(0, 0, -90);
        CheckPower();

        foreach (Pipe p in powerNodes)
        {
            p.UpdatePower();
        }

        CheckSuccessCondition();
    }

    public void CheckSuccessCondition()
    {

        bool success = true;

        foreach(Pipe p in goalNodes)
        {
            if (p.powerType != p.goalPower && p.powerType != PowerType.Unpowered)
            {
                foreach(Pipe pp in PipesToReset)
                {
                    pp.transform.rotation = pp.startRot;
                }
                PipesToReset.Clear();
                PuzzleController.defaultInstance.CloseCurrentPuzzle(false, "Short circuit!");
            }

            if(p.powerType != p.goalPower)
            success = false;
        }

        if (success)
        {
            PuzzleController.defaultInstance.CloseCurrentPuzzle(true, "Success! Jippii Wahuu");
        }
    }

    public void CheckPower()
    {
        switch (pipeType)
        {
            case PipeType.PowerNode:
                poweredUp = true;
                poweredRight = true;
                poweredDown = true;
                poweredLeft = true;
                break;
            case PipeType.TwoWay:

                switch (gameObject.transform.rotation.eulerAngles.z)
                {
                    case 0:
                        poweredUp = true;
                        poweredRight = false;
                        poweredDown = true;
                        poweredLeft = false;
                        break;

                    case 90:
                        poweredUp = false;
                        poweredRight = true;
                        poweredDown = false;
                        poweredLeft = true;
                        break;

                    case 180:
                        poweredUp = true;
                        poweredRight = false;
                        poweredDown = true;
                        poweredLeft = false;
                        break;

                    case 270:
                        poweredUp = false;
                        poweredRight = true;
                        poweredDown = false;
                        poweredLeft = true;
                        break;

                    default:
                        break;
                }

                break;
            case PipeType.ThreeWay:
                break;
            case PipeType.FourWay:
                poweredUp = true;
                poweredRight = true;
                poweredDown = true;
                poweredLeft = true;
                break;
            case PipeType.CornerJ:
                break;
            case PipeType.CornerL:
                switch (gameObject.transform.rotation.eulerAngles.z)
                {
                    case 0:
                        poweredUp = false;
                        poweredRight = false;
                        poweredDown = true;
                        poweredLeft = true;
                        break;

                    case 90:
                        poweredUp = false;
                        poweredRight = true;
                        poweredDown = true;
                        poweredLeft = false;
                        break;

                    case 180:
                        poweredUp = true;
                        poweredRight = true;
                        poweredDown = false;
                        poweredLeft = false;
                        break;

                    case 270:
                        poweredUp = true;
                        poweredRight = false;
                        poweredDown = false;
                        poweredLeft = true;
                        break;

                    default:
                        break;
                }
                break;

            case PipeType.GoalNode:
                poweredUp = true;
                poweredRight = true;
                poweredDown = true;
                poweredLeft = true;
                break;
            default:
                break;
        }
    }

    public void UpdateColor(PowerType _powerType)
    {
        Color color = new Color();

        switch (_powerType)
        {
            case PowerType.Unpowered:
                color = new Color32(0, 0, 0, 0);
                break;
            case PowerType.Red:
                color = Color.red;
                break;
            case PowerType.Green:
                color = Color.green;
                break;
            case PowerType.Blue:
                color = Color.blue;
                break;
            case PowerType.Yellow:
                color = Color.yellow;
                break;
            case PowerType.Purple:
                color = new Color32(128, 0, 128, 255);
                break;
            default:
                break;
        }

        if(coloredPart != null)
        {
            coloredPart.color = color;
        }

        if (gameObject.GetComponent<Image>() != null && pipeType == PipeType.PowerNode || pipeType == PipeType.GoalNode)
            gameObject.GetComponent<Image>().color = color;

    }

    public void UpdatePower(List<Pipe> _checkedPipes = null)
    {
        bool alreadyChecked = false;

        if(_checkedPipes == null)
        {
            _checkedPipes = new List<Pipe>();
        }

        foreach (Pipe p in _checkedPipes)
        {
            if(p == this)
            {
                alreadyChecked = true;
            }
        }

        List<Pipe> checkedPipes = _checkedPipes;
        checkedPipes.Add(this);


        if (!alreadyChecked && this.pipeType != PipeType.GoalNode && powerType != PowerType.Unpowered)
        {
            CheckPower();

            if (poweredUp && pipeUp != null && pipeUp.pipeType != PipeType.PowerNode && pipeUp.poweredDown)
            {
                Pipe pipeToCheck = pipeUp;
                pipeToCheck.CheckPower();
                if(pipeToCheck.pipeType != PipeType.FourWay)
                {
                    if(pipeToCheck.powerType != PowerType.Unpowered && powerType != PowerType.Unpowered && pipeToCheck.powerType != powerType)
                    {
                        foreach (Pipe pp in PipesToReset)
                        {
                            pp.transform.rotation = pp.startRot;
                        }
                        PipesToReset.Clear();
                        PuzzleController.defaultInstance.CloseCurrentPuzzle(false, "Short circuit!");
                    }

                    pipeToCheck.powerType = powerType;
                    pipeToCheck.UpdateColor(powerType);
                    pipeToCheck.UpdatePower(checkedPipes);
                }
                else if(pipeToCheck.pipeUp != null && pipeToCheck.pipeUp.poweredDown)
                {
                    pipeToCheck = pipeToCheck.pipeUp;

                    if (pipeToCheck.powerType != PowerType.Unpowered && powerType != PowerType.Unpowered && pipeToCheck.powerType != powerType)
                    {
                        foreach (Pipe pp in PipesToReset)
                        {
                            pp.transform.rotation = pp.startRot;
                        }
                        PipesToReset.Clear();
                        PuzzleController.defaultInstance.CloseCurrentPuzzle(false, "Short circuit!");
                    }

                    pipeToCheck.powerType = powerType;
                    pipeToCheck.UpdateColor(powerType);
                    pipeToCheck.UpdatePower(checkedPipes);
                }

                if (pipeToCheck.powerType != PowerType.Unpowered)
                    PoweredPipes.Add(pipeToCheck);
            }

            if (poweredRight && pipeRight != null && pipeRight.pipeType != PipeType.PowerNode && pipeRight.poweredLeft)
            {
                Pipe pipeToCheck = pipeRight;
                pipeToCheck.CheckPower();
                if (pipeToCheck.pipeType != PipeType.FourWay)
                {
                    if (pipeToCheck.powerType != PowerType.Unpowered && powerType != PowerType.Unpowered && pipeToCheck.powerType != powerType)
                    {
                        foreach (Pipe pp in PipesToReset)
                        {
                            pp.transform.rotation = pp.startRot;
                        }
                        PipesToReset.Clear();
                        PuzzleController.defaultInstance.CloseCurrentPuzzle(false, "Short circuit!");
                    }

                    pipeToCheck.powerType = powerType;
                    pipeToCheck.UpdateColor(powerType);
                    pipeToCheck.UpdatePower(checkedPipes);
                }
                else if (pipeToCheck.pipeRight != null && pipeToCheck.pipeRight.poweredLeft)
                {
                    pipeToCheck = pipeToCheck.pipeRight;

                    if (pipeToCheck.powerType != PowerType.Unpowered && powerType != PowerType.Unpowered && pipeToCheck.powerType != powerType)
                    {
                        foreach (Pipe pp in PipesToReset)
                        {
                            pp.transform.rotation = pp.startRot;
                        }
                        PipesToReset.Clear();
                        PuzzleController.defaultInstance.CloseCurrentPuzzle(false, "Short circuit!");
                    }

                    pipeToCheck.powerType = powerType;
                    pipeToCheck.UpdateColor(powerType);
                    pipeToCheck.UpdatePower(checkedPipes);
                }

                if (pipeToCheck.powerType != PowerType.Unpowered)
                    PoweredPipes.Add(pipeToCheck);
            }

            if (poweredDown && pipeDown != null && pipeDown.pipeType != PipeType.PowerNode && pipeDown.poweredUp)
            {
                Pipe pipeToCheck = pipeDown;
                pipeToCheck.CheckPower();
                if (pipeToCheck.pipeType != PipeType.FourWay)
                {
                    if (pipeToCheck.powerType != PowerType.Unpowered && powerType != PowerType.Unpowered && pipeToCheck.powerType != powerType)
                    {
                        PuzzleController.defaultInstance.CloseCurrentPuzzle(false, "Short circuit!");
                    }

                    pipeToCheck.powerType = powerType;
                    pipeToCheck.UpdateColor(powerType);
                    pipeToCheck.UpdatePower(checkedPipes);
                }
                else if (pipeToCheck.pipeDown != null && pipeToCheck.pipeDown.poweredUp)
                {
                    pipeToCheck = pipeToCheck.pipeDown;

                    if (pipeToCheck.powerType != PowerType.Unpowered && powerType != PowerType.Unpowered && pipeToCheck.powerType != powerType)
                    {
                        PuzzleController.defaultInstance.CloseCurrentPuzzle(false, "Short circuit!");
                    }

                    pipeToCheck.powerType = powerType;
                    pipeToCheck.UpdateColor(powerType);
                    pipeToCheck.UpdatePower(checkedPipes);
                }

                if (pipeToCheck.powerType != PowerType.Unpowered)
                    PoweredPipes.Add(pipeToCheck);
            }

            if (poweredLeft && pipeLeft != null && pipeLeft.pipeType != PipeType.PowerNode && pipeLeft.poweredRight)
            {
                Pipe pipeToCheck = pipeLeft;
                pipeToCheck.CheckPower();
                if (pipeToCheck.pipeType != PipeType.FourWay)
                {
                    if (pipeToCheck.powerType != PowerType.Unpowered && powerType != PowerType.Unpowered && pipeToCheck.powerType != powerType)
                    {
                        PuzzleController.defaultInstance.CloseCurrentPuzzle(false, "Short circuit!");
                    }

                    pipeToCheck.powerType = powerType;
                    pipeToCheck.UpdateColor(powerType);
                    pipeToCheck.UpdatePower(checkedPipes);
                }
                else if (pipeToCheck.pipeLeft != null && pipeToCheck.pipeLeft.poweredRight)
                {
                    pipeToCheck = pipeToCheck.pipeLeft;

                    if (pipeToCheck.powerType != PowerType.Unpowered && powerType != PowerType.Unpowered && pipeToCheck.powerType != powerType)
                    {
                        PuzzleController.defaultInstance.CloseCurrentPuzzle(false, "Short circuit!");
                    }

                    pipeToCheck.powerType = powerType;
                    pipeToCheck.UpdateColor(powerType);
                    pipeToCheck.UpdatePower(checkedPipes);
                }

                if (pipeToCheck.powerType != PowerType.Unpowered)
                    PoweredPipes.Add(pipeToCheck);
            }

        }

    }
    
}