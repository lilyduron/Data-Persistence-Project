using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{

    public TMP_InputField nameInputField;
    public TextMeshProUGUI textMeshPro;
    public TextMeshProUGUI scorePlayerText;
    private string filePath;
    private MainManager.PlayerData playerData;
    
    

    void Start()
    {
        // Get the active scene
        Scene currentScene = SceneManager.GetActiveScene();
     
        // Path to the player data JSON file
        filePath = Path.Combine(Application.persistentDataPath, "playerData.json");

        // Load player data to get the best score
        LoadPlayerData();

        // Check if the current scene is "Menu" or has index 0
        if (currentScene.name == "Menu" || currentScene.buildIndex == 0)
        {
           // Update the best score text
            UpdateHighScoreText();
        }
        
     
    }

    // Reference to the TMP input field
    public void OnSubmitName()
    {
        // Store the entered name in the static variable
        DataManager.Instance.userInput = nameInputField.text;
    }

     public void StartNew()
    {
       OnSubmitName();
       SceneManager.LoadScene(1);
      
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

    // Method to load player data from the JSON file
    void LoadPlayerData()
    {
        if (File.Exists(filePath))
        {
            // Read the JSON data from the file and deserialize it into PlayerData object
            string json = File.ReadAllText(filePath);
            playerData = JsonUtility.FromJson<MainManager.PlayerData>(json);
        }
        else
        {
            // If the file doesn't exist, create a default PlayerData
            playerData = new MainManager.PlayerData { playerName = "  ", highScore = 0 };
        }
    }

    // Method to update the best score text in the menu
    void UpdateHighScoreText()
    {
        // Display the best score and player name
        scorePlayerText.text = $"Best Score: {playerData.playerName}: {playerData.highScore}";
    }
}
