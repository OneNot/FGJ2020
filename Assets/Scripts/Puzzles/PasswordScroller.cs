using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordScroller : MonoBehaviour
{
    public bool numbersOnly;
    public ScrollRect scrollView;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scrollView.GetComponentInChildren<Text>().text += "a";
    }
}
