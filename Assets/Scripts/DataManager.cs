using UnityEngine;

public class DataManager : MonoBehaviour
{
    // The singleton instance
    public static DataManager Instance;

    // Variable to store the input data
    public string userInput;

    void Awake()
    {
        // Ensure that only one instance of the DataManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Prevents this object from being destroyed when loading new scenes
            Debug.Log("DataManager instance created");
        }
        else
        {
            Debug.LogWarning("DataManager instance already exists!");
            Destroy(gameObject);  // Destroy duplicate if it exists
        }

    }

    
}
