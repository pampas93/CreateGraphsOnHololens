using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawAxisLabels : MonoBehaviour {

    private int ticks;
    private int yMax;

    public DrawAxisLabels(int ticksNumber, int yMaximum)
    {
        ticks = ticksNumber;
        yMax = yMaximum;
    }

    public void drawAxis()
    {
        
        //var extents = this.gameObject.GetComponent<Renderer>().bounds.extents;
        var valueGap = yMax / ticks;
        var gap = (float) 1.0f / ticks;
        float initialPosition = -0.5f;
        
        for (int i = 0; i <= ticks; i++)
        {
            GameObject cross_axis = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cross_axis.transform.parent = this.gameObject.transform;
            cross_axis.name = "crossAxis";

            Vector3 axisScale = new Vector3(1.0f, 0.005f, 0.005f);
            cross_axis.transform.localScale = axisScale;

            Vector3 axisPosition = new Vector3(0.0f, initialPosition, 0.0f);
            cross_axis.transform.localPosition = axisPosition;

            initialPosition = initialPosition + gap;
        }


        //float bar_height = item.Value / yMax;

        //Vector3 cubeScale = new Vector3(barWidth, bar_height, barWidth * 10);
        //bar.transform.localScale = cubeScale;

        //var barSize = bar.GetComponent<Renderer>().bounds.size;
        //var barExtents = bar.GetComponent<Renderer>().bounds.extents;
        //var temp = barSize.y + bottomAxisPos;

        //var yPos = temp - barExtents.y;
        //Vector3 cubePos = new Vector3(initial_pos, yPos, -0.2f);
        //bar.transform.localPosition = cubePos;
    }

}
