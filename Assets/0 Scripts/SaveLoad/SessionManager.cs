using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using System.Text;
using Newtonsoft.Json;
using Infrastructure.Grid;

[Serializable]
public class SaveData
{

    public List<GridInfo> gridInfo = new List<GridInfo>();
    //public List<>
}

[Serializable]
public class GridInfo
{
    public int width;
    public int height;
    public Vector3 worldPos;
    
    //x,y = pos; z = index
    public List<Vector3Int> gridEntities = new List<Vector3Int>();

    public GridInfo(int Width, int Height, Vector3 WorldPos, List<Vector3Int> GridEntities)
    {
        width = Width;
        height = Height;
        worldPos = WorldPos;
        gridEntities = GridEntities;
    }
}

public class SessionManager : MonoBehaviour {

    private SessionCreator sessionCreator;
    private static  string SaveFolder;
    private readonly string defaultName = "savegame";
    private readonly string defaultType = ".hsf";

    private void Awake()
    {
        SaveFolder = Application.persistentDataPath + "/SaveFolder/";
        if (!Directory.Exists(SaveFolder)) Directory.CreateDirectory(SaveFolder);
    }

    private void Start()
    {
        sessionCreator = GetComponent<SessionCreator>();
    }

    public SaveData CreateSaveFile()
    {
        SaveData saveData = new SaveData();
        List<GridSystem> grids = Game.CurrentSession.City.GetGrids();

        List<GridInfo> gridInfo = new List<GridInfo>();

        foreach(GridSystem grid in grids)
        {
            //loop for each tile entity;
            GridInfo gridInfos = new GridInfo(grid.Width, grid.Height, grid.Position, grid.GetTilesForSaving());

            gridInfo.Add(gridInfos);
        }

        saveData.gridInfo = gridInfo;

        return saveData;
    }

    public bool OverwriteGame(string saveName, SaveData saveData)
    {
        if (!File.Exists(Application.dataPath + saveName)) return false;

        string path = SaveFolder + saveName + defaultType;

        string json = JsonUtility.ToJson(saveData);
        byte[] data = Encoding.ASCII.GetBytes(json);

        UnityEngine.Windows.File.WriteAllBytes(path, data);

        return true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            LoadGame(SaveFolder + defaultName + 0 + defaultType);
        }
    }


    [ContextMenu("Save")]
    public bool SaveGame()
    {
        SaveData saveData = CreateSaveFile();

        int i = 0;
        while (File.Exists(Application.dataPath + defaultName + i + defaultType)) i++;

        string path = SaveFolder + defaultName + i + defaultType;

        string json = JsonUtility.ToJson(saveData);
        byte[] data = Encoding.ASCII.GetBytes(json);
        UnityEngine.Windows.File.WriteAllBytes(path, data);

        return true;
    }
    
    public bool LoadGame(string saveName)
    {
        if (!File.Exists(saveName)) return false;


        string path = saveName;

        byte[] data = UnityEngine.Windows.File.ReadAllBytes(path);
        string json = Encoding.ASCII.GetString(data);
        SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

        sessionCreator.StartGame(loadedData.gridInfo);

        return true;
    }

}




