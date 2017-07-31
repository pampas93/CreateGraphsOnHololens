using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class ReadCSV : MonoBehaviour {

    public static ReadCSV instance;

    [Tooltip("The Titles of the data")]
    [HideInInspector]
    public string[] titles = { "", "" };

    [Tooltip("Original data in dictionary format")]
    [HideInInspector]
    public Dictionary<string, float> data_set;

    //[Tooltip("The new data, which is used when export option or transformed.")]
    //public Dictionary<string, float> new_data_set;

    [Tooltip("The path of csv file")]
    string relativeFilePath = "Assets/Resources/SampleCsvForUnity.csv";
    
    private void Awake()
    {
        instance = this;      
    }

    private void Start()
    {
        AddIntoDictionary();
    }

    private void AddIntoDictionary()
    {
        bool readTitles = false;

        data_set = new Dictionary<string, float>();

        Debug.Log("Reading Data from " + relativeFilePath);

        //// convert string to stream
        //byte[] byteArray = Encoding.UTF8.GetBytes(relativeFilePath);
        //MemoryStream stream = new MemoryStream(byteArray);

        using (var reader = new StreamReader(relativeFilePath))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                if (!readTitles)    //if readTitles = false, I capture the titles first (since first line of csv always has titles)
                {
                    titles[0] = values[0];
                    titles[1] = values[1];
                    readTitles = true;
                }
                else             //Capture store into dictionary
                {
                    float tempValue = (float)Convert.ToDouble(values[1]);
                    data_set.Add(values[0], tempValue);
                }
            }
        }
    }

    public Dictionary<string, float> returnOriginalDataSet()
    {
        return data_set;
    }

    public string[] returnTitles()
    {
        return titles;
    }

}
