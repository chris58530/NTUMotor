using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UnityEngine;

public class CSV : MonoBehaviour
{
    public static DataTable OpenCSV()
    {
        string filePath = Application.streamingAssetsPath + "//CSVTest.csv";
        return GetCSV(filePath);
    }

    private  static DataTable GetCSV(string filePath)
    {
        DataTable dt = new DataTable();
        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                string strLine = "";
                string[] aryLine = null;
                string[] tableHead = null;
                int columnCount = 0;
                bool isFirst = true;
                while ((strLine = sr.ReadLine()) != null)
                {
                    if (isFirst)
                    {
                        tableHead = strLine.Split(",");
                        isFirst = false;
                        columnCount = tableHead.Length;
                        for (int i = 0; i < columnCount; i++)
                        {
                            DataColumn dc = new DataColumn(tableHead[i]);
                            dt.Columns.Add(dc);
                        }
                    }
                    else
                    {
                        aryLine = strLine.Split(",");
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < columnCount; j++)
                        {
                            dr[j] = aryLine[j];
                        }

                        dt.Rows.Add(dr);
                    }
                }

                if (aryLine != null && aryLine.Length > 0)
                {
                    dt.DefaultView.Sort = tableHead[0] + " " + "asc";
                }

                sr.Close();
                fs.Close();
                return dt;
            }
        }
    }
}