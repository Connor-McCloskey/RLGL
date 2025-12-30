using System;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    private bool _triggered = false;

    public event Action OnGameWon;

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered)
        {
            return;
        }
        
        if (other.gameObject.CompareTag("Player"))
        {
            _triggered = true;
            OnGameWon?.Invoke();
        }
    }
}
