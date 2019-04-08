using UnityEngine;

public class TextButton : MonoBehaviour {
    
    private Renderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    public void ChangeMaterial(Material mat)
    {
        if (mat && _renderer) _renderer.material = mat;
    }
}
