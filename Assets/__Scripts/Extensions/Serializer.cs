using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class Serializer
{
    public static void Save<T>(T toSave, string fileName, string path = "")
    {
        string savePath = Path.Combine(Application.persistentDataPath, path);

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
        string savePath = Path.Combine(Application.persistentDataPath, path);

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

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
}
