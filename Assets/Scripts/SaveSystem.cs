using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static string path = "/save.dat";

    public static void Save(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string saveTo = Application.persistentDataPath + path;

        FileStream stream = new(saveTo, FileMode.Create);
        formatter.Serialize(stream, JsonUtility.ToJson(data));
        stream.Close();
    }

    public static SaveData Load()
    {
        string loadFrom = Application.persistentDataPath + path;

        if (File.Exists(loadFrom))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new(loadFrom, FileMode.Open);

            SaveData value = JsonUtility.FromJson<SaveData>((string) formatter.Deserialize(stream));
            stream.Close();

            return value;
        }
        else
        {
            Save(new SaveData());
            return Load();
        }

    }
}
