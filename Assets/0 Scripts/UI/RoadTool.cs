using HoloToolkit.Unity.Buttons;
using HoloToolkit.Unity.InputModule;
using Infrastructure.Grid.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RoadTool : MonoBehaviour {

    public GameObject indicatorPrefab;
    public GameObject buttonMenuPrefab;

    public bool active = false;
    WorldGridTile startTile;
    WorldGridTile endTile;

    private TileHighlighter tileHighlighter;
    private GameObject prevTarget;
    private Quaternion rot = Quaternion.Euler(-90, 0, 0);

    private List<WorldGridTile> tiles = new List<WorldGridTile>();
    private List<GameObject> roadIndicators = new List<GameObject>();
    
    
    public void StartTool(WorldGridTile tile)
    {
        startTile = (tile != null) ? tile : null;
        if (startTile == null) return;

        active = true;
    }

    void StopTool()
    {
        active = false;
        startTile = null;
        tiles.Clear();
        DestroyIndicators();
    }
    
    private void Start()
    {
        buttonMenuPrefab = Instantiate(buttonMenuPrefab);
        buttonMenuPrefab.SetActive(false);
        indicatorPrefab.SetActive(false);
        
        tileHighlighter = GetComponent<TileHighlighter>();

        GetButtons();
    }

    void GetButtons()
    {
        foreach (Transform child in buttonMenuPrefab.transform)
        {
            foreach (Transform gc in child)
            {
                if (gc.GetComponent<CompoundButton>())
                {
                    if (gc.name.ToLower() == "place") gc.GetComponent<CompoundButton>().OnButtonClicked += PlaceRoad;
                    else gc.GetComponent<CompoundButton>().OnButtonClicked += CancelRoad;
                }
            }
        }
    }

    private void Update()
    {
        if (active)
        {
            if (!buttonMenuPrefab.activeSelf)
            {
                buttonMenuPrefab.SetActive(true);
                buttonMenuPrefab.transform.parent = startTile.transform;
                Vector3 pos = Vector3.zero;
                pos.y += 0.05f;

                buttonMenuPrefab.transform.localPosition = pos;
            }
            GetTiles();
        }
        else
        {
            if (buttonMenuPrefab.activeSelf) buttonMenuPrefab.SetActive(false);
        }
    }

    void GetTiles()
    {
        if (GazeManager.Instance.HitObject && GazeManager.Instance.HitObject.GetComponent<FocusHighlighter>() && GazeManager.Instance.HitObject != prevTarget)
        {
            prevTarget = GazeManager.Instance.HitObject;
            endTile = GazeManager.Instance.HitObject.transform.parent.GetComponent<WorldGridTile>();
            WorldGrid grid = startTile.ParentGrid;

            ClearLine();
            DisableIndicators();



            if (startTile.Position.x == endTile.Position.x)
            {
                int i = (startTile.Position.y <= endTile.Position.y) ? 1 : -1;

                for (int y = startTile.Position.y;;)
                {
                    tiles.Add(grid.GetTile(startTile.Position.x, y));
                    
                    if (y == endTile.Position.y) break;

                    y += i;
                }

                tileHighlighter.ValidPlace = true;
            }
            else if (startTile.Position.y == endTile.Position.y)
            {
                int i = (startTile.Position.x <= endTile.Position.x) ? 1 : -1;

                for (int x = startTile.Position.x; ;)
                {
                    tiles.Add(grid.GetTile(x, startTile.Position.y));

                    if (x == endTile.Position.x) break;

                    x += i;
                }
                
                tileHighlighter.ValidPlace = true;
            }
            else
            {
                tileHighlighter.ValidPlace = false;
            }
            HighlightTiles();

        }
    }

    void HighlightTiles()
    {
        for(int i =0; i < tiles.Count; i++)
        {
            if (i >= roadIndicators.Count)
            {
                GameObject indicator = Instantiate(indicatorPrefab);
                roadIndicators.Add(indicator);
            }
            
            roadIndicators[i].transform.parent = tiles[i].transform;
            roadIndicators[i].transform.localPosition = Vector3.zero;
            roadIndicators[i].transform.localRotation = rot;
            roadIndicators[i].transform.localScale = tiles[i].transform.GetChild(0).transform.localScale;
            roadIndicators[i].SetActive(true);
        }
    }
    
    public void TilePressed()
    {
        if (tileHighlighter.ValidPlace && tiles.Count > 0)
        {
            //

            StartTool(endTile);
        }
        else
        {
            // play some error sound
            // AudioManager.Instance.errorSound();
        }
    }

    void ClearLine()
    {
        for(int i= tiles.Count - 1; i > tiles.IndexOf(startTile); i--)
        {
            tiles.RemoveAt(i);
        }
    }
    void DisableIndicators()
    {
        if (roadIndicators.Count == 0) return;

        
        for(int i = roadIndicators.Count - 1; i > tiles.IndexOf(startTile); i--)
        {
            roadIndicators[i].SetActive(false);
        }

        //roadIndicators[0].transform.parent = startTile.transform;
        //roadIndicators[0].transform.localPosition = Vector3.zero;
        //roadIndicators[0].transform.localRotation = rot;
        //roadIndicators[0].transform.localScale = startTile.transform.GetChild(0).transform.localScale;
        //roadIndicators[0].SetActive(true);

    }
    void DestroyIndicators()
    {
        for(int i = roadIndicators.Count - 1; i >=0; i--)
        {
            Destroy(roadIndicators[i]);
        }
        roadIndicators.Clear();
    }

    void PlaceRoad(GameObject go)
    {
        foreach (WorldGridTile tile in tiles)
        {
            tile.ParentGrid.GridSystem.AddTileEntityToTile(tile.Position, new Road());
        }
        StopTool();
    }
    void CancelRoad(GameObject go)
    {
        StopTool();
    }

}
