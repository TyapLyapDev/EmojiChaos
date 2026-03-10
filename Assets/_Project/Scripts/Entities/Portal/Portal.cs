using UnityEngine;

namespace EmojiChaos.Entities.Portal
{
    public class Portal : MonoBehaviour
    {
        public void Hide() =>
            gameObject.SetActive(false);
    }
}