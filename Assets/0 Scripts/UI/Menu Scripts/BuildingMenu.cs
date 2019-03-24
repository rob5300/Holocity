using HoloToolkit.Unity.Buttons;
using Infrastructure.Grid.Entities;
using Infrastructure.Grid.Entities.Buildings;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingMenu : MonoBehaviour {

    [Header("Building Menu")]
    public Transform buildingMenuCanvas;
    public GameObject buildingBtnPrefab;
    public CompoundButton[] textButtons;
    public CompoundButton[] changePageButtons;
    public TextMeshPro pageNumberText;

    [Header("Text Materials")]
    public Material On;
    public Material Off;



    private List<BuildingMap> Buildings;
    private float iconSize = 0.0391f;


    private int currentPage = 1;
    private int CurrentPage
    {
        get
        {
            return currentPage;
        }
        set
        {
            currentPage = value;
            PageChanged();
        }
    }


    private int max;


    private void Start()
    {
        foreach (CompoundButton button in textButtons)
        {
            button.OnButtonClicked += TextButtonPressed;
        }

        foreach (CompoundButton button in changePageButtons)
        {
            button.OnButtonClicked += ChangePagePressed;
        }
    }
    
    void BuildingPressed(GameObject go)
    {
        if (!UIManager.Instance.targetTile) return;

        WorldGridTile targetTile = UIManager.Instance.targetTile;

        int gridID = 0;
        gridID = targetTile.ParentGrid.Id;
        Vector2Int pos = targetTile.Position;

        TileEntity tileEnt = Activator.CreateInstance(Buildings[go.GetComponent<BuildingButton>().index].BuildingType) as TileEntity;

        if (targetTile.Model)
        {
            targetTile.UpdateModel(tileEnt.GetModel());
        }
        else
        {
            Game.CurrentSession.City.GetGrid(gridID).AddTileEntityToTile(pos.x, pos.y, tileEnt);
        }


        //Take money
        Game.CurrentSession.TakeFunds(Buildings[go.GetComponent<BuildingButton>().index].Cost);
        DestroyBuildingButtons();
        UIManager.Instance.SwitchState(UIManager.MenuState.Off);
    }

    public void DestroyBuildingButtons()
    {
        //Clear the page before we spawn the set of buttons.
        foreach (Transform child in buildingMenuCanvas)
        {
            if (child.GetComponent<CompoundButton>())
                child.GetComponent<CompoundButton>().OnButtonClicked -= BuildingPressed;

            Destroy(child.gameObject);
        }
    }

    void GetBuildings(BuildingCategory category)
    {
        Buildings = BuildingLibrary.GetListForTimePeriod(category);

        max = (int)Math.Ceiling(Buildings.Count / 6f);
        


        GenerateBuildingButtons();
    }

    void GenerateBuildingButtons()
    {
        DestroyBuildingButtons();

        int start = CurrentPage == 1 ? 0 : (6 * (CurrentPage-1));
        int end = (start + 6) <= Buildings.Count ? (start + 6) : Buildings.Count;

       
        for (int i = start; i < end; i++)
        {
            if (i >= Buildings.Count) break;

            GameObject go = Instantiate(buildingBtnPrefab, buildingMenuCanvas);
            BuildingButton buildingButton = go.GetComponentInChildren<BuildingButton>();

            GameObject model = Instantiate(Buildings[i].Model, buildingButton.modelParent.transform);
            model.transform.localPosition = Vector3.zero;

            buildingButton.ScaleModel(iconSize);

            buildingButton.index = i;
            buildingButton.costText.text = "£" + Buildings[i].Cost.ToString();
            buildingButton.nameText.text = Buildings[i].Name;
            go.SetActive(true);

            buildingButton.GetComponent<CompoundButton>().OnButtonClicked += BuildingPressed;
        }

        UIManager.Instance.BuildingsGenerated(Buildings.Count);
    }


    void TextButtonPressed(GameObject go)
    {
        string Name = "";

        foreach (CompoundButton textButton in textButtons)
        {
            if (textButton == go.GetComponent<CompoundButton>())
            {
                textButton.GetComponent<TextButton>().ChangeMaterial(On);
                textButton.ButtonState = ButtonStateEnum.Pressed;
                Name = textButton.name.ToLower();
            }
            else
            {
                textButton.GetComponent<TextButton>().ChangeMaterial(Off);
            }
        }

        ChangeBuildingCategory(Name);
    }

    void ChangeBuildingCategory(string name)
    {
        CurrentPage = 1;

        switch (name)
        {
            case "commercial":
                GetBuildings(BuildingCategory.Commercial);
                break;
            case "resource":
                GetBuildings(BuildingCategory.Resource);
                break;
            case "residential":
                GetBuildings(BuildingCategory.Residential);
                break;
            default:
                GetBuildings(BuildingCategory.All);
                break;
        }
    }

    public void ShowAllBuildings()
    {
        TextButtonPressed(textButtons[0].gameObject);
    }

    void ChangePagePressed(GameObject go)
    {
        if (go.name.ToLower() == "forward")
        {
            if(CurrentPage < max)
            {
                CurrentPage++;
                GenerateBuildingButtons();
            }
        }
        else if (go.name.ToLower() == "back")
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                GenerateBuildingButtons();
            }
        }
    }

    void PageChanged()
    {
        //update number text;
        pageNumberText.text = currentPage.ToString();
    }
}
