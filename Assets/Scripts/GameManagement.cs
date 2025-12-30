using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour
{
    #region Vars
    public CameraFade cameraFade;
    public EyeRobot robot;
    public PlayerController player;
    public PlayableDirector timeline;
    public Trigger WinTrigger;
    #endregion
    
    #region Methods
    void Awake()
    {
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
        // Fade camera
        cameraFade.FadeOut();
        
        // Play dialogue
        
        // Re-open level
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnGameWon()
    {
        // Fade camera
        cameraFade.FadeOut();
        
        // Play dialogue
        
        // Quit application
        Application.Quit();
    }
    #endregion
}
