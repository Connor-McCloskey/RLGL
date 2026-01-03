using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour
{
    #region Vars
    public static GameManagement Instance;

    public CameraFade cameraFade;
    public EyeRobot robot;
    public PlayerController player;
    public PlayableDirector timeline;
    public Trigger WinTrigger;
    #endregion
    
    #region Methods
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }

        Instance = this;

        // Subscribe to game over and game loss events
        robot.OnGameLost += OnGameOver;
        WinTrigger.OnGameWon += OnGameWon;
    }
    
    void Start()
    {
        // Tell camera to fade
        cameraFade.FadeIn();
        
        // Once done, start timeline
        timeline.Play();
    }

    public void OnIntroDone()
    {
        player.EnableMovement();
        robot.StartGameplay();
    }

    private void OnGameOver()
    {
        player.DisableMovement();
        player.SetGameOver();
        
        // Fade camera
        cameraFade.FadeOut();
        
        // Play dialogue
        robot.OnGameOver(RestartGame, 2);
    }

    private void OnGameWon()
    {
        player.DisableMovement();
        player.SetGameOver();
        
        // Fade camera
        cameraFade.FadeOut();
        
        // Play dialogue
        robot.OnGameOver(CloseGame, 3);
    }

    private void CloseGame()
    {
        Application.Quit();
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  
    }
    #endregion
}
