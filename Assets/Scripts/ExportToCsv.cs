using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class ExportToCsv : MonoBehaviour, IInputClickHandler {

    Dictionary<string, float> new_data;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        new_data = PlotBars.instance.ReadGraphValues();
        WriteIntoFile();
    }

    void WriteIntoFile()
    {
        Debug.Log("Writing into NewData.csv in Resources Folder");
        string text;
        string[] titles = ReadCSV.instance.titles;
        text = titles[0] + "," + titles[1]+"\n";
        foreach(var item in new_data)
        {
            string temp = item.Key + "," + (item.Value.ToString()) + "\n";
            text = text + temp;
        }

        //Debug.Log(text);

        System.IO.File.WriteAllText("Assets/Resources/NewData.csv", text);
    }



}
