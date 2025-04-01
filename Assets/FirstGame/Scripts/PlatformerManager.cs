using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Add this for UI support

public class PlatformerManager : MonoBehaviour
{
    static PlatformerManager instance;
    public static PlatformerManager Instacnce { get { return instance; } }

    [SerializeField] GameObject titleUI;
    [SerializeField] GameObject winUI;
    [SerializeField] GameObject loseUI;
    [SerializeField] GameObject pauseUI;
    [SerializeField] TMP_Text timerText; // UI Text to display timer

    PlayerMovement player;
    private float elapsedTime = 0f;
    private bool timerRunning = false;

    enum eState
    {
        TITLE,
        GAME,
        PAUSE,
        WIN,
        LOSE
    }
    eState state = eState.TITLE;

    private void Awake()
    {
        instance = this;
        winUI.SetActive(false);
        loseUI.SetActive(false);
        pauseUI.SetActive(false);
    }

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (timerRunning)
        {
            elapsedTime += Time.unscaledDeltaTime;
            UpdateTimerDisplay();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            state = eState.PAUSE;
        }

        switch (state)
        {
            case eState.TITLE:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                titleUI.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    OnStartGame();
                }
                break;
            case eState.GAME:
                if (player.lives == 0 || player.winner)
                {
                    SetGameOver();
                }
                break;
            case eState.PAUSE:
                OnPause();
                break;
            case eState.WIN:
                break;
            case eState.LOSE:
                break;
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int hundredths = Mathf.FloorToInt((elapsedTime * 100) % 100);
        timerText.text = $"{minutes:00}:{seconds:00}.{hundredths:00}";
    }

    public void OnStartGame()
    {
        titleUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        state = eState.GAME;
        elapsedTime = 0f; // Reset timer
        timerRunning = true; // Start timer
    }

    public void SetGameOver()
    {
        timerRunning = false; // Stop timer

        if (player.lives > 0)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            winUI.SetActive(true);
            state = eState.WIN;
        }
        else
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            loseUI.SetActive(true);
            state = eState.LOSE;
        }
    }

    public void OnRestart()
    {

        Time.timeScale = 1;
        winUI.SetActive(false);
        loseUI.SetActive(false);
        pauseUI.SetActive(false);
        player.lives = 3;
        player.transform.position = player.spawnPoint;
        player.winner = false;
        state = eState.TITLE;
        elapsedTime = 0f;
        timerRunning = false;
        UpdateTimerDisplay();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnPause()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseUI.SetActive(true);
        timerRunning = false; // Pause timer
    }

    public void OnResume()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseUI.SetActive(false);
        state = eState.GAME;
        timerRunning = true; // Resume timer
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
