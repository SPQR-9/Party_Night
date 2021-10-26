using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OutlinesEffect : MonoBehaviour
{
    
    public Color OutlineColor
    {
        get 
        { 
            return _outlineColor; 
        }
        set
        {
            _outlineColor = value;
            _isNeedUpdate = true;
        }
    }

    public float OutlineWidth
    {
        get 
        { 
            return _outlineWidth; 
        }
        set
        {
            _outlineWidth = value;
            _isNeedUpdate = true;
        }
    }

    [Serializable] private class ListVector3
    {
        public List<Vector3> data;
    }
    [SerializeField] private Color _outlineColor = Color.yellow;
    [SerializeField, Range(0f, 10f)] private float _outlineWidth = 8f;
    [SerializeField, HideInInspector] private List<Mesh> _bakeKeys = new List<Mesh>();
    [SerializeField, HideInInspector] private List<ListVector3> _bakeValues = new List<ListVector3>();

    private HashSet<Mesh> _registeredMeshes = new HashSet<Mesh>();
    private Renderer[] _renderers;
    private Material _outlineMaskMaterial;
    private Material _outlineFillMaterial;
    private bool _isNeedUpdate;



    void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        _outlineMaskMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineMask"));
        _outlineFillMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineFill"));
        _outlineMaskMaterial.name = "OutlineMask (Instance)";
        _outlineFillMaterial.name = "OutlineFill (Instance)";
        LoadSmoothNormals();
        _isNeedUpdate = true;
    }

    void OnEnable()
    {
        foreach (var renderer in _renderers)
        {
            if (renderer.TryGetComponent(out ParticleSystem _))
                continue;
            var materials = renderer.sharedMaterials.ToList();
            materials.Add(_outlineMaskMaterial);
            materials.Add(_outlineFillMaterial);
            renderer.materials = materials.ToArray();
        }
    }

    void OnValidate()
    {
        _isNeedUpdate = true;
        if (_bakeKeys.Count != 0 || _bakeKeys.Count != _bakeValues.Count)
        {
            _bakeKeys.Clear();
            _bakeValues.Clear();
        }
    }

    void Update()
    {
        if (_isNeedUpdate)
        {
            _isNeedUpdate = false;

            UpdateMaterialProperties();
        }
    }

    void OnDisable()
    {
        foreach (var renderer in _renderers)
        {
            var materials = renderer.sharedMaterials.ToList();

            materials.Remove(_outlineMaskMaterial);
            materials.Remove(_outlineFillMaterial);
            renderer.materials = materials.ToArray();
        }
    }

    void OnDestroy()
    {
        Destroy(_outlineMaskMaterial);
        Destroy(_outlineFillMaterial);
    }

    void LoadSmoothNormals()
    {

        foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
        {
            if (!_registeredMeshes.Add(meshFilter.sharedMesh))
            {
                continue;
            }
            var index = _bakeKeys.IndexOf(meshFilter.sharedMesh);
            var smoothNormals = (index >= 0) ? _bakeValues[index].data : SmoothNormals(meshFilter.sharedMesh);
            meshFilter.sharedMesh.SetUVs(3, smoothNormals);
        }

        foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            if (_registeredMeshes.Add(skinnedMeshRenderer.sharedMesh))
            {
                skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];
            }
        }
    }

    private List<Vector3> SmoothNormals(Mesh mesh)
    {
        var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);
        var smoothNormals = new List<Vector3>(mesh.normals);
        foreach (var group in groups)
        {
            if (group.Count() == 1)
                continue;
            var smoothNormal = Vector3.zero;

            foreach (var pair in group)
            {
                smoothNormal += mesh.normals[pair.Value];
            }
            smoothNormal.Normalize();
            foreach (var pair in group)
            {
                smoothNormals[pair.Value] = smoothNormal;
            }
        }
        return smoothNormals;
    }

    void UpdateMaterialProperties()
    {
        _outlineFillMaterial.SetColor("_OutlineColor", _outlineColor);
        _outlineFillMaterial.SetFloat("_OutlineWidth", _outlineWidth);
    }
}
