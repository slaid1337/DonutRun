using UnityEngine;
using System.Linq;

public class SetDonutSkin : MonoBehaviour
{
    [SerializeField] private DonutSkinData _data;

    private void Start()
    {
        string name = SaveController.Instance.GetActiveDonut();

        DonutData data = _data.Data.First(x => x.Name == name);

        GetComponent<MeshFilter>().mesh = data.DonutMesh;
        GetComponent<MeshRenderer>().sharedMaterial = data.DonutMaterial;
    }
}
