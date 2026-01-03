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

    private Button _focusedButton;
    private PlayerInput _playerInput;
    #endregion
    
    #region Methods
    void Awake()
    {
        btnResumeGame.onClick.AddListener(OnResumeGame);
        btnQuit.onClick.AddListener(OnQuit);

        btnResumeGame.GetComponent<ButtonFocusHandler>().OnFocus += OnButtonGotFocus;
        btnQuit.GetComponent<ButtonFocusHandler>().OnFocus += OnButtonGotFocus;
        
        _playerInput = GetComponent<PlayerInput>();
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
    
    private void OnButtonGotFocus(Button btn)
    {
        _focusedButton = btn;
    }

    private void OnResumeGame()
    {
        SceneManager.LoadScene("RLGL_Main");
    }

    private void OnQuit()
    {
        Application.Quit();
    }
    #endregion
}
