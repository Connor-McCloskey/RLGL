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
    
    public PlayerController player;
    public Light eyeLight;
    public Color green;
    public Color red;
    #endregion

    #region Methods
    private void Start()
    {
        player.OnPlayerMoved += PlayerMoved;
        StartCoroutine(StartGreenLight());
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
        if (_currentPhase == Phase.RedLight)
        {
            Debug.Log("Player Losses!");
        }
    }
    
    private void Update()
    {
        LookAtPlayer();
    }
    #endregion
}
