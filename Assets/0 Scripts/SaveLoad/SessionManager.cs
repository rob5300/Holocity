using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using System.Text;
using Infrastructure.Grid;
using System.Reflection;
using Infrastructure.Grid.Entities.Buildings;
using Infrastructure.Residents;



[Serializable]
public class SaveData
{
    public List<GridInfo> gridInfo;

    public SaveData(List<GridInfo> GridInfo)
    {
        gridInfo = GridInfo;
    }
}

[Serializable]
public class GridInfo
{
    public int width;
    public int height;
    public Vector3 worldPos;
    
    //x,y = pos; z = index
    public List<Vector3Int> gridEntities = new List<Vector3Int>();
    public List<ResidentInfo> residentInfo = new List<ResidentInfo>();

    public GridInfo(int Width, int Height, Vector3 WorldPos, List<Vector3Int> GridEntities, List<ResidentInfo> ResidentInfo)
    {
        width = Width;
        height = Height;
        worldPos = WorldPos;
        gridEntities = GridEntities;
        residentInfo = ResidentInfo;
    }
}

[Serializable]
public class ResidentInfo
{
    public string FirstName { get; protected set; }
    public string SecondName { get; protected set; }
    public Vector2Int HomePosition;
    public bool ShouldBeRemoved { get; set; }
    public bool Homeless = true;

    public ResidentInfo()
    {
        
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
        List<GridSystem> grids = Game.CurrentSession.City.GetGrids();
        List<GridInfo> gridInfo = new List<GridInfo>();
        
        foreach (GridSystem grid in grids)
        {
            List<ResidentInfo> residentInfo = new List<ResidentInfo>();
            foreach(Resident res in grid.Residents)
            {
                residentInfo.Add(CopyResidentWithReflection(res));
            }


            GridInfo gridInfos = new GridInfo(grid.Width, grid.Height, grid.Position, grid.GetTilesForSaving(), residentInfo);

            gridInfo.Add(gridInfos);
        }

        SaveData saveData = new SaveData(gridInfo);

        return saveData;
    }

    private ResidentInfo CopyResidentWithReflection(Resident residentToCopy)
    {
        ResidentInfo info = new ResidentInfo();

        Type residentType = typeof(Resident);
        PropertyInfo[] residentProperties = residentType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        FieldInfo[] residentFields = residentType.GetFields(BindingFlags.Public | BindingFlags.Instance);

       
        foreach(PropertyInfo property in residentProperties)
        {
            object value = property.GetValue(residentToCopy);
            if (property.PropertyType.IsEquivalentTo(typeof(Happiness))) continue;
            else
            {
                PropertyInfo prop2 = info.GetType().GetProperty(property.Name);
                if (prop2 != null) prop2.SetValue(info, value);
            }
        }
        foreach(FieldInfo field in residentFields)
        {
            object value = field.GetValue(residentToCopy);
            if (field.FieldType.IsEquivalentTo(typeof(Residential)))
            {
                info.HomePosition = ((Residential)value).ParentTile.Position;
            }
            else 
            {
                FieldInfo field2 = info.GetType().GetField(field.Name);
                if(field2 != null) field2.SetValue(info, value);
            }
        }
        
        return info;
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
            LoadGame(SaveFolder + defaultName + 1 + defaultType);
        }
    }


    [ContextMenu("Save")]
    public bool SaveGame()
    {
        SaveData saveData = CreateSaveFile();

        int i = 0;
        while (File.Exists(SaveFolder + defaultName + i + defaultType)) i++;

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




