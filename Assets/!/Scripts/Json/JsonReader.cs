using System.IO;
using UnityEngine;

namespace @_.Scripts.Json
{
    public static class JsonReader
    {
        public static JsonData GetJsonData()
        {
            try
            {
                string streamingAssetsPath = Application.streamingAssetsPath;
                string filePath = Path.Combine(streamingAssetsPath, "result.json");

                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);

                    JsonData jsonData = JsonUtility.FromJson<JsonData>(json);
                    return jsonData;
                }

                Debug.LogError("File not found: " + filePath);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error reading JSON file: " + e.Message);
            }

            return null;
        }
    }
}