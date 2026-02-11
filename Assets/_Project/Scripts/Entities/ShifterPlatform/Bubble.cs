using UnityEngine;

public class Bubble : MonoBehaviour 
{ 
    public void Enable() =>
        gameObject.SetActive(true);

    public void Disable() =>
        gameObject.SetActive(false);
}