using System.Collections.Generic;
using System.Linq;
using DonutRun;
using UnityEngine;
using UnityEngine.Events;

public class PlatformManager : Singletone<PlatformManager>
{
    [SerializeField] private Donut _donut;
    [SerializeField] private GameObject _platformPrefab;
    [SerializeField] private GameObject _мovingPlatformPrefab;
    [SerializeField] private GameObject _spikePlatformPrefab;
    [SerializeField] private GameObject _jumpPlatformPrefab;
    [SerializeField] private GameObject _jumpPlatformReversedPrefab;

    private List<Platform> _platforms = new List<Platform>();

    public UnityEvent OnStartSpawn;

    private enum StateType
    {
        Default,
        Forked
    }

    private StateType _state;

    public Transform GetFirstPlatform()
    {
        return _platforms[0].transform.Find("NextPlatformTrigger").transform;
    }

    void Start()
    {
        _state = StateType.Default;

        SpawnPlatform(_platformPrefab);

        for (int i = 0; i < 20; i++)
        {
            int rand = Random.Range(0, 100);

            if (rand >= 60)
            {
                SpawnMovePlatform();
            }
            else if (rand >= 50)
            {
                SpawnForkedPlatform();
            }
            else
            {
                SpawnPlatform(_platformPrefab);
            }
        }

        OnStartSpawn?.Invoke();

        PausePanel.Instance.OnPause.AddListener(Pause);
        PausePanel.Instance.OnResume.AddListener(UnPause);
    }

    public void Pause()
    {
        foreach (var item in _platforms)
        {
            item.Pause();
        }
    }

    public void UnPause()
    {
        foreach (var item in _platforms)
        {
            item.UnPause();
        }
    }

    private void OnEnable()
    {
        _donut.OnNextPlatform += OnNextPlatform;
    }

    private void OnDisable()
    {
        _donut.OnNextPlatform -= OnNextPlatform;
    }

    private void OnNextPlatform()
    {
        int rand = Random.Range(0, 100);

        if (rand >= 60)
        {
            SpawnPlatform(_мovingPlatformPrefab);
        }
        else if (rand >= 50)
        {
            SpawnForkedPlatform();
        }
        else
        {
            SpawnPlatform(_platformPrefab);
        }

        PopPlatform();
    }

    private void PopPlatform()
    {
        print(_platforms[0].StartPosition.x);
        while (_donut.transform.position.x - _platforms[0].StartPosition.x > 20)
        {
            Destroy(_platforms[0].gameObject);
            _platforms.Remove(_platforms[0]);
        }
        
    }

    private void SpawnMovePlatform()
    {
        if (_state == StateType.Forked)
        {
            int[] mapSection = new int[3];

            Platform previousPlatform = _platforms[_platforms.Count - 1];
            Transform previousTransform = previousPlatform.transform;

            Vector3 spawnPosition = new Vector3
            (
                previousTransform.position.x + 5f,
                previousTransform.position.y,
                3
            );

            int rand = Random.Range(0, 100);

            if (rand >= 80)
            {
                SpawnJumpPlatform(spawnPosition);
                mapSection[0] = 1;
            }
            else
            {
                SpawnPlatform(_platformPrefab, spawnPosition);
            }

            spawnPosition = new Vector3
           (
               previousTransform.position.x + 5f,
               previousTransform.position.y,
               0
           );

            rand = Random.Range(0, 100);

            if (rand >= 80)
            {
                SpawnJumpPlatform(spawnPosition);
                mapSection[1] = 1;
            }
            else
            {
                SpawnPlatform(_platformPrefab, spawnPosition);
            }

            spawnPosition = new Vector3
           (
               previousTransform.position.x + 5f,
               previousTransform.position.y,
               -3
           );

            rand = Random.Range(0, 100);

            if (rand >= 80)
            {
                SpawnJumpPlatform(spawnPosition);
                mapSection[2] = 1;
            }
            else
            {
                SpawnPlatform(_platformPrefab, spawnPosition);
            }

            spawnPosition.x += 5;
            spawnPosition.z = 0;

            SpawnPlatform(_мovingPlatformPrefab, spawnPosition);

            if (mapSection.Contains(1))
            {
                if (mapSection[0] == 0)
                {
                    SpawnPlatform(_platformPrefab, new Vector3(spawnPosition.x + 5f, previousTransform.position.y, 3));
                }

                if (mapSection[1] == 0)
                {
                    SpawnPlatform(_platformPrefab, new Vector3(spawnPosition.x + 5f, previousTransform.position.y, 0));
                }

                if (mapSection[2] == 0)
                {
                    SpawnPlatform(_platformPrefab, new Vector3(spawnPosition.x + 5f, previousTransform.position.y, -3));
                }
            }
        }
        else
        {
            SpawnPlatform(_мovingPlatformPrefab);
        }
    }

    private void SpawnPlatform(GameObject platform)
    {
        if (_platforms.Count == 0)
        {
            SpawnPlatform(platform, new Vector3(0, -1.5f, 0));
        }
        else if (_state == StateType.Forked)
        {
            Platform previousPlatform = _platforms[_platforms.Count - 1];
            Transform previousTransform = previousPlatform.transform;

            Vector3 spawnPosition = new Vector3
            (
                previousTransform.position.x + 5f,
                previousTransform.position.y,
                3
            );

            SpawnPlatform(_platformPrefab, spawnPosition);

            previousPlatform = _platforms[_platforms.Count - 1];
            previousTransform = previousPlatform.transform;

            spawnPosition = new Vector3
           (
               previousTransform.position.x,
               previousTransform.position.y,
               0
           );

            SpawnPlatform(_platformPrefab, spawnPosition);

            previousPlatform = _platforms[_platforms.Count - 1];
            previousTransform = previousPlatform.transform;

            spawnPosition = new Vector3
           (
               previousTransform.position.x,
               previousTransform.position.y,
               -3
           );

            SpawnPlatform(_platformPrefab, spawnPosition);
        }
        else
        {
            Platform previousPlatform = _platforms[_platforms.Count - 1];
            Transform previousTransform = previousPlatform.transform;

            Vector3 spawnPosition = new Vector3
            (
                previousTransform.position.x + 5f,
                previousTransform.position.y,
                0
            );

            SpawnPlatform(platform, spawnPosition);
        }
    }

    private void SpawnForkedPlatform()
    {
        if (_state == StateType.Default)
        {
            _state = StateType.Forked;

            Platform previousPlatform = _platforms[_platforms.Count - 1];
            Transform previousTransform = previousPlatform.transform;

            Vector3 spawnPosition = new Vector3
            (
                previousTransform.position.x + 5f,
                previousTransform.position.y,
                0
            );

            SpawnPlatform(_мovingPlatformPrefab, spawnPosition);

            previousPlatform = _platforms[_platforms.Count - 1];
            previousTransform = previousPlatform.transform;

            spawnPosition = new Vector3
            (
                previousTransform.position.x + 5f,
                previousTransform.position.y,
                3
            );

            SpawnPlatform(_platformPrefab, spawnPosition);

            previousPlatform = _platforms[_platforms.Count - 1];
            previousTransform = previousPlatform.transform;

            spawnPosition = new Vector3
            (
                previousTransform.position.x,
                previousTransform.position.y,
                0
            );

            SpawnPlatform(_platformPrefab, spawnPosition);

            previousPlatform = _platforms[_platforms.Count - 1];
            previousTransform = previousPlatform.transform;

            spawnPosition = new Vector3
            (
                previousTransform.position.x,
                previousTransform.position.y,
                -3
            );

            int randomNum = Random.Range(0, 100);

            GameObject prefab = _platformPrefab;

            if (randomNum >= 60)
            {
                prefab = _spikePlatformPrefab;
            }

            SpawnPlatform(prefab, spawnPosition);
        }
        else
        {
            _state = StateType.Default;

            Platform previousPlatform = _platforms[_platforms.Count - 1];
            Transform previousTransform = previousPlatform.transform;

            Vector3 spawnPosition = new Vector3
            (
                previousTransform.position.x + 5f,
                previousTransform.position.y,
                0
            );

            SpawnPlatform(_мovingPlatformPrefab, spawnPosition);

            previousPlatform = _platforms[_platforms.Count - 1];
            previousTransform = previousPlatform.transform;

            spawnPosition = new Vector3
            (
                previousTransform.position.x + 5f,
                previousTransform.position.y,
                0
            );

            SpawnPlatform(_platformPrefab, spawnPosition);
        }
    }

    private Vector3 GetRandomMoveVector()
    {
        return new Vector3(0f, 0f, Random.value * 3f);
    }

    private Platform SpawnPlatform(GameObject prefab, Vector3 position)
    {
        Platform newPlatform = Instantiate(prefab, position, new Quaternion(), transform)
        .gameObject.GetComponent<Platform>();

        if (_platforms.Count != 0) newPlatform.GenerateMoney();

        _platforms.Add(newPlatform);
        newPlatform.transform.Find("NextPlatformTrigger").gameObject.SetActive(true);
        return newPlatform;
    }

    private void SpawnJumpPlatform()
    {
        Platform previousPlatform = _platforms[_platforms.Count - 1];
        Transform previousTransform = previousPlatform.transform;

        Vector3 spawnPosition = new Vector3
            (
                previousTransform.position.x + 5f,
                previousTransform.position.y,
                0
            );

        JumpPlace place = SpawnPlatform(_jumpPlatformPrefab, spawnPosition).GetComponentInChildren<JumpPlace>();

        spawnPosition = new Vector3
            (
                previousTransform.position.x + 15f,
                previousTransform.position.y,
                0
            );

        JumpPlace otherPlace = SpawnPlatform(_jumpPlatformReversedPrefab, spawnPosition).GetComponentInChildren<JumpPlace>();

        place.OtherPlace = otherPlace;
        otherPlace.enabled = false;
    }

    private void SpawnJumpPlatform(Vector3 position)
    {
        JumpPlace place = SpawnPlatform(_jumpPlatformPrefab, position).GetComponentInChildren<JumpPlace>();

        Vector3 spawnPosition = new Vector3
            (
                position.x + 10f,
                position.y,
                position.z
            );

        JumpPlace otherPlace = SpawnPlatform(_jumpPlatformReversedPrefab, spawnPosition).GetComponentInChildren<JumpPlace>();

        place.OtherPlace = otherPlace;
        otherPlace.enabled = false;
    }
}
