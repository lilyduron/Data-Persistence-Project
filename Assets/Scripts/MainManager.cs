using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    public TextMeshProUGUI scorePlayerText;
    private string filePath;               // File path to save and load the JSON file
    private PlayerData playerData;         // The player data to be saved and loaded
    
    public string userInput;
   

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        // Retrieve the input data from the singleton
        userInput = DataManager.Instance.userInput;

        // Path and name of the JSON file
        filePath = Path.Combine(Application.persistentDataPath, "playerData.json");

        // Load the saved high score and player name from the JSON file
        LoadPlayerData();

        // Display the current high score
        UpdateHighScoreText();

    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
       // ScoreText.text = $"Score : {m_Points}";
        ScoreText.text = DataManager.Instance.userInput + " Score: " + m_Points;
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        SubmitScore();
    }

   

    // Method to update the high score text on the screen
    public void UpdateHighScoreText()
    {
        scorePlayerText.text = $"Best Score: {playerData.playerName}: {playerData.highScore}";
        
    }

    // Call this method to submit the score
    public void SubmitScore()
    {
        // Get the player name from the input field
        string playerName = userInput;

        // Check if the current score beats the high score
        if (m_Points > playerData.highScore)
        {
            // Update the high score and player name
            playerData.highScore = m_Points;
            playerData.playerName = playerName;

            // Save the new high score to the JSON file
            SavePlayerData();

            // Update the displayed high score text
            UpdateHighScoreText();
        }
    }

    [System.Serializable]
    public class PlayerData
    {
        public string playerName;
        public int highScore;
    }

     //Method to save player data to a JSON file
    void SavePlayerData()
    {
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(filePath, json);
    }

    // Method to load player data from the JSON file
    void LoadPlayerData()
    {
        if (File.Exists(filePath))
        {
            // Read the JSON data from the file and deserialize it into the PlayerData object
            string json = File.ReadAllText(filePath);
            playerData = JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            // If the file doesn't exist, create a new PlayerData object with default values
            playerData = new PlayerData { playerName = "  ", highScore = 0 };
        }
    }
}
