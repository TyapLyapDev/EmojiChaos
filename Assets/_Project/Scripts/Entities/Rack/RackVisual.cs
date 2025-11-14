using UnityEngine;

public class RackVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem _appearanceGun;

    public void Twinkle()
    {
        Debug.Log("Мерцание свободного слота");
    }

    public void StopTwinkle()
    {
        Debug.Log("Прекращено мерцание свободного слота");
    }

    public void ShowAppearance() =>
        _appearanceGun.Play();
}