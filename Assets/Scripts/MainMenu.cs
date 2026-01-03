using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    #region Vars
    public Button btnStartGame;
    public Button btnQuit;

    public AudioClip hoverSFX;
    public AudioClip selectSFX;

    private Button _focusedButton;
    private PlayerInput _playerInput;
    private AudioSource _audioSource;
    #endregion
    
    #region Methods
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        btnStartGame.onClick.AddListener(OnStartGame);
        btnQuit.onClick.AddListener(OnQuit);

        btnStartGame.GetComponent<ButtonFocusHandler>().OnFocus += OnButtonGotFocus;
        btnStartGame.GetComponent<ButtonFocusHandler>().OnHover += PlayHoverSFX;
        
        btnQuit.GetComponent<ButtonFocusHandler>().OnFocus += OnButtonGotFocus;
        btnQuit.GetComponent<ButtonFocusHandler>().OnHover += PlayHoverSFX;
        
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.onControlsChanged += OnInputMethodChanged;

        if (_playerInput.currentControlScheme.ToLower() == "gamepad")
        {
            EventSystem.current.SetSelectedGameObject(btnStartGame.gameObject);
        }
    }

    private void OnInputMethodChanged(PlayerInput input)
    {
        if (input.currentControlScheme.ToLower() == "gamepad")
        {
            if (!_focusedButton)
            {
                EventSystem.current.SetSelectedGameObject(btnStartGame.gameObject);
                return;
            }
            EventSystem.current.SetSelectedGameObject(_focusedButton.gameObject);
        }
    }
    
    private void OnButtonGotFocus(Button btn)
    {
        PlayHoverSFX(btn);

        _focusedButton = btn;
    }

    private void PlayClickSFX()
    {
        if (!_audioSource)
        {
            return;
        }
        
        _audioSource.clip = selectSFX;
        _audioSource.Play();
    }

    private void PlayHoverSFX(Button _)
    {
        if (!_audioSource)
        {
            return;
        }
        
        _audioSource.clip = hoverSFX;
        _audioSource.Play();
    }

    private void OnStartGame()
    {
        PlayClickSFX();
        SceneManager.LoadScene("RLGL_Main");
    }

    private void OnQuit()
    {
        PlayClickSFX();
        Application.Quit();
    }
    #endregion
}
