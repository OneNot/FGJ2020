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

    public static List<Pipe> PoweredPipes = new List<Pipe>();

    public List<Pipe> goalNodes = new List<Pipe>();

    [HideInInspector]
    public PowerType goalPower;

    public static void ResetPower()
    {
        foreach(Pipe p in PoweredPipes)
        {
            p.powerType = PowerType.Unpowered;
        }

        PoweredPipes.Clear();
    }

    private void Start()
    {
        ResetPower();
        CheckPower();

        if (pipeType == PipeType.PowerNode)
        {
            UpdatePower();
        }
        if (pipeType == PipeType.GoalNode)
        {
            goalPower = powerType;
            powerType = PowerType.Unpowered;
        }

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
            if (p.goalPower != p.powerType)
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
                        poweredUp = false;
                        poweredRight = true;
                        poweredDown = false;
                        poweredLeft = true;
                        break;

                    case 90:
                        poweredUp = true;
                        poweredRight = false;
                        poweredDown = true;
                        poweredLeft = false;
                        break;

                    case 180:
                        poweredUp = false;
                        poweredRight = true;
                        poweredDown = false;
                        poweredLeft = true;
                        break;

                    case 270:
                        poweredUp = true;
                        poweredRight = false;
                        poweredDown = true;
                        poweredLeft = false;
                        break;

                    default:
                        break;
                }

                break;
            case PipeType.ThreeWay:
                break;
            case PipeType.FourWay:
                break;
            case PipeType.CornerJ:
                break;
            case PipeType.CornerL:
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

        if (!alreadyChecked && this.pipeType != PipeType.GoalNode)
        {
            CheckPower();

            if (poweredUp)
            {
                Pipe pipeToCheck = pipeUp;
                bool pipeChecked = false;
                if (pipeToCheck != null)
                {
                    pipeToCheck.CheckPower();

                    foreach(Pipe p in checkedPipes)
                    {
                        if (pipeToCheck == p)
                            alreadyChecked = true;
                    }
                }


                if (pipeToCheck != null && pipeToCheck.poweredDown && pipeToCheck.pipeType != PipeType.PowerNode && !pipeChecked)
                {
                    if (powerType != pipeToCheck.powerType || pipeToCheck.powerType == PowerType.Unpowered)
                        pipeToCheck.powerType = powerType;
                    else
                        PuzzleController.defaultInstance.CloseCurrentPuzzle(false, "Short circuit!");

                    if (powerType != PowerType.Unpowered)
                        PoweredPipes.Add(pipeToCheck);

                    pipeToCheck.UpdatePower(checkedPipes);         
                }
            }

            if (poweredRight)
            {
                Pipe pipeToCheck = pipeRight;
                bool pipeChecked = false;
                if (pipeToCheck != null)
                {
                    pipeToCheck.CheckPower();

                    foreach (Pipe p in checkedPipes)
                    {
                        if (pipeToCheck == p)
                            alreadyChecked = true;
                    }
                }

                if (pipeToCheck != null && pipeToCheck.poweredLeft && pipeToCheck.pipeType != PipeType.PowerNode && !pipeChecked)
                {
                    if (powerType != pipeToCheck.powerType || pipeToCheck.powerType == PowerType.Unpowered)
                        pipeToCheck.powerType = powerType;
                    else
                        PuzzleController.defaultInstance.CloseCurrentPuzzle(false, "Short circuit!");

                    if (powerType != PowerType.Unpowered)
                        PoweredPipes.Add(pipeToCheck);

                    pipeToCheck.UpdatePower(checkedPipes);
                }
            }

            if (poweredDown)
            {
                Pipe pipeToCheck = pipeDown;
                bool pipeChecked = false;
                if (pipeToCheck != null)
                {
                    pipeToCheck.CheckPower();

                    foreach (Pipe p in checkedPipes)
                    {
                        if (pipeToCheck == p)
                            alreadyChecked = true;
                    }
                }

                if (pipeToCheck != null && pipeToCheck.poweredUp && pipeToCheck.pipeType != PipeType.PowerNode && !pipeChecked)
                {
                    if (powerType != pipeToCheck.powerType || pipeToCheck.powerType == PowerType.Unpowered)
                        pipeToCheck.powerType = powerType;
                    else
                        PuzzleController.defaultInstance.CloseCurrentPuzzle(false, "Short circuit!");

                    if (powerType != PowerType.Unpowered)
                        PoweredPipes.Add(pipeToCheck);

                    pipeToCheck.UpdatePower(checkedPipes);
                }
            }

            if (poweredLeft)
            {
                Pipe pipeToCheck = pipeLeft;
                bool pipeChecked = false;
                if (pipeToCheck != null)
                {
                    pipeToCheck.CheckPower();

                    foreach (Pipe p in checkedPipes)
                    {
                        if (pipeToCheck == p)
                            alreadyChecked = true;
                    }
                }

                if (pipeToCheck != null && pipeToCheck.poweredRight && pipeToCheck.pipeType != PipeType.PowerNode && !pipeChecked)
                {
                    if (powerType != pipeToCheck.powerType || pipeToCheck.powerType == PowerType.Unpowered)
                        pipeToCheck.powerType = powerType;
                    else
                        PuzzleController.defaultInstance.CloseCurrentPuzzle(false, "Short circuit!");

                    if (powerType != PowerType.Unpowered)
                        PoweredPipes.Add(pipeToCheck);

                    pipeToCheck.UpdatePower(checkedPipes);
                }
            }

        }

    }
}