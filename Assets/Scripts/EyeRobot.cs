using System.Collections;
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
    
    public PlayerController player;
    public Light eyeLight;
    public Color green;
    public Color red;
    #endregion

    #region Methods

    public void StartGameplay()
    {
        StartCoroutine(StartGreenLight());
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
        _currentPhase = Phase.GreenLight;
        eyeLight.color = green;
        
        float randDuration = Random.Range(1f, 5f);
        yield return new WaitForSeconds(randDuration);

        StartCoroutine(StartRedLight());
    }

    private IEnumerator StartRedLight()
    {
        eyeLight.color = red;
        
        yield return new WaitForSeconds(GracePeriod);
        
        _currentPhase = Phase.RedLight;
        
        float randDuration = Random.Range(1f, 5f);
        yield return new WaitForSeconds(randDuration);

        StartCoroutine(StartGreenLight());
    }

    private void PlayerMoved()
    {
        if (_currentPhase == Phase.GreenLight)
        {
            return;
        }
        
        Debug.Log("Player Spotted!");
        timesSpotted++;
        if (timesSpotted >= maxTimesSpotted && !gameOver)
        {
            gameOver = true;
            Debug.Log("Player Lost!");
        }
    }
    
    private void Update()
    {
        LookAtPlayer();
    }
    #endregion
}
