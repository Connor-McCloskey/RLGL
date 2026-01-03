using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    #region Vars
    public Button btnResumeGame;
    public Button btnQuit;

    public AudioClip hoverSFX;
    public AudioClip selectSFX;

    private Button _focusedButton;
    private PlayerInput _playerInput;
    private AudioSource _audioSource;

    public event Action OnUnpaused;
    #endregion
    
    #region Methods
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        btnResumeGame.onClick.AddListener(OnResumeGame);
        btnQuit.onClick.AddListener(OnQuit);

        btnResumeGame.GetComponent<ButtonFocusHandler>().OnFocus += OnButtonGotFocus;
        btnQuit.GetComponent<ButtonFocusHandler>().OnFocus += OnButtonGotFocus;

        _playerInput = GameManagement.Instance.gameObject.GetComponent<PlayerInput>();
        _playerInput.onControlsChanged += OnInputMethodChanged;

        if (_playerInput.currentControlScheme.ToLower() == "gamepad")
        {
            EventSystem.current.SetSelectedGameObject(btnResumeGame.gameObject);
        }
    }

    private void OnInputMethodChanged(PlayerInput input)
    {
        if (input.currentControlScheme.ToLower() == "gamepad")
        {
            if (!_focusedButton)
            {
                EventSystem.current.SetSelectedGameObject(btnResumeGame.gameObject);
                return;
            }
            EventSystem.current.SetSelectedGameObject(_focusedButton.gameObject);
        }
    }

    private void OnDestroy()
    {
        _playerInput.onControlsChanged -= OnInputMethodChanged;
    }
    
    private void OnButtonGotFocus(Button btn)
    {
        _audioSource.clip = hoverSFX;
        _audioSource.Play();

        _focusedButton = btn;
    }

    private void PlayClickSFX()
    {
        _audioSource.clip = selectSFX;
        _audioSource.Play();
    }

    private void OnResumeGame()
    {
        PlayClickSFX();
        OnUnpaused?.Invoke();
    }

    private void OnQuit()
    {
        PlayClickSFX();
        Application.Quit();
    }

    #endregion
}
