using UnityEngine;
using System.Data;
using System.IO;
using System;
using System.Collections.Generic;


public class WeatherDance : MonoBehaviour
{

    public string filename;
    public DataTable Data;
    public List<float> windDir = new List<float>();
    public List<float> rainHour = new List<float>();
    public List<float> outTemp = new List<float>();
    public List<float> windStr = new List<float>();



    public float facing = 0;
    
    
    float timer = -1;
    public int curRec = 155;   //start here
    Quaternion lastRot;
    Quaternion nextRot;

    public float lastHiphop = 0;
    public float hiphop = 0;

    public float lastRumba = 0;
    public float rumba = 0;

    public float lastSilly = 0;
    public float silly = 0;


    Animator anim;

    //going to simply say that Y axis rot = 0, Z forward, is "north" in the case of wind direction
    //date and time don't matter, as I only care about the intervals, the record numbers
    //then everything else in the data can become a float
    
     //the columns
    enum COLS
    {
        RecNo,	
        DateTime,
        InTemp,
        InHum,	
        OutTemp,	
        OutHum,	
        OutDew,	
        OutFeel,	
        Wind,	
        Gust,	
        WindDir,	
        AbsPressure,	
        RelPressure,	
        RainHour,	
        RainDay,
        RainWeek,	
        RainMonth,	
        RainTotal
    }

     

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        anim = GetComponent<Animator>();


        filename = Application.dataPath + "/Data/weather.txt";
        Data = ReadFile(filename,'|', true);

        // Print Data of Datatable
        foreach (DataRow dataRow in Data.Rows)
        {
            int col = 0;
            foreach (var item in dataRow.ItemArray)
            {
                //so, which columns am I interested in?
                Debug.Log(col + " " + item);
                if(col == (int) COLS.WindDir)
                {
                    //this is totally fuupt!
                    float f = float.Parse(item.ToString());

                    windDir.Add(f);
                }

                if(col == (int)COLS.RainHour)
                {
                    //this is totally fuupt!
                    float f = float.Parse(item.ToString());

                    rainHour.Add(f);
                }

                if(col == (int)COLS.OutTemp)
                {
                    //this is totally fuupt!
                    float f = float.Parse(item.ToString());
                    outTemp.Add(f);
                }

                if (col == (int)COLS.Wind)
                {
                    //this is totally fuupt!
                    float f = float.Parse(item.ToString());
                    windStr.Add(f);
                }

                col++;
            }
           
        }
        
        //lets get a min/max on rain normalized to 1.0f
        float min = 10000f;
        float max = 0;
        for(int i = 0; i < rainHour.Count; i++)
        {
            if (rainHour[i] > max)
            {
                max = rainHour[i];
            }
            if (rainHour[i] < min)
            {
                min = rainHour[i];
            }

        }
        //so now we have the biggest value, we normalize the values to this, 0 to 1.0f
        for (int i = 0; i < rainHour.Count; i++)
        {
            //if max is whatever, then max over max == 1.0f?
            rainHour[i] = rainHour[i] / max;
            
        }

        //do the same for other columns...
        min = 10000f;
        max = 0;
        for (int i = 0; i < outTemp.Count; i++)
        {
            if (outTemp[i] > max)
            {
                max = outTemp[i];
            }
            if (outTemp[i] < min)
            {
                min = outTemp[i];
            }
        }
        //in the case of temperature, max - min is the whole range
        float r = max - min;
        for (int i = 0; i < outTemp.Count; i++)
        {
            outTemp[i] = (outTemp[i] - min) / r;   //min record - min = 0, div by range = 1 max ?
        }

        //and once more for wind strength, time to make a function!

        min = 10000f;
        max = 0;
        for (int i = 0; i < windStr.Count; i++)
        {
            if (windStr[i] > max)
            {
                max = windStr[i];
            }
            if (windStr[i] < min)
            {
                min = windStr[i];
            }
        }
        //in the case of wind, max - min is the whole range
        r = max - min;
        for (int i = 0; i < windStr.Count; i++)
        {
            windStr[i] = (windStr[i] - min) / r;   //min record - min = 0, div by range = 1 max ?
        }


        //get our start values
        timer = Time.time;              //now
        lastRot = transform.rotation;   //looking where
        nextRot = lastRot;              //and to initialize...

    }

    // Update is called once per frame
    float lerptime = 0;
    void Update()
    {
        if(Time.time - timer >= 1.0f)
        {
            timer = Time.time;                          //reset
            lastRot = transform.rotation;               //looking where now
            float y = windDir[curRec];                  //get data rec
            y += facing;                                //add my offset
            nextRot = Quaternion.Euler(0, y, 0);        //looking where next
            lerptime = 0;                               //reset lerp

            //anim blend is simpler
            lastHiphop = hiphop;
            hiphop = rainHour[curRec];

            lastRumba = rumba;
            rumba = outTemp[curRec];

            lastSilly = silly;
            silly = windStr[curRec];

            //increment and check
            curRec++;
            if(curRec >= windDir.Count )
            {
                curRec = 0;
            }



        }

        //simply interpolate by one second intervals over DT
        
        lerptime += Time.deltaTime;
        if (lastRot != nextRot)
        {
            transform.rotation = Quaternion.Lerp(lastRot, nextRot, lerptime);
        }

        if(hiphop!=lastHiphop)
        {
            float f = Mathf.Lerp(lastHiphop, hiphop, lerptime);
            anim.SetFloat("HipHopDancing", f);
        }

        if (rumba != lastRumba)
        {
            float f = Mathf.Lerp(lastRumba, rumba, lerptime);
            anim.SetFloat("RumbaDancing", f);
        }

        if (silly != lastSilly)
        {
            float f = Mathf.Lerp(lastSilly, silly, lerptime);
            anim.SetFloat("SillyDancing", f);
        }


    }

    public DataTable ReadFile(string FilePath, char delimiter, bool isFirstRowHeader = true)
    {

        try
        {
            //Create object of Datatable
            DataTable objDt = new DataTable();

            //Open and Read Delimited Text File
            using (TextReader tr = File.OpenText(FilePath))
            {
                string line;
                //Read all lines from file
                while ((line = tr.ReadLine()) != null)
                {
                    //Debug.Log(line);

                    //split data based on delimiter/separator and convert it into string array
                    string[] arrItems = line.Split(delimiter);

                    //Check for first row of file
                    if (objDt.Columns.Count == 0)
                    {
                        // If first row of text file is header then, we will not put that data into datarow and consider as column name 
                        if (isFirstRowHeader)
                        {
                            for (int i = 0; i < arrItems.Length; i++)
                                objDt.Columns.Add(new DataColumn(Convert.ToString(arrItems[i]), typeof(string)));
                            continue;
                        }
                        else
                        {
                            // Create the data columns for the data table based on the number of items on the first line of the file
                            for (int i = 0; i < arrItems.Length; i++)
                                objDt.Columns.Add(new DataColumn("Column" + Convert.ToString(i), typeof(string)));

                        }
                    }

                    //Insert data from text file to datarow
                    objDt.Rows.Add(arrItems);
                }
            }
            //return datatable
            return objDt;
        }
        catch (Exception)
        {
            throw;
        }
    }


}
