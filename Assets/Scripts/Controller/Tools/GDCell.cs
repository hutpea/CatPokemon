using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GDCell : MonoBehaviour
{
    public CELL_TYPE cellType;
    public int catType;

    public Dropdown dropdownCellType;
    public Dropdown dropdownAnimalType;

    private void Awake()
    {
        dropdownCellType = transform.Find("DropdownCellType").GetComponent<Dropdown>();
        dropdownAnimalType = transform.Find("DropdownAnimalType").GetComponent<Dropdown>();

        dropdownCellType.onValueChanged.AddListener(delegate
        {
            OnDropdownCellTypeChanged();
        });

        dropdownAnimalType.onValueChanged.AddListener(delegate
        {
            OnDropdownAnimalTypeChanged();
        });
    }

    public void OnDropdownCellTypeChanged()
    {
        cellType = (CELL_TYPE)Enum.Parse(typeof(CELL_TYPE), dropdownCellType.options[dropdownCellType.value].text);
    }

    public void OnDropdownAnimalTypeChanged()
    {
        catType = (int)Enum.Parse(typeof(int), dropdownAnimalType.options[dropdownAnimalType.value].text);
    }

    public string ConvertToJsonCell()
    {
        string jsonCell = "";

        switch (cellType)
        {
            case CELL_TYPE.Animal:
                {
                    jsonCell += "a";
                    jsonCell += catType;

                    break;
                }
            case CELL_TYPE.Empty:
                {
                    jsonCell = "e";
                    break;
                }
        }

        return jsonCell;
    }
}

