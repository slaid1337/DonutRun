using UnityEngine;

public class Music : Singletone<Music>
{
    public AudioSource MusicSource;

    public override void Awake()
    {
        base.Awake();

        MusicSource = GetComponent<AudioSource>();
    }
}
