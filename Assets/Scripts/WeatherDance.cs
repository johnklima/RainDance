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

    float timer = -1;
    int curRec = 0;
    Quaternion lastRot;
    Quaternion nextRot;

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
                    //this is totally fucked up!
                    float f = float.Parse(item.ToString());

                    windDir.Add(f);
                }

                if(col == (int)COLS.RainHour)
                {
                    //this is totally fucked up!
                    float f = float.Parse(item.ToString());

                    rainHour.Add(f);
                }

                col++;
            }
           
        }
        //get our start values
        timer = Time.time;              //now
        lastRot = transform.rotation;   //looking where
        nextRot = lastRot;              //and to initialize...

        //lets get a min/max on rain normalized to 1.0f
        float min = 0;
        float max = 0;
        for(int i = 0; i < rainHour.Count; i++)
        {
            if (rainHour[i] > max)
            {
                max = rainHour[i];
            }
        }
        //so now we have the biggest value, we normalize the values to this, 0 to 1.0f
        for (int i = 0; i < rainHour.Count; i++)
        {
            //if max is whatever, then max over max == 1.0f?
            rainHour[i] = rainHour[i] / max;
            
        }


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
            nextRot = Quaternion.Euler(0, y, 0);        //looking where next
            lerptime = 0;                               //reset lerp
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
