using UnityEngine;
using HoloToolkit.Unity.InputModule;

[RequireComponent(typeof(MeshRenderer))]
public class FocusHighlighter : MonoBehaviour, IFocusable {

    public Color FocusTint = Color.yellow;

    private Color _originalColor;
    private MeshRenderer _meshRenderer;

    public void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _originalColor = _meshRenderer.material.color;
    }

    void IFocusable.OnFocusEnter()
    {
        _meshRenderer.material.color = FocusTint;
    }

    void IFocusable.OnFocusExit()
    {
        ResetColour();
    }

    public void OnDisable()
    {
        ResetColour();
    }

    public void OnDestroy()
    {
        if(_meshRenderer) ResetColour();
    }

    private void ResetColour()
    {
        _meshRenderer.material.color = _originalColor;
    }
}
