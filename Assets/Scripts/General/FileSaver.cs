using UnityEngine;
using System.IO; //For file read and write
using System.Runtime.Serialization.Formatters.Binary;

public static class FileSaver
{
    public static void Save(object data, string filePath)
    {
        Debug.Log("Saving" + filePath);
        FileStream file = File.Create(GeneratePath(filePath));
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(file, data);
        file.Close();
    }

    public static object Load(string filePath)
    {
        object data = null;
        if(File.Exists(filePath))
        {
            Debug.Log("Deserializing" + filePath);
            data = DeserializeFile(filePath);
        }
        return data;
    }

    private static string GeneratePath(string filePath)
    {
        return Application.persistentDataPath + "/" + filePath;
    }

    private static object DeserializeFile(string filePath)
    {
        FileStream file = File.Open(filePath, FileMode.Open);
        BinaryFormatter formatter = new BinaryFormatter();
        object data = formatter.Deserialize(file);
        file.Close();

        return data;
    }
}
