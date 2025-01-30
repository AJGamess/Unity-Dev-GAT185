using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instacnce { get { return instance;  } }

    [SerializeField] GameObject titleUI;

    enum eState
    {
        TITLE,
        GAME,
        WIN,
        LOSE
    }

    eState state = eState.TITLE;
    float timer = 0;

    private void Awake()
    {
        instance = this;
    }
    void Update()
    {
        switch (state)
        {
            case eState.TITLE:
                titleUI.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    OnStartGame();
                }
                break;
            case eState.GAME:
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
        state = eState.WIN;
    }
}
