using HoloToolkit.Unity.Buttons;
using HoloToolkit.Unity.InputModule;
using Infrastructure.Grid.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RoadTool : MonoBehaviour {

    public GameObject indicatorPrefab;
    public GameObject buttonMenuPrefab;

    Stack<WorldGridTile> endTiles = new Stack<WorldGridTile>();
    List<WorldGridTile> tiles = new List<WorldGridTile>();
    
    private List<GameObject> roadIndicators = new List<GameObject>();

    public bool active = false;
    WorldGridTile startTile;
    WorldGridTile endTile;

    private TileHighlighter tileHighlighter;
    private GameObject prevTarget;
    private Quaternion rot = Quaternion.Euler(-90, 0, 0);
    private Vector3 offset = new Vector3(0.0f, 0.05f, 0.0f);
    

    public void StartTool(WorldGridTile tile)
    {
        if (active) return;

        startTile = (tile != null) ? tile : null;
        if (startTile == null) return;

        active = true;
        endTiles.Push(startTile);
        HighlightStartTile();

        buttonMenuPrefab.SetActive(true);
        buttonMenuPrefab.transform.parent = startTile.transform;
        buttonMenuPrefab.transform.localPosition = offset;
    }

    void StopTool()
    {
        active = false;
        startTile = null;
        endTiles.Clear();
        tiles.Clear();
        DestroyIndicators();
        tileHighlighter.ValidPlace = true;
        buttonMenuPrefab.SetActive(false);
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
                    else if(gc.name.ToLower() == "undo") gc.GetComponent<CompoundButton>().OnButtonClicked += UndoRoad;
                    else gc.GetComponent<CompoundButton>().OnButtonClicked += CancelRoad;
                }
            }
        }
    }

    private void Update()
    {
        if (active) GetTiles();
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

                for (int y = startTile.Position.y; ;)
                {
                    WorldGridTile tile = grid.GetTile(startTile.Position.x, y);

                    if (!tiles.Contains(tile)) tiles.Add(tile);
                    
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
                    WorldGridTile tile = grid.GetTile(x, startTile.Position.y);

                    if (!tiles.Contains(tile)) tiles.Add(tile);

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

    void HighlightStartTile()
    {
        if (roadIndicators.Count == 0)
        {
            GameObject indicator = Instantiate(indicatorPrefab);
            roadIndicators.Add(indicator);
        }

        roadIndicators[0].transform.parent = startTile.transform;
        roadIndicators[0].transform.localPosition = Vector3.zero;
        roadIndicators[0].transform.localRotation = rot;
        roadIndicators[0].transform.localScale = startTile.transform.GetChild(0).transform.localScale;
        roadIndicators[0].SetActive(true);
    }
    void HighlightTiles()
    {
        for(int i = 0; i < tiles.Count; i++)
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
    
    void ClearLine()
    {
        for(int i= tiles.Count - 1; i > tiles.IndexOf(startTile); i--)
        {
            if (i == 0) continue;
            tiles.RemoveAt(i);
        }
    }
    void DisableIndicators()
    {

        if (roadIndicators.Count == 0) return;

        
        for(int i = roadIndicators.Count - 1; i > tiles.IndexOf(startTile); i--)
        {
            if (i == 0) continue;

            roadIndicators[i].SetActive(false);
        }

    }
    void DestroyIndicators()
    {
        for(int i = roadIndicators.Count - 1; i >=0; i--)
        {
            Destroy(roadIndicators[i]);
        }
        roadIndicators.Clear();
    }

    public void TilePressed()
    {
        if (tileHighlighter.ValidPlace && tiles.Count > 0)
        {
            if (!endTiles.Contains(endTile))
            {
                startTile = endTile;
                endTiles.Push(endTile);
            }
        }
        else
        {
            // play some error sound
            // AudioManager.Instance.errorSound();
        }
    }
    void PlaceRoad(GameObject go)
    {
        foreach (WorldGridTile tile in tiles)
        {
            tile.ParentGrid.GridSystem.AddTileEntityToTile(tile.Position, new Road());
        }
        StopTool();
    }
    void UndoRoad(GameObject go)
    {
        if (endTiles.Count < 2) return;

        endTiles.Pop();
        startTile = endTiles.Peek();

        ClearLine();
        DisableIndicators();
        
    }

    void CancelRoad(GameObject go)
    {
        StopTool();
    }

}
