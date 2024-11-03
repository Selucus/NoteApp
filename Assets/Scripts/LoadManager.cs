using UnityEngine;
using System.IO;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class LoadManager : MonoBehaviour 
{
    public GameObject dropdownList;
    public void DisplayFiles(){
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath.Replace ("Hackathon.app/Data", "/Documents/") + "/SaveFiles");
        dir = new DirectoryInfo("/Documents/");

        FileInfo[] info = dir.GetFiles("*.txt");
        Debug.Log(dir);
        List<string> fileNames = new List<string>();
        foreach (FileInfo f in info) 
        {
            fileNames.Add(f.Name.Substring(0,f.Name.Length-4));
        }
        dropdownList.GetComponent<TMP_Dropdown>().AddOptions(fileNames);
    }

    void Start(){
        DisplayFiles();
    }

    public void LoadBlank(){
        PlayerPrefs.SetInt("Load or not",0); // use 0 for not load and 1 for load
        SceneManager.LoadScene("DrawingScene");
    }
    public void LoadScene(){
        
        PlayerPrefs.SetString("Scene Name",dropdownList.GetComponent<TMP_Dropdown>().options[dropdownList.GetComponent<TMP_Dropdown>().value].text);
        PlayerPrefs.SetInt("Load or not",1); // use 0 for not load and 1 for load
        SceneManager.LoadScene("DrawingScene");
    }
}
