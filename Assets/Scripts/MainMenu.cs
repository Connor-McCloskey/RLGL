using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Button Btn_StartGame;
    public Button Btn_Quit;
    
    void Awake()
    {
        Btn_StartGame.onClick.AddListener(OnStartGame);
        Btn_Quit.onClick.AddListener(OnQuit);
    }

    private void OnStartGame()
    {
        SceneManager.LoadScene("RLGL_Main");
    }

    private void OnQuit()
    {
        Application.Quit();
    }
}
