using UnityEngine;

namespace UI.LevelCards
{
    public class StarsIndiator : MonoBehaviour
    {
        [SerializeField] private Star[] _stars;

        public void OpenStars(int count)
        {
            count = Mathf.Clamp(count, 0, _stars.Length);

            for (int i = 0; i < _stars.Length; i++)
                _stars[i].SetStatus(i < count);
        }
    }
}