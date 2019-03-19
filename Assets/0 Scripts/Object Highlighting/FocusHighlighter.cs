using UnityEngine;
using HoloToolkit.Unity.InputModule;

[RequireComponent(typeof(MeshRenderer))]
public class FocusHighlighter : MonoBehaviour, IFocusable {

    public Color FocusTint = Color.yellow;

    private Color _originalColor;
    private MeshRenderer _meshRenderer;

   // private Material _material;
    //private Color _focusColour = Color.green;
    //public  float _outlineWidth = 0.015f; //Will need to be adjusted for each object
    
    public void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _originalColor = (_meshRenderer.material.color == Color.gray) ? Color.white : _meshRenderer.material.color;
        _meshRenderer.material.color = _originalColor;
        //_meshRenderer.material.GetColor();


        //  _material = _meshRenderer.material;
        //  _meshRenderer.material.shader = Shader.Find("Outlined/Uniform");

        //  _material.SetColor("_OutlineColor", _focusColour);
        // _material.SetFloat("_OutlineWidth", 0);
        //Don't need this since you've got the requirecomponent
        if (_meshRenderer == null) Destroy(this);
    }
    void IFocusable.OnFocusEnter()
    {
        if (InputManager.Instance.CheckModalInputStack()) return;

        //Store the current material colouring when we get focus each time incase it was changed before.
        HighlightObject();
        AddOutline();
    }

    void IFocusable.OnFocusExit()
    {
      ResetColour();
        RemoveOutline();
    }

    public void OnDisable()
    {
        ResetColour();
    }

    public void OnDestroy()
    {
        if(_meshRenderer) ResetColour();
    }

    public void AddOutline()
    {
       // _material.SetFloat("_OutlineWidth", _outlineWidth);
        // _effect.AddOutline(this);
    }

    public void RemoveOutline()
    {
        //_material.SetFloat("_OutlineWidth", 0);
        // _effect.AddOutline(this);
    }
    public void HighlightObject()
    {

        //_originalColor = _meshRenderer.material.color;
        _meshRenderer.material.color = FocusTint;
    }
    public void ResetColour()
    {
        if (_meshRenderer) _meshRenderer.material.color = _originalColor;
    }
}
