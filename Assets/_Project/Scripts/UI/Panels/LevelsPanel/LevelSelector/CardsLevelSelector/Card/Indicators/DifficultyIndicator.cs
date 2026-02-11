using UnityEngine;
using UnityEngine.UI;

namespace UI.LevelCards
{
    public class DifficultyIndicator : MonoBehaviour
    {
        [SerializeField] private Image[] _images;

        public void SetDifficulty(int value)
        {
            value = Mathf.Clamp(value, 0, _images.Length);

            for (int i = 0; i < _images.Length; i++)
                _images[i].gameObject.SetActive(i < value);
        }
    }
}