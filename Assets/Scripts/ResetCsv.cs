using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class ResetCsv : MonoBehaviour, IInputClickHandler {


    public void OnInputClicked(InputClickedEventData eventData)
    {
        ResetBarHeights();
    }

    private void ResetBarHeights()
    {
        Dictionary<string, float> original_data = ReadCSV.instance.returnOriginalDataSet();

        PlotBars.instance.resetBarGraph(original_data);
    }

    
}
