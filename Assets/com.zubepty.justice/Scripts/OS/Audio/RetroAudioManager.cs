using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class RetroAudioManager : MonoBehaviour
{
    private const string AudioPath = "Audio/";
    private const string MainMenuName = "Main_menu";

    private static RetroAudioManager instance;

    private readonly Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();
    private readonly HashSet<Button> hookedButtons = new HashSet<Button>();

    private AudioSource musicSource;
    private AudioSource sfxSource;
    private AudioSource desktopLoopSource;
    private AudioSource platformerLoopSource;
    private AudioSource shuffleLoopSource;
    private GameObject mainMenu;
    private bool wasMainMenuActive;
    private float nextButtonScanTime;

    public static RetroAudioManager Instance
    {
        get
        {
            EnsureInstance();
            return instance;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void InitializeAfterSceneLoad()
    {
        EnsureInstance();
        SceneManager.sceneLoaded -= HandleSceneLoaded;
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    public static void PlayMouseClick()
    {
        Instance.PlayOneShot("mouse_click");
    }

    public static void PlayNotification()
    {
        Instance.PlayOneShot("notification");
    }

    public static void PlayAlert()
    {
        Instance.PlayOneShot("error_006");
    }

    public static void PlayDownloadEnabled()
    {
        Instance.PlayOneShot("drop_003");
    }

    public static void PlayWrongShuffle()
    {
        Instance.PlayOneShot("error_005");
    }

    public static void PlayJump()
    {
        Instance.PlayOneShot("jump_0");
    }

    public static void PlayPlayerFailed()
    {
        Instance.PlayOneShot("Ouch__008");
    }

    public static void PlayDoorMove()
    {
        Instance.PlayOneShot("Door_move");
    }

    public static void PlayDesktopMode()
    {
        Instance.PlayLoop(Instance.desktopLoopSource, "Desktop_mode");
    }

    public static void StopDesktopMode()
    {
        Instance.StopLoop(Instance.desktopLoopSource);
    }

    public static void PlayPlatformerTheme()
    {
        Instance.PlayLoop(Instance.platformerLoopSource, "Platformer");
    }

    public static void StopPlatformerTheme()
    {
        Instance.StopLoop(Instance.platformerLoopSource);
    }

    public static void PlayCardShuffleTheme()
    {
        Instance.PlayLoop(Instance.shuffleLoopSource, "Card_Shuffle");
    }

    public static void StopCardShuffleTheme()
    {
        Instance.StopLoop(Instance.shuffleLoopSource);
    }

    public static void RefreshSceneAudio()
    {
        Instance.ResolveSceneReferences();
    }

    private static void EnsureInstance()
    {
        if (instance != null)
            return;

        RetroAudioManager existing = FindObjectOfType<RetroAudioManager>();
        if (existing != null)
        {
            instance = existing;
            return;
        }

        GameObject audioObject = new GameObject(nameof(RetroAudioManager));
        instance = audioObject.AddComponent<RetroAudioManager>();
        DontDestroyOnLoad(audioObject);
    }

    private static void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Instance.ResolveSceneReferences();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        CreateSources();
        ResolveSceneReferences();
    }

    private void Update()
    {
        UpdateMainMenuMusic();

        if (Time.unscaledTime < nextButtonScanTime)
            return;

        nextButtonScanTime = Time.unscaledTime + 0.75f;
        HookSceneButtons();
    }

    private void CreateSources()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = 0.45f;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.volume = 0.85f;

        desktopLoopSource = CreateLoopSource(0.22f);
        platformerLoopSource = CreateLoopSource(0.2f);
        shuffleLoopSource = CreateLoopSource(0.2f);
    }

    private void ResolveSceneReferences()
    {
        mainMenu = FindSceneObject(MainMenuName);
        wasMainMenuActive = IsMainMenuActive();
        UpdateMainMenuMusic(true);
        HookSceneButtons();
    }

    private void HookSceneButtons()
    {
        Button[] buttons = Resources.FindObjectsOfTypeAll<Button>();

        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];
            if (button == null || !button.gameObject.scene.IsValid() || hookedButtons.Contains(button))
                continue;

            button.onClick.AddListener(PlayMouseClick);
            if (button.name == "Btn_Virus")
                button.onClick.AddListener(StopDesktopMode);

            hookedButtons.Add(button);
        }
    }

    private void UpdateMainMenuMusic(bool force = false)
    {
        bool isMainMenuActive = IsMainMenuActive();
        if (!force && isMainMenuActive == wasMainMenuActive)
            return;

        wasMainMenuActive = isMainMenuActive;

        if (isMainMenuActive)
        {
            PlayMusic("BG_music");
            return;
        }

        if (musicSource != null && musicSource.isPlaying)
            musicSource.Stop();
    }

    private bool IsMainMenuActive()
    {
        if (mainMenu == null)
            mainMenu = FindSceneObject(MainMenuName);

        return mainMenu != null && mainMenu.activeInHierarchy;
    }

    private void PlayMusic(string clipName)
    {
        AudioClip clip = LoadClip(clipName);
        if (clip == null || musicSource == null)
            return;

        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    private void PlayOneShot(string clipName)
    {
        AudioClip clip = LoadClip(clipName);
        if (clip == null || sfxSource == null)
            return;

        sfxSource.PlayOneShot(clip);
    }

    private AudioSource CreateLoopSource(float volume)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.loop = true;
        source.playOnAwake = false;
        source.volume = volume;
        return source;
    }

    private void PlayLoop(AudioSource source, string clipName)
    {
        AudioClip clip = LoadClip(clipName);
        if (clip == null || source == null)
            return;

        if (source.clip == clip && source.isPlaying)
            return;

        source.clip = clip;
        source.Play();
    }

    private void StopLoop(AudioSource source)
    {
        if (source != null && source.isPlaying)
            source.Stop();
    }

    private AudioClip LoadClip(string clipName)
    {
        if (clips.TryGetValue(clipName, out AudioClip clip))
            return clip;

        clip = Resources.Load<AudioClip>(AudioPath + clipName);
        if (clip == null)
            Debug.LogWarning($"Missing audio clip Resources/{AudioPath}{clipName}", this);

        clips[clipName] = clip;
        return clip;
    }

    private static GameObject FindSceneObject(string objectName)
    {
        GameObject[] sceneObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        for (int i = 0; i < sceneObjects.Length; i++)
        {
            GameObject sceneObject = sceneObjects[i];
            if (sceneObject.name == objectName && sceneObject.scene.IsValid())
                return sceneObject;
        }

        return null;
    }
}
