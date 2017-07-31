using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class ReadCSV : MonoBehaviour {

    [SerializeField]
    private string relativeFilePath = "Assets/Resources/SampleCsvForUnity.csv";

    public string[] AddIntoDictionary()
    {
        bool readTitles = false;
        string[] titles = { "", "" };

        Dictionary<string, float> data_set = new Dictionary<string, float>();

        relativeFilePath = "Assets/Resources/SampleCsvForUnity.csv";
        Debug.Log("Reading Data from " + relativeFilePath);

        // convert string to stream
        byte[] byteArray = Encoding.UTF8.GetBytes(relativeFilePath);
        //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
        MemoryStream stream = new MemoryStream(byteArray);
        // convert stream to string

        using (var reader = new StreamReader(stream))
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

        PlotBars.data_set = data_set;

        return titles;
    }

}
