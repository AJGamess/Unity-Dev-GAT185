using UnityEngine;

public class NewGameManager : Singleton<NewGameManager>
{
    [SerializeField] Event OnDestroyEvent;
    [SerializeField] IntEvent OnScoreEvent;
    [SerializeField] IntData scoreData;

    int highScore = 0;


    private void Start()
    {
        OnDestroyEvent?.Subscribe(OnDestroyed);
        OnScoreEvent?.Subscribe(OnAddScore);

        scoreData.Value = 0;
        highScore = PlayerPrefs.GetInt("highscore", 0);
        print("high score: " + highScore);
    }

    public void OnDestroyed()
    {
        print("destroyed");
    }

    public void OnAddScore(int points)
    {
        scoreData.Value += points;
        if (scoreData >= highScore)
        {
            highScore = scoreData;
            PlayerPrefs.SetInt("highscore", highScore);
        }

        print(scoreData.Value);

    }
}