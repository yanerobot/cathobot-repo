using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Newtonsoft.Json;

public static class Serializer
{
    public static void Save<T>(T toSave, string fileName, string path = "")
    {
        string savePath = Application.persistentDataPath + path;

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = new FileStream(savePath + fileName, FileMode.Create);
        bf.Serialize(file, toSave);
        file.Close();
        Debug.Log("Successfully saved file at: " + savePath + fileName);
    }

    public static bool TryLoad<T>(out T toLoad, string fileName, string path = "")
    {
        string savePath = Application.persistentDataPath + path;

        if (File.Exists(savePath + fileName))
        {
            Debug.Log("Loading file at: " + savePath + fileName);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(savePath + fileName, FileMode.Open);
            toLoad = (T)bf.Deserialize(stream);
            stream.Close();
            return true;
        }

        Debug.Log("File doesn't exist at :" + savePath + fileName);
        toLoad = default(T);
        return false;
    }

    public static string SerializeToString<T>(T obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    public static bool TryDeserializeString<T>(string str, out T result)
    {
        if (str == "")
        {
            result = default;
            return false;
        }

        var obj = JsonConvert.DeserializeObject(str, typeof(T));
        try
        {
            result = (T)obj;
            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
            result = default;
            return false;
        }
    }
}
