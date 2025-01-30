using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformerManager : MonoBehaviour
{
    static PlatformerManager instance;
    public static PlatformerManager Instacnce { get { return instance; } }

    [SerializeField] GameObject titleUI;
    [SerializeField] GameObject winUI;
    [SerializeField] GameObject loseUI;

    PlayerMovement player;

    enum eState
    {
        TITLE,
        GAME,
        WIN,
        LOSE
    }
    eState state = eState.TITLE;

    private void Awake()
    {
        instance = this;
        winUI.SetActive(false);
        loseUI.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>(); // Finds the player in the scene
    }
   
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case eState.TITLE:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                titleUI.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    OnStartGame();
                }
                break;
            case eState.GAME:
                if (player.lives == 0 || player.winner)
                {
                    SetGameOver();
                }
                break;
            case eState.WIN:
                break;
            case eState.LOSE:
                break;
        }
    }
    public void OnStartGame()
    {
        titleUI.SetActive(false);
        state = eState.GAME;
    }

    public void SetGameOver()
    {
        if (player.lives > 0)
        {
            winUI.SetActive(true);
            state = eState.WIN;
        }
        else
        {
            loseUI.SetActive(true);
            state = eState.LOSE;
        }
    }
}
