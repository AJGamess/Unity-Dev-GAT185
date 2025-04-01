using TMPro;
using UnityEngine;

public class GUIManager : Singleton<GUIManager>
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] IntData score;

    void Update()
    {
        scoreText.text = score.Value.ToString();
    }
}
