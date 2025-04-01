using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] StringEvent onLoadLevel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnLoadLevel(string levelName)
    {
       
    }
    public void OnQuitGame()
    {
        Application.Quit();
    }
}
