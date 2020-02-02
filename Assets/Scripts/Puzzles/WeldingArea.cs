using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeldingArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static bool hovering;
    public static GameObject CurrentWeldingArea;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
        CurrentWeldingArea = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
