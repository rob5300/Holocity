using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class TileHighlighter : MonoBehaviour {

    public GameObject indicator;

    private bool validPlace = true;
    [HideInInspector]
    public bool ValidPlace { get { return validPlace;} set { validPlace = value; ChangeColour(); } }
    private bool highlightTiles = true;
    
    private GameObject currentTarget;
    private Quaternion rot = Quaternion.Euler(-90, 0, 0);

    private MeshRenderer meshRenderer;

    private void Start()
    {

        indicator = Instantiate(indicator);
        meshRenderer = indicator.GetComponentInChildren<MeshRenderer>();
        UIManager.Instance.StateChanged += ToggleHighlighter;
        
    }

    private void Update()
    {
        HighlightTile();
    }
    
    void ChangeColour()
    {
        Material[] mats = meshRenderer.materials;
        Color colour = Color.green;

        if (!validPlace) colour = Color.red;
        
        foreach(Material mat in mats)
        {
            mat.color = colour;
        }

    }

    void HighlightTile()
    {
        if (highlightTiles)
        {
            if (GazeManager.Instance.HitObject && GazeManager.Instance.HitObject.GetComponent<FocusHighlighter>())
            {
                if (GazeManager.Instance.HitObject == currentTarget) return;

                currentTarget = GazeManager.Instance.HitObject;
                indicator.SetActive(true);
            }
            else
            {
                indicator.SetActive(false);
            }
        }
        else
        {
            currentTarget = UIManager.Instance.targetTile.gameObject.transform.GetChild(0).gameObject;/// gameObject;
            indicator.SetActive(true);
        }

       
        if(indicator.activeSelf)
        {
            indicator.transform.parent = currentTarget.transform.parent;
            indicator.transform.localPosition = Vector3.zero;
            indicator.transform.localRotation = rot;
            indicator.transform.localScale = currentTarget.transform.localScale;
        }
    }


    void ToggleHighlighter(bool check)
    {
       // indicator.SetActive(check);
        highlightTiles = check;
    }
}