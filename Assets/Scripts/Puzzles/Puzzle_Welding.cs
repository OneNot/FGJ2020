using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle_Welding : MonoBehaviour
{
    public Texture2D blowTorchTexture;
    public float health;
    public float fuel;

    public float damage = 0.1f;
    public float fuelConsumption = 0.1f;
    public float percentageToWin = 0.95f;

    public Slider HPBar;
    public Slider FuelBar;
    public GameObject ps;
    public List<WeldingArea> weldingAreas = new List<WeldingArea>();
    public List<WeldingArea> weldedAreas = new List<WeldingArea>();


    private float startingHealth;
    private float startingFuel;

    // Start is called before the first frame update
    void Start()
    {
        SetBlowtorchCursor(true);
        UpdateFuelBar();
        UpdateHPBar();
        UpdateWeldingAreas();

        startingHealth = health;
        startingFuel = fuel;
    }

    public void UpdateWeldingAreas()
    {
        weldingAreas.Clear();
        foreach(WeldingArea w in gameObject.GetComponentsInChildren<WeldingArea>())
        {
            weldingAreas.Add(w);
        }
    }

    public void ResetPuzzle()
    {
        SetBlowtorchCursor(false);
        ps.GetComponent<ParticleSystem>().Clear();
        ps.GetComponent<ParticleSystem>().Stop();
        health = startingHealth;
        fuel = startingFuel;
        weldedAreas.Clear();
        foreach (WeldingArea w in weldingAreas)
        {
            Color waColor = w.gameObject.GetComponentInChildren<Image>().color;
            w.gameObject.GetComponentInChildren<Image>().color = new Color(waColor.r, waColor.g, waColor.b, 0);
        }
        
    }

    public void CheckWin()
    {
        float percentage = (float)weldedAreas.Count / (float)weldingAreas.Count;
        if(percentage >= percentageToWin)
        {
            ResetPuzzle();
            PuzzleController.defaultInstance.SetPuzzleCompleted(PuzzleController.defaultInstance.activePuzzle.puzzleID);
            PuzzleController.defaultInstance.CloseCurrentPuzzle(true, "Welding successful!");
            SetBlowtorchCursor(false);
        }
    }

    public void SetBlowtorchCursor(bool _active)
    {
        if(_active)
            Cursor.SetCursor(blowTorchTexture, new Vector2(0, 15), CursorMode.ForceSoftware);
        else
            Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);
    }

    public void UpdateHPBar()
    {
        HPBar.value = health;
    }

    public void UpdateFuelBar()
    {
        FuelBar.value = fuel;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            fuel -= fuelConsumption * Time.deltaTime;
            UpdateFuelBar();
            if (WeldingArea.hovering == false)
            {
                health -= damage * Time.deltaTime;
                UpdateHPBar();
            }

            if (health <= 0 || fuel <= 0)
            {
                PuzzleController.defaultInstance.CloseCurrentPuzzle(false, "Welding failed");
                ResetPuzzle();
            }

            else if (WeldingArea.CurrentWeldingArea != null)
            {
                Color waColor = WeldingArea.CurrentWeldingArea.gameObject.GetComponentInChildren<Image>().color;
                WeldingArea.CurrentWeldingArea.gameObject.GetComponentInChildren<Image>().color = new Color(waColor.r, waColor.g, waColor.b, 255);

                if (!weldedAreas.Contains(WeldingArea.CurrentWeldingArea.gameObject.GetComponent<WeldingArea>()))
                {
                    weldedAreas.Add(WeldingArea.CurrentWeldingArea.gameObject.GetComponent<WeldingArea>());
                }

                CheckWin();
            }

            if (PuzzleController.defaultInstance.puzzlesCompleted[PuzzleController.defaultInstance.activePuzzle] == false)
            {
                ps.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                ps.GetComponent<ParticleSystem>().Play();
            }
        }

        if (Input.GetButtonUp("Fire1") || fuel <= 0)
        {
            ps.GetComponent<ParticleSystem>().Stop();
        }

    }
}
