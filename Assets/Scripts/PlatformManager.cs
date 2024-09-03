using System.Collections;
using System.Collections.Generic;
using DonutRun;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] private Donut _donut;
    [SerializeField] private GameObject _platformPrefab;
    
    private List<Platform> _platforms = new List<Platform>();

    void Start() 
    {
        SpawnPlatform();
        SpawnPlatform();
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
        SpawnPlatform();
        PopPlatform();
    }

    private void PopPlatform() 
    {
        if (_platforms.Count > 3) 
        {
            _platforms[0].DestroyPlatform(1.0f);
            _platforms.RemoveAt(0);
        }
    }

    private void SpawnPlatform() 
    {
        if (_platforms.Count == 0) 
        {
            Platform newPlatform = Instantiate(_platformPrefab, transform).gameObject.GetComponent<Platform>();
            _platforms.Add(newPlatform);
        }
        else 
        {
            Platform previousPlatform = _platforms[_platforms.Count - 1];
            Transform previousTransform = previousPlatform.transform;


            Vector3 spawnPosition = new Vector3
            (
                previousTransform.position.x + previousPlatform.MoveVector.x + previousTransform.localScale.x, 
                previousTransform.position.y + previousPlatform.MoveVector.y, 
                previousTransform.position.z + previousPlatform.MoveVector.z
            );
            
            Platform newPlatform = Instantiate(_platformPrefab, spawnPosition, new Quaternion(), transform)
            .gameObject.GetComponent<Platform>();
            
            _platforms.Add(newPlatform);

            newPlatform.ChangeMovingSpeed(3.0f);
            newPlatform.ChangeMovingVector(GetRandomMoveVector());

            newPlatform.transform.Find("NextPlatformTrigger").gameObject.SetActive(true);
        }
    }

    private Vector3 GetRandomMoveVector() 
    {
        return new Vector3(Random.value * 3f, 0f, 0f);
    }
}
