using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Prime31.TransitionKit;

public class SaveLoad : MonoBehaviour
{

    public static void Save()
    {
        var scene = SceneManager.GetActiveScene().buildIndex;
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.dat";

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, scene);
        stream.Close();
    }
    public void Load()
    {
        string path = Application.persistentDataPath + "/save.dat";
        if (File.Exists(path))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            var scene = formatter.Deserialize(stream);

            var enumValues = System.Enum.GetValues(typeof(PixelateTransition.PixelateFinalScaleEffect));


           
            var vertical = new VerticalSlicesTransition()
            {
                nextScene = (int)scene,
                duration = 1.0f
            };
            TransitionKit.instance.transitionWithDelegate(vertical);

            stream.Close();

        }
        else
        {
            Debug.LogError("Save file not found!");
        }
    }

}
