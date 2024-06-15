using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void Save(string path, SaveData data) {
        BinaryFormatter formatter = new BinaryFormatter();
        string saveTo = Application.persistentDataPath + path;

        FileStream stream = new FileStream(saveTo, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData Load(string path) {
        string loadFrom = Application.persistentDataPath + path;

        if (File.Exists(loadFrom)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(loadFrom, FileMode.Open);

            SaveData value = (SaveData)formatter.Deserialize(stream);
            stream.Close();

            return value;
        }

        return new SaveData();
    }
}
