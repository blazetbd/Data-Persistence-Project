using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text bestScoreText;
    public GameObject GameOverText;
    public int bestScore;


    public string playerName;
    public string bestName;


    private bool m_Started = false;
    private int m_Points;
    private bool m_GameOver = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void StartOver()
    {
        m_Points = 0;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Only run in the gameplay scene (scene index 1)
        if (scene.buildIndex == 1)
        {
            LoadScore();
            AssignReferences();
            SpawnBricks();
        }
    }

    private void AssignReferences()
    {
        bestScoreText = GameObject.Find("ScoreText (1)").GetComponent<Text>();
        Ball = GameObject.Find("Ball").GetComponent<Rigidbody>();
        ScoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        GameOverText = GameObject.Find("GameoverText");

        bestScoreText.text = "Best Score : " + bestName + " : " + bestScore;

        if (GameOverText != null)
            GameOverText.SetActive(false);
    }

    private void SpawnBricks()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
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
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0).normalized;

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = false;
                m_GameOver = false;
                if (GameOverText != null) GameOverText.SetActive(false);
                StartOver();
            }
        }
    }

    private void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        if (m_Points > bestScore)
        {
            bestScore = m_Points;
            bestName = playerName;
            SaveScore();
            bestScoreText.text = "Best Score : " + bestName + " : " + bestScore;
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        if (GameOverText != null) GameOverText.SetActive(true);
    }

    [System.Serializable]
    class SaveData
    {
        public int bestScore;
        public string bestName;
    }

    public void SaveScore()
    {
        SaveData data = new SaveData();
        data.bestScore = bestScore;
        data.bestName = playerName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestScore = data.bestScore;
            bestName = data.bestName;
        }
    }

    
}