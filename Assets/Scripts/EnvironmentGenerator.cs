using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnvironmentGenerator : Singletone<EnvironmentGenerator>
{
    [SerializeField] private GameObject[] _prefabs;
    [SerializeField] private float _step;
    private List<GameObject> _sections;

    private void Start()
    {
        _sections = new List<GameObject>();

        for (int i = 0; i < 10; i++)
        {
            GenerateSection();
        }
    }

    public void UpdateEnviernment()
    {
        Destroy( _sections[0]);
        _sections.Remove(_sections[0]);

        GenerateSection();
    }

    private void GenerateSection()
    {
        GameObject prefab = _prefabs[Random.Range(0, _prefabs.Length)];

        GameObject newSection = Instantiate(prefab, transform);

        if (_sections.Count == 0)
        {
            newSection.transform.localPosition = new Vector3(-100f, 0, 0);
        }
        else
        {
            newSection.transform.localPosition = new Vector3(_sections.Last().transform.position.x + _step, 0, 0);
        }

        _sections.Add(newSection);
    }

    [ContextMenu("regen")]
    private void Regenerate()
    {
        foreach (var item in _sections)
        {
            Destroy(item);
        }

        _sections.Clear();

        for (int i = 0; i < 20; i++)
        {
            GameObject prefab = _prefabs[Random.Range(0, _prefabs.Length)];

            GameObject newSection = Instantiate(prefab, transform);

            if (_sections.Count == 0)
            {
                newSection.transform.localPosition = new Vector3(-25f, 0, 0);
            }
            else
            {
                newSection.transform.localPosition = new Vector3(_sections.Last().transform.position.x + _step, 0, 0);
            }
            _sections.Add(newSection);
        }
    }
}
