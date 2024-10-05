using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DonutSkinData")]
public class DonutSkinData : ScriptableObject
{
    public DonutData[] Data;
}

[Serializable]
public class DonutData
{
    public Mesh DonutMesh;
    public Material DonutMaterial;
    public string Name;
}
