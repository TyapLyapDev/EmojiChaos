using UnityEngine;

namespace UI.CustomMiniCellsLevelSelector
{
    public class Locker : MonoBehaviour
    {
        [SerializeField] private GameObject[] _lockObjects;
        [SerializeField] private GameObject[] _unlockObjects;

        public bool IsLock { get; private set; }

        public void SetLockStatus(bool isLock)
        {
            IsLock = isLock;

            foreach (GameObject obj in _lockObjects)
                obj.SetActive(IsLock);

            foreach (GameObject obj in _unlockObjects)
                obj.SetActive(IsLock == false);
        }
    }
}