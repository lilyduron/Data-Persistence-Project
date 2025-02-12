using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{

    public TMP_InputField nameInputField;
    public TextMeshProUGUI textMeshPro;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

      // Reference to the TMP input field

    public void OnSubmitName()
    {
        // Store the entered name in the static variable
        DataManager.Instance.userInput = nameInputField.text;
    }

     public void StartNew()
    {
       SceneManager.LoadScene(1);
       OnSubmitName();
    }

    public void Exit()
    {
        //this is when we use the app in the Unity editor, so the exit button works
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #else
        Application.Quit(); // original code to quit Unity player
        #endif
    }

    public void BackMenu()
    {
       SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
