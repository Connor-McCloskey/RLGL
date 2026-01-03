using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonFocusHandler : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private Button _parent;
    
    public event Action<Button> OnFocus;
    public event Action<Button> OnUnfocus;

    void Awake()
    {
        _parent = GetComponentInParent<Button>();
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        OnFocus?.Invoke(_parent);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        OnUnfocus?.Invoke(_parent);
    }
}