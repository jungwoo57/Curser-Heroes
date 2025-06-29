using UnityEngine;

public class StartWaveButton : MonoBehaviour
{
    public WaveManager waveManager;

    public void OnStartWaveButtonClicked()
    {
        waveManager?.StartWave();
    }
}