using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordScroller : MonoBehaviour
{
    public int passwordLength = 5;
    public bool numbersOnly;
    public ScrollRect scrollView;
    public float scrollSpeed = 0.05f;
    public float characterInsertRate = 0.01f;
    public Vector2 passwordInsertRate = new Vector2(0.4f, 0.8f);
    public Text inputText;

    [HideInInspector]
    public string password = "";

    private float characterInsertTimer;
    private float passwordInsertTimer;

    [HideInInspector]
    public Scrollbar scrollVert;

    char[] charsAndNumbers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

    char[] numbers = "1234567890".ToCharArray();

    List<char> passwordQueue = new List<char>();

    // Start is called before the first frame update
    void Start()
    {
        passwordInsertTimer = Random.Range(passwordInsertRate.x,passwordInsertRate.y);
        characterInsertTimer = characterInsertRate;
        ResetPassword();

        scrollView.gameObject.SetActive(true);
        scrollVert = scrollView.verticalScrollbar;
    }

    public void CheckCharacterSpawns()
    {
        characterInsertTimer -= Time.deltaTime;
        passwordInsertTimer -= Time.deltaTime;

        if(characterInsertTimer <= 0)
        {
            characterInsertTimer = characterInsertRate;
            char randomChar;

            if (passwordQueue.Count > 0)
            {
                randomChar = passwordQueue[0];
                passwordQueue.Remove(passwordQueue[0]);
            }
            else
            {
                if (!numbersOnly)
                {
                    randomChar = charsAndNumbers[Random.Range(0, charsAndNumbers.Length)];
                }
                else
                    randomChar = numbers[Random.Range(0, numbers.Length)];
            }

            scrollView.content.GetComponent<Text>().text += randomChar.ToString();
        }

        if (passwordInsertTimer <= 0)
        {
            SetPasswordQueue();
            passwordInsertTimer = Random.Range(passwordInsertRate.x, passwordInsertRate.y);

            //scrollView.content.GetComponent<Text>().text += password;
        }
    }

    public void SetPasswordQueue()
    {
        char[] passwordArray = password.ToCharArray();
        for (int i = 0; i < passwordArray.Length; i++)
        {
            passwordQueue.Add(passwordArray[i]);
        }
    }

    public void ResetPassword()
    {
        password = "";

        for(int i = 0; i < passwordLength; i++)
        {
            if (!numbersOnly)
            {
                password += charsAndNumbers[Random.Range(0, charsAndNumbers.Length)].ToString();
            }

            else
            {
                password += numbers[Random.Range(0, numbers.Length)].ToString();
            }
        }
        print(password);
    }

    public void CheckPassword()
    {
        if(inputText.text.ToLower() == password.ToLower())
        {
            print("Password correct");
            ResetPassword();
            PuzzleController.defaultInstance.CloseCurrentPuzzle(true, "Hacked!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckCharacterSpawns();
        scrollVert.value -= scrollSpeed * Time.deltaTime;
    }
}
