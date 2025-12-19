using UnityEngine;
using UnityEngine.UI;

public class AvatarImage : MonoBehaviour
{
    [SerializeField] private Image _image;

    private string _uniqueId;

    private void OnEnable()
    {
        if (LeaderboardYGMediator.Instance != null)
            LeaderboardYGMediator.Instance.AvatarLoaded += OnLoadedAvatar;

        UpdateTexture();
    }

    private void OnDisable()
    {
        if (LeaderboardYGMediator.Instance != null)
            LeaderboardYGMediator.Instance.AvatarLoaded -= OnLoadedAvatar;
    }

    public void Activate(string uniqueId)
    {
        if (uniqueId == _uniqueId)
            return;

        _uniqueId = uniqueId;
        UpdateTexture();
    }

    public void Deactivate()
    {
        _uniqueId = string.Empty;
        _image.sprite = null;
    }

    private void UpdateTexture()
    {
        if (LeaderboardYGMediator.Instance == null)
            return;

        if (string.IsNullOrEmpty(_uniqueId) || _uniqueId == "null")
        {
            _image.sprite = null;

            return;
        }

        if (LeaderboardYGMediator.Instance.TryGetAvatar(_uniqueId, out AvatarInfo avatarInfo))
            _image.sprite = avatarInfo.Sprite;
    }

    private void OnLoadedAvatar(AvatarInfo info)
    {
        if (_uniqueId == info.UniqueId)
            UpdateTexture();
    }
}