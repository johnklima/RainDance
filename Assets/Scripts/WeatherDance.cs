using UnityEngine;
using System.Data;
using System.IO;
using System;
using static UnityEngine.Rendering.DebugUI;



public class WeatherDance : MonoBehaviour
{

    public string filename;
    public DataTable Data;

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
                Debug.Log(col + " " + item);
                col++;
            }
           
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
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
