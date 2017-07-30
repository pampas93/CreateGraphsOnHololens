using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotBars : MonoBehaviour {

    private float bar_gap = 0.16f;
    private float initial_pos = -0.4f;
    private float bottomAxisPos;

    private int ticks = 5;
    private int eachSegHeight;

    Dictionary<string, float> data_set;

    // Use this for initialization
    void Start () {

        var centerPos = this.gameObject.GetComponent<Renderer>().bounds.center;
        var extents = this.gameObject.GetComponent<Renderer>().bounds.extents;
        bottomAxisPos = centerPos.y - extents.y;
        //Debug.Log(centerPos.y - extents.y);

        data_set = new Dictionary<string, float>();
        data_set.Add("Alexandra's Mist", 51.8f);
        data_set.Add("Axe Capital", 32.4f);
        data_set.Add("Caribbean Cowboy", 48.2f);
        data_set.Add("Cass's Dovehunt", 40f);
        data_set.Add("Chessen", 39.6f);

        plotGraph();

    }

    void plotGraph()
    {
        int yMin = 0;
        int yMax = calculateAxis();
        //Debug.Log(yMax);

        foreach (var item in data_set)
        {
            //Debug.Log(item.Key + "  " + item.Value);
            GameObject bar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bar.transform.parent = this.gameObject.transform;
            bar.name = item.Key;

            float bar_height = item.Value / yMax;

            
            Vector3 cubeScale = new Vector3(0.1f, bar_height, 1.0f);
            bar.transform.localScale = cubeScale;

            //To position  the bar on top of X axis
            var barSize = bar.GetComponent<Renderer>().bounds.size;
            var barExtents = bar.GetComponent<Renderer>().bounds.extents;
            var temp = barSize.y + bottomAxisPos;
            //Debug.Log(barExtents.y);

            var yPos = temp - barExtents.y;
            Vector3 cubePos = new Vector3(initial_pos, yPos, -0.2f);
            bar.transform.localPosition = cubePos;
            

            initial_pos = initial_pos + bar_gap;
        }
    }

    // To Calculate the MAximum height of the Y Axis
    int calculateAxis()
    {
        int yMax = 2;
        float upperBound = 0.0f, lowerBound = 0.0f;

        foreach (var item in data_set)
        {
            if (item.Value > upperBound)
                upperBound = item.Value;
        }

        float temp = (upperBound - lowerBound) / ticks;
        int x = (int)temp;

        if (x % 2 == 0)
            eachSegHeight = x + 2;
        else
            eachSegHeight = x + 1;

        yMax = eachSegHeight * ticks;

        return yMax;
    }
	
	
}
