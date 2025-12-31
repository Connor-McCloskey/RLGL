using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Phase
{
    RedLight,
    GreenLight
}

public class EyeRobot : MonoBehaviour
{
    #region Vars
    private Phase _currentPhase = Phase.GreenLight;
    private const float GracePeriod = 0.5f;
    private int timesSpotted = 0;
    private int maxTimesSpotted = 3;
    private bool gameOver = false;
    private float[] spectrumData = new float[1024];
    private Coroutine _runningCoroutine;
    
    public AudioSource _audioSource;
    public AudioSource redLightStart;
    public AudioSource redLightIdle;
    public AudioSource redLightAlert;
    public PlayerController player;
    public Light eyeLight;
    public Color green;
    public Color red;
    public List<AudioClip> audioClips;
    
    public event Action DialogueCallback;
    public event Action OnGameLost;
    #endregion

    #region Methods

    public void StartGameplay()
    {
        _runningCoroutine = StartCoroutine(StartGreenLight());
    }
    
    private void Start()
    {
        player.OnPlayerMoved += PlayerMoved;
    }

    private void LookAtPlayer()
    {
        if (!player)
        {
            return;
        }

        transform.LookAt(player.transform.position);
    }

    private IEnumerator StartGreenLight()
    {
        redLightIdle.Stop();
        _currentPhase = Phase.GreenLight;
        eyeLight.color = green;

        if (gameOver)
        {
            yield return null;
        }

        float randDuration = UnityEngine.Random.Range(1f, 4f);
        yield return new WaitForSeconds(randDuration);

        _runningCoroutine = StartCoroutine(StartRedLight());
    }

    private IEnumerator StartRedLight()
    {
        eyeLight.color = red;
        redLightStart.Play();
        redLightIdle.Play();
        
        yield return new WaitForSeconds(GracePeriod);
        
        _currentPhase = Phase.RedLight;
        
        float randDuration = UnityEngine.Random.Range(1f, 4f);
        yield return new WaitForSeconds(randDuration);

        if (gameOver)
        {
            yield return null;
        }

        _runningCoroutine = StartCoroutine(StartGreenLight());
    }

    private void PlayerMoved()
    {
        if (gameOver)
        {
            return;
        }
        if (_currentPhase == Phase.GreenLight)
        {
            return;
        }
        
        StopCoroutine(_runningCoroutine);
        Debug.Log("Player Spotted!");
        timesSpotted++;
        redLightAlert.Play();
        
        if (timesSpotted >= maxTimesSpotted && !gameOver)
        {
            gameOver = true;
            Debug.Log("Player Lost!");
            OnGameLost?.Invoke();
        }
        else
        {
            _currentPhase = Phase.GreenLight;
            
            // Play dialogue
            StartCoroutine(PlayDialogue(timesSpotted-1));
        }
    }

    public void OnGameOver(Action callback, int id)
    {
        DialogueCallback = callback;
        gameOver = true;
        StopCoroutine(_runningCoroutine);
        StartCoroutine(PlayDialogue(id));
    }

    private IEnumerator PlayDialogue(int id)
    {
        AudioClip audioClip = audioClips[id];
        float duration = audioClip.length;
        
        _audioSource.clip = audioClip;
        _audioSource.Play();

        yield return new WaitForSeconds(duration);

        if (gameOver)
        {
            DialogueCallback?.Invoke();
        }
        else
        {
            _runningCoroutine = StartCoroutine(StartGreenLight());
        }
    }
    
    private float GetAverageAmplitude(float[] samples)
    {
        float sum = 0;
        foreach (float sample in samples)
        {
            // Use absolute value as spectrum data can be positive or negative
            sum += Mathf.Abs(sample);
        }
        return sum / samples.Length;
    }

    private void FlashLight()
    {
        if (!_audioSource.isPlaying)
        {
            eyeLight.intensity = 10f;
            return;
        }
        
        // Thank you to Google for this little nugget of wisdom!
        _audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);
        
        float AverageAmplitude = GetAverageAmplitude(spectrumData) * 10000f;

        float loudness = Mathf.Clamp(AverageAmplitude, 1f, 10f);

        eyeLight.intensity = loudness;
    }
    
    private void Update()
    {
        FlashLight();
        LookAtPlayer();
    }
    #endregion
}
