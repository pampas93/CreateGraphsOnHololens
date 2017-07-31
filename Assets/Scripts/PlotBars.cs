using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotBars : MonoBehaviour {

    public static PlotBars instance;

    private void Awake()
    {
        instance = this;
    }

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
    public bool ReadFromCSV = true;

    Dictionary<string, float> local_data_set;

    [Serializable]
    public struct DataVariable
    {
        [Tooltip("The Variable name.")]
        public string data_name;
        [Tooltip("The Value for the Variable.")]
        public float data_value;
    }

    [Tooltip("The array of data values created. (Only when ReadFromCSV = false)")]
    public DataVariable[] dataSetArray;

    [Tooltip("X, Y Titles for the bar graph")]
    public string XAxisTitle;
    public string YAxisTitle;

    // Use this for initialization
    void Start () {

        var centerPos = this.gameObject.GetComponent<Renderer>().bounds.center;
        var extents = this.gameObject.GetComponent<Renderer>().bounds.extents;
        bottomAxisPos = centerPos.y - extents.y;
        //Debug.Log(centerPos.y - extents.y);

        AddToDictionary();

        yMax = calculateAxis();
        drawAxis();

        if (local_data_set.Count < 7)
            plotGraph();
        else
            plotSmallerGraph();
    }

    private void AddToDictionary()
    {
        titles = new string[2];

        if (ReadFromCSV)
        {

            local_data_set = ReadCSV.instance.returnOriginalDataSet();
            titles = ReadCSV.instance.returnTitles();
        }
        else
        {
            titles[0] = XAxisTitle;
            titles[1] = YAxisTitle;
            //Adding items from the dataSetArray (data taken when added in inspector)
            for (int i = 0; i < dataSetArray.Length; i++)
            {
                string name = dataSetArray[i].data_name;
                float value = dataSetArray[i].data_value;
                local_data_set.Add(name, value);
            }

            ReadCSV.instance.titles = titles;
            ReadCSV.instance.data_set = local_data_set;
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

        foreach (var item in local_data_set)
        {
            //Debug.Log(item.Key + "  " + item.Value);
            GameObject bar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bar.transform.parent = this.gameObject.transform;
            bar.name = item.Key; // + " with " + titles[1] + ": " + item.Value;
            bar.tag = "Bar";
            bar.GetComponent<Renderer>().material = offFocusMaterial;
            bar.AddComponent<editComponent>();

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

        foreach (var item in local_data_set)
        {
            GameObject bar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bar.transform.parent = this.gameObject.transform;
            bar.name = item.Key;  //+ " with " + titles[1] + ": " + item.Value; 
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
        int count = local_data_set.Count;
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

        foreach (var item in local_data_set)
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

    public float getUpdatedValue(GameObject obj)
    {
        float value;

        var height = obj.transform.localScale.y;
        value = height * yMax;

        return value;
    }

    public Dictionary<string, float> ReadGraphValues()
    {
        Dictionary<string, float> new_data_set = new Dictionary<string, float>();

        foreach (Transform t_child in transform)
        {
            GameObject child_obj = t_child.gameObject;
            if(child_obj.tag == "Bar")
            {
                float newHeight = child_obj.transform.localScale.y;
                float newValue = newHeight * yMax;
                //Debug.Log(child_obj.name + " has new value of " + newValue.ToString());

                new_data_set.Add(child_obj.name, newValue);
            }
            else
            {
                continue;
            }
        }
        return new_data_set;
    }

    public void resetBarGraph(Dictionary<string, float> original_set)
    {
        foreach(var item in original_set)
        {
            GameObject bar_obj = GameObject.Find(item.Key);
            float bar_height = item.Value / yMax;

            Vector3 lastscale = bar_obj.transform.localScale;
            bar_obj.transform.localScale = new Vector3(lastscale.x, bar_height, lastscale.z);

            var yPos = -0.5f + (bar_height / 2);
            Vector3 lastPos = bar_obj.transform.localPosition;
            bar_obj.transform.localPosition = new Vector3(lastPos.x, yPos, lastPos.z);

        }

    }
	
	
}
