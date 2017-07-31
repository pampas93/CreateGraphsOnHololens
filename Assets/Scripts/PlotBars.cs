using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotBars : MonoBehaviour {

    private float bar_gap = 0.065f;
    private float initial_pos = -0.4f;  //locally, its starting 0.1 from the y axis //because y axis is at -0.5
    private float bottomAxisPos;

    private int ticks = 5;
    private int eachSegHeight;
    private int yMax;

    private string[] titles;

    [SerializeField]
    Material offFocusMaterial;

    [Tooltip("Setting enables reading data from csv file")]
    public bool ReadFromCSV = false;

    public static Dictionary<string, float> data_set;

    [Serializable]
    public struct DataVariable
    {
        [Tooltip("The Variable name.")]
        public string data_name;
        [Tooltip("The Value for the Variable.")]
        public float data_value;
    }

    [Tooltip("The array of data values created.")]
    public DataVariable[] dataSetArray;

    // Use this for initialization
    void Start () {

        var centerPos = this.gameObject.GetComponent<Renderer>().bounds.center;
        var extents = this.gameObject.GetComponent<Renderer>().bounds.extents;
        bottomAxisPos = centerPos.y - extents.y;
        //Debug.Log(centerPos.y - extents.y);

        addToDictionary();

        yMax = calculateAxis();
        drawAxis();
        //DrawAxisLabels dxObj = new DrawAxisLabels(ticks, yMax);
        //dxObj.drawAxis();   //Creates Axis and labels

        if (data_set.Count < 7)
            plotGraph();
        else
            plotSmallerGraph();

    }

    private void addToDictionary()
    {
        data_set = new Dictionary<string, float>();
        titles = new string[2];

        if (!ReadFromCSV)
        {
            titles[0] = "Horse Name";
            titles[1] = "Average BSF";
            //Adding items from the dataSetArray (data taken when added in inspector)
            //for (int i=0; i < dataSetArray.Length; i++)
            //{
            //    string name = dataSetArray[i].data_name;
            //    float value = dataSetArray[i].data_value;
            //    data_set.Add(name, value);
            //}

            //Adding data into dictionary manually
            data_set.Add("Alexandra's Mist", 40f);
            data_set.Add("Axe Capital", 35f);
            data_set.Add("Caribbean Cowboy", 15f);
            data_set.Add("Cass's Dovehunt", 20f);
            data_set.Add("Chessen", 25f);
            data_set.Add("Pemps Cowboy", 30f);
            data_set.Add("Mind body soul", 5f);
            data_set.Add("Axe", 120.4f);
            data_set.Add("Cowboy", 48.2f);
            //data_set.Add("Dovehunt", 40f);
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
        }
        else
        {
            ReadCSV csvObj = new ReadCSV();
            titles = csvObj.AddIntoDictionary();
        }

    }

    private void drawAxis()
    {
        var gap = (float)1.0f / ticks;
        float initialPosition = -0.5f;
        var valueGap = yMax / ticks;

        for (int i = 0; i <= ticks; i++)
        {
            GameObject cross_axis = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cross_axis.transform.parent = this.gameObject.transform;
            cross_axis.name = "crossAxis";

            Vector3 axisScale = new Vector3(1.0f, 0.005f, 0.005f);
            cross_axis.transform.localScale = axisScale;

            Vector3 axisPosition = new Vector3(0.0f, initialPosition, 0.5f);
            cross_axis.transform.localPosition = axisPosition;

            //Creating textMesh Gameobject also;
            var axisValue = i * valueGap;

            GameObject textObj = new GameObject(axisValue.ToString());
            textObj.transform.parent = this.gameObject.transform;
            var t = textObj.AddComponent<TextMesh>();
            t.text = axisValue.ToString();
            textObj.transform.localScale = new Vector3(0.02f, 0.05f, 0.05f);
            textObj.transform.localPosition = new Vector3(-0.5f, initialPosition, 0.5f);

            initialPosition = initialPosition + gap;
        }
    }


    private void plotGraph()
    {
        //Debug.Log(yMax);
        float barWidth = 0.1f;

        foreach (var item in data_set)
        {
            //Debug.Log(item.Key + "  " + item.Value);
            GameObject bar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bar.transform.parent = this.gameObject.transform;
            bar.name = item.Key + " with " + titles[1] + ": " + item.Value;
            bar.tag = "Bar";
            bar.GetComponent<Renderer>().material = offFocusMaterial;

            float bar_height = item.Value / yMax;
            
            Vector3 cubeScale = new Vector3(barWidth, bar_height, barWidth*10);     //z scale = barWidth*10 only to keep the bar depth also proportional to bar
            bar.transform.localScale = cubeScale;

            //To position  the bar on top of X axis;
            //Debug.Log(barExtents.y);
            var yPos = -0.5f + (bar_height / 2);
            Vector3 cubePos = new Vector3(initial_pos, yPos, -0.2f);
            bar.transform.localPosition = cubePos;
            

            initial_pos = initial_pos + bar_gap + barWidth;
        }
    }

    private void plotSmallerGraph()
    {
        Debug.Log("Executing bigger graph");

        //need to recalculate bar_gap and barWidth based on number of items
        float barWidth = CalculateBarWidth();
        initial_pos = -0.45f + bar_gap;

        foreach (var item in data_set)
        {
            GameObject bar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bar.transform.parent = this.gameObject.transform;
            bar.name = item.Key + " with " + titles[1] + ": " + item.Value; 
            bar.tag = "Bar";
            bar.GetComponent<Renderer>().material = offFocusMaterial;
            bar.AddComponent<editComponent>();

            float bar_height = item.Value / yMax;

            Vector3 cubeScale = new Vector3(barWidth, bar_height, barWidth*10);
            bar.transform.localScale = cubeScale;

            //Debug.Log(bar_height);
            var yPos = -0.5f + (bar_height / 2);
            Vector3 cubePos = new Vector3(initial_pos, yPos, -0.2f);
            bar.transform.localPosition = cubePos;

            initial_pos = initial_pos + bar_gap + barWidth;
        }
    }

    //Incase more than 6 dataValues are present; accordingly shortedn the barWidth and bar_gap
    private float CalculateBarWidth()
    {
        float bar_width = 0.1f;
        int count = data_set.Count;
        //Formula->   (count*bar_width) + (count*bar_gap) = 0.9     //0.9 because, initially, we use 0.1 to have a gap between yAxis and first bar.
        //And bar_gap = bar_width/2

        //After simplifying above expression,
        bar_width = (float) (0.95 / (count * 1.5));
        //Debug.Log(bar_width);

        //Need to change bar_gap value
        bar_gap = (float) (bar_width / 2.0);

        return bar_width;
    }

    // To Calculate the MAximum height of the Y Axis
    private int calculateAxis()
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
