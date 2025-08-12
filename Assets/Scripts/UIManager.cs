using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public InputField nameField;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartNew()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(1);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MainManager.Instance.Ball = GameObject.Find("Ball").GetComponent<Rigidbody>();
        MainManager.Instance.ScoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        MainManager.Instance.GameOverText = GameObject.Find("GameoverText");
        MainManager.Instance.GameOverText.SetActive(false);

        SceneManager.sceneLoaded -= OnSceneLoaded; 
    }


    public string getName()
    {
        string playerName = nameField.text;
        return playerName;
    }
}
