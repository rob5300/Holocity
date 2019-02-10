using UnityEngine;
using HoloToolkit.Unity.InputModule;

[RequireComponent(typeof(MeshRenderer))]
public class FocusHighlighter : MonoBehaviour, IFocusable {

    public Color FocusTint = Color.yellow;

    private Color _originalColor;
    private MeshRenderer _meshRenderer;
    private HandDetection _handDetection;
    
    public void Start()
    {
        _handDetection = FindObjectOfType<HandDetection>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _originalColor = _meshRenderer.material.color;
		if(_meshRenderer == null) Destroy(this);
    }

    void IFocusable.OnFocusEnter()
    {
        if (InputManager.Instance.CheckModalInputStack()) return;

        //Store the current material colouring when we get focus each time incase it was changed before.
        HighlightObject();
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

    public void HighlightObject()
    {
        _originalColor = _meshRenderer.material.color;
        _meshRenderer.material.color = FocusTint;
    }
    public void ResetColour()
    {
        if (_meshRenderer) _meshRenderer.material.color = _originalColor;
    }
}
