using UnityEngine;

public class Audio : InitializingBehaviour
{
    private static Audio s_instance;

    [SerializeField] private Music _music;
    [SerializeField] private Sfx _sfx;

    public static Music Music => s_instance._music;

    public static Sfx Sfx => s_instance._sfx;

    private void Awake()
    {
        if (IsInitialized == false)
            Initialize();
    }

    protected override void OnInitialize()
    {
        if (s_instance != null)
        {
            Destroy(gameObject);

            return;
        }

        s_instance = this;
        DontDestroyOnLoad(gameObject);

        _music.Initialize();
        _sfx.Initialize();
    }
}