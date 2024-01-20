using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    public static DataTable ReadCSV(string fileName)
    {
        // 取得StreamingAssets資料夾的路徑
        string streamingAssetsPath = Application.streamingAssetsPath;

        // 組合完整的檔案路徑
        string filePath = Path.Combine(streamingAssetsPath, fileName);

        try
        {
            // 使用StreamReader讀取CSV檔案
            using (StreamReader reader = new StreamReader(filePath))
            {
                // 創建 DataTable
                DataTable dataTable = new DataTable();

                // 讀取第一行，將列名加入 DataTable
                string[] columnNames = reader.ReadLine()?.Split(',');
                for (int i = 0; i < columnNames.Length; i++)
                {
                    string columnName = "Column_" + i; // 使用非純數字的列名
                    dataTable.Columns.Add(columnName.Trim());
                }

                // 讀取剩餘行，將數據加入 DataTable
                while (!reader.EndOfStream)
                {
                    string[] values = reader.ReadLine().Split(',');
                    DataRow row = dataTable.NewRow();

                    for (int i = 0; i < values.Length; i++)
                    {
                        row[i] = values[i].Trim();
                    }

                    dataTable.Rows.Add(row);
                }

                return dataTable;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error reading CSV file: " + e.Message);
            return null;
        }
    }
}
