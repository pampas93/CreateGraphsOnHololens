using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotBars : MonoBehaviour {

    private float bar_gap = 0.03f;
    private float initial_pos = -0.45f;  //locally, its starting 0.1 from the y axis //because y axis is at -0.5
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
        data_set.Add("Pemps Cowboy", 28.2f);
        data_set.Add("Mind body soul", 50f);
        data_set.Add("Axe", 32.4f);
        data_set.Add("Cowboy", 48.2f);
        data_set.Add("Dovehunt", 40f);
        //data_set.Add("Caribbean", 48.2f);     //Extra data just to test
        //data_set.Add("Cass's", 40f);
        //data_set.Add("sen", 39.6f);
        //data_set.Add("Pemp", 28.2f);
        //data_set.Add("Mind", 50f);
        //data_set.Add("Dohunt", 40f);
        //data_set.Add("Cabbean", 48.2f);
        //data_set.Add("ss's", 40f);
        //data_set.Add("s", 39.6f);
        //data_set.Add("Pep", 28.2f);
        //data_set.Add("Min", 50f);
        //data_set.Add("'s Mist", 51.8f);
        //data_set.Add("A", 32.4f);
        //data_set.Add("B", 48.2f);
        //data_set.Add("C", 40f);
        //data_set.Add("D", 39.6f);
        //data_set.Add("E", 28.2f);

        if (data_set.Count < 7)
            plotGraph();
        else
            plotSmallerGraph();

    }

    void plotGraph()
    {
        int yMin = 0;
        int yMax = calculateAxis();
        //Debug.Log(yMax);
        float barWidth = 0.1f;

        foreach (var item in data_set)
        {
            //Debug.Log(item.Key + "  " + item.Value);
            GameObject bar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bar.transform.parent = this.gameObject.transform;
            bar.name = item.Key;

            float bar_height = item.Value / yMax;

            
            Vector3 cubeScale = new Vector3(barWidth, bar_height, barWidth*10);     //z scale = barWidth*10 only to keep the bar depth also proportional to bar
            bar.transform.localScale = cubeScale;

            //To position  the bar on top of X axis
            var barSize = bar.GetComponent<Renderer>().bounds.size; 
            var barExtents = bar.GetComponent<Renderer>().bounds.extents;
            var temp = barSize.y + bottomAxisPos;
            //Debug.Log(barExtents.y);

            var yPos = temp - barExtents.y;
            Vector3 cubePos = new Vector3(initial_pos, yPos, -0.2f);
            bar.transform.localPosition = cubePos;
            

            initial_pos = initial_pos + bar_gap + barWidth;
        }
    }

    void plotSmallerGraph()
    {
        Debug.Log("Executing bigger graph");
        int yMin = 0;
        int yMax = calculateAxis();

        //need to recalculate bar_gap and barWidth based on number of items
        float barWidth = CalculateBarWidth();

        foreach (var item in data_set)
        {
            GameObject bar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bar.transform.parent = this.gameObject.transform;
            bar.name = item.Key;

            float bar_height = item.Value / yMax;

            Vector3 cubeScale = new Vector3(barWidth, bar_height, barWidth*10);
            bar.transform.localScale = cubeScale;

            var barSize = bar.GetComponent<Renderer>().bounds.size;
            var barExtents = bar.GetComponent<Renderer>().bounds.extents;
            var temp = barSize.y + bottomAxisPos;

            var yPos = temp - barExtents.y;
            Vector3 cubePos = new Vector3(initial_pos, yPos, -0.2f);
            bar.transform.localPosition = cubePos;


            initial_pos = initial_pos + bar_gap + barWidth;
        }
    }

    float CalculateBarWidth()
    {
        float bar_width = 0.1f;
        int count = data_set.Count;
        //Formula->   (count*bar_width) + (count*bar_gap) = 0.9     //0.9 because, initially, we use 0.1 to have a gap between yAxis and first bar.
        //And bar_gap = bar_width/2

        //After simplifying above expression,
        bar_width = (float) (0.95 / (count * 1.5));
        Debug.Log(bar_width);

        //Need to change bar_gap value
        bar_gap = (float) (bar_width / 2.0);

        return bar_width;
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
