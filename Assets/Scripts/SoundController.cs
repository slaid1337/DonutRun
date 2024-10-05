using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundController : Singletone<SoundController>
{
    private List<AudioSource> _sources;

    public override void Awake()
    {
        base.Awake();

        SceneManager.activeSceneChanged += OnSceneChange;
    }

    private void Start()
    {
        UpdateSoundsArray();

        SetCurrentState();
    }

    public void MuteMainSounds()
    {
        SaveController.MuteMainAudio(true);
        SetCurrentState();
    }

    public void MuteMusic()
    {
        SaveController.MuteMusicAudio(true);
        SetCurrentState();
    }

    public void UnmuteMainSounds()
    {
        SaveController.MuteMainAudio(false);
        SetCurrentState();
    }

    public void UnmuteMusic()
    {
        SaveController.MuteMusicAudio(false);
        SetCurrentState();
    }

    private void OnSceneChange(Scene scene1, Scene scene2)
    {

        UpdateSoundsArray();

        SetCurrentState();
    }

    private void SetCurrentState()
    {
        UpdateSoundsArray();

        if (SaveController.IsMuteMainAudio())
        {
            foreach (var item in _sources)
            {
                if (item == null) continue;
                item.enabled = false;
            }
        }
        else
        {
            foreach (var item in _sources)
            {
                if (item == null) continue;
                item.enabled = true;
            }
        }

        if (SaveController.IsMuteMusicAudio())
        {
            Music.Instance.MusicSource.Pause();
        }
        else
        {
            Music.Instance.MusicSource.UnPause();
        }
    }

    [ContextMenu("update")]
    private void UpdateSoundsArray()
    {
        if (_sources == null) _sources = new List<AudioSource>();
        _sources.Clear();
        _sources = FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        _sources.Remove(Music.Instance.MusicSource);
    }
}
