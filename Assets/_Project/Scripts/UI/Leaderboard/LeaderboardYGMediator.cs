using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using YG;
using YG.Utils.LB;

public class LeaderboardYGMediator : MonoBehaviour
{
    private const string PhotoSize = "small";
    private const int QuantityTop = 10;

    private static LeaderboardYGMediator s_instance;

    [SerializeField] private Texture2D _anonymousAvatar;

    private LBData _data = new();

    private readonly List<AvatarInfo> _avatars = new();

    public event Action<AvatarInfo> AvatarLoaded;
    public event Action<LBData> DataUpdated;

    public static LeaderboardYGMediator Instance => s_instance;

    public LBData Data => _data;

    private void Awake()
    {
        if (s_instance != null && s_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        s_instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() =>
        RequestAnUpdate();

    private void OnEnable()
    {
        YG2.onGetLeaderboard += OnUpdateLeaderboard;
        YG2.onGetSDKData += OnGetSDKData;
    }

    private void OnDisable()
    {
        YG2.onGetLeaderboard -= OnUpdateLeaderboard;
        YG2.onGetSDKData -= OnGetSDKData;
    }

    public void RequestAnUpdate() =>
        YG2.GetLeaderboard(Constants.LeaderboardTechnoName, QuantityTop, 0, PhotoSize);

    public bool TryGetAvatar(string uniqueId, out AvatarInfo avatarInfo)
    {
        avatarInfo = _avatars.FirstOrDefault(v => v.UniqueId == uniqueId);

        return avatarInfo.UniqueId == uniqueId;
    }

    private IEnumerator LoadAvatarCoroutine(string url, string uniqueId)
    {
        if (TryGetAvatar(uniqueId, out AvatarInfo _))
            yield break;

        using UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url);

        yield return webRequest.SendWebRequest();

        Texture2D loadedTexture = null;

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            DownloadHandlerTexture handler = webRequest.downloadHandler as DownloadHandlerTexture;
            loadedTexture = handler?.texture;
        }

        loadedTexture = loadedTexture != null ? loadedTexture : _anonymousAvatar;

        Sprite sprite = Sprite.Create(
                loadedTexture,
                new Rect(0, 0, loadedTexture.width, loadedTexture.height),
                new Vector2(0.5f, 0.5f));

        AvatarInfo avatarInfo = new()
        {
            UniqueId = uniqueId,
            Texture = loadedTexture,
            Sprite = sprite
        };

        _avatars.Add(avatarInfo);
        AvatarLoaded?.Invoke(avatarInfo);
    }

    private void OnUpdateLeaderboard(LBData data)
    {
        if (data.technoName != Constants.LeaderboardTechnoName)
            return;

        _data = data;

        foreach (LBPlayerData player in data.players)
            StartCoroutine(LoadAvatarCoroutine(player.photo, player.uniqueID));

        StartCoroutine(LoadAvatarCoroutine(YG2.player.photo, YG2.player.id));

        DataUpdated?.Invoke(_data);
    }

    private void OnGetSDKData() =>
        YG2.SetLeaderboard(Constants.LeaderboardTechnoName, YG2.saves.SavesData.Score);
}