using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text _bestScoreText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.Instance != null)
        {
            if(GameManager.Instance._previousPlayer != null && GameManager.Instance._previousBestScore > GameManager.Instance._bestScore)
            _bestScoreText.text = $"Best Score: {GameManager.Instance._previousPlayer}, score: {GameManager.Instance._previousBestScore}";
        }else _bestScoreText.text = $"Best Score: {GameManager.Instance._playerName}, score: {GameManager.Instance._bestScore}";


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
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        if (GameManager.Instance != null)
        {
            if(m_Points > GameManager.Instance._bestScore)
            {
                GameManager.Instance._bestScore = m_Points;
            }

            if(GameManager.Instance._previousBestScore < m_Points)
            {
                _bestScoreText.text = $"Best Score: {GameManager.Instance._playerName}, score: {GameManager.Instance._bestScore}";

                GameManager.Instance.SaveData();
            }
            else _bestScoreText.text = $"Best Score: {GameManager.Instance._previousPlayer}, score: {GameManager.Instance._previousBestScore}";
            //GameManager.Instance.SaveData();
        }

        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
