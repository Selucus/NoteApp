using UnityEngine;
using System.IO;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DrawingData {
    public List<Vector3> positions;
    public float width;
    public Color color;
}

[System.Serializable]
public class DrawingsData {
    public List<DrawingData> drawings;
}
public class SaveScene : MonoBehaviour 
{
    /*
    public void SaveDrawings(){
        GameObject[] allDrawings = GameObject.FindGameObjectsWithTag("Drawing");
        foreach (GameObject go in allDrawings){
            Debug.Log(go.GetComponent<LineRenderer>().GetPositions());
        }
    }*/
    public GameObject saveFilename;
    public GameObject loadFilename;
    public GameObject brush;

    void Start(){
        if(PlayerPrefs.GetInt("Load or not") == 1){
            string filePath = Application.dataPath.Replace ("Hackathon.app/Data", "/Documents/") + "" + PlayerPrefs.GetString("Scene Name") + ".txt";
            filePath = "/Documents/" + PlayerPrefs.GetString("Scene Name") + ".txt";
            saveFilename.GetComponent<TMP_InputField>().text = PlayerPrefs.GetString("Scene Name");
            if (!File.Exists(filePath)) {
                Debug.LogWarning("File not found: " + filePath);
                return;
            }

            string json = File.ReadAllText(filePath);
            DrawingsData allDrawingsData = JsonUtility.FromJson<DrawingsData>(json);

            foreach (DrawingData drawingData in allDrawingsData.drawings) {
                // Create a new GameObject for each drawing
                GameObject newDrawing = Instantiate(brush);
                LineRenderer lineRenderer = newDrawing.GetComponent<LineRenderer>();
                
                lineRenderer.positionCount = drawingData.positions.Count;
                lineRenderer.SetPositions(drawingData.positions.ToArray());

                lineRenderer.startColor = drawingData.color;
                lineRenderer.endColor = drawingData.color;
                lineRenderer.startWidth = drawingData.width;
                lineRenderer.endWidth = drawingData.width;

            }
        }

        PlayerPrefs.SetInt("Load or not",0);
        
    }
    public void SaveDrawingsToFile() {
        string filePath = Application.dataPath.Replace ("Hackathon.app/Data", "/Documents/") + "" + saveFilename.GetComponent<TMP_InputField>().text + ".txt";
        filePath = "/Documents/" + saveFilename.GetComponent<TMP_InputField>().text + ".txt";
        GameObject[] allDrawings = GameObject.FindGameObjectsWithTag("Drawing");
        DrawingsData allDrawingsData = new DrawingsData();
        allDrawingsData.drawings = new List<DrawingData>();

        foreach (GameObject go in allDrawings) {
            LineRenderer lineRenderer = go.GetComponent<LineRenderer>();
            if (lineRenderer != null) {
                int positionCount = lineRenderer.positionCount;
                Vector3[] positions = new Vector3[positionCount];
                lineRenderer.GetPositions(positions);

                DrawingData drawingData = new DrawingData();
                drawingData.positions = new List<Vector3>(positions);
                drawingData.width = lineRenderer.startWidth;
                drawingData.color = lineRenderer.startColor;
                allDrawingsData.drawings.Add(drawingData);
            }
        }

        string json = JsonUtility.ToJson(allDrawingsData, true);
        File.WriteAllText(filePath, json);
        Debug.Log($"Drawings saved to {filePath}");
    }

    public void LoadDrawingsFromFile() {
        
        string filePath = Application.dataPath.Replace ("Hackathon.app/Data", "/Documents/") + "" + loadFilename.GetComponent<TMP_InputField>().text + ".txt";
        filePath = "/Documents/" + loadFilename.GetComponent<TMP_InputField>().text + ".txt";
        // "/SaveFiles/"
        if (!File.Exists(filePath)) {
            Debug.LogWarning("File not found: " + filePath);
            return;
        }

        string json = File.ReadAllText(filePath);
        DrawingsData allDrawingsData = JsonUtility.FromJson<DrawingsData>(json);

        foreach (DrawingData drawingData in allDrawingsData.drawings) {
            // Create a new GameObject for each drawing
            GameObject newDrawing = Instantiate(brush);
            LineRenderer lineRenderer = newDrawing.GetComponent<LineRenderer>();
            
            lineRenderer.positionCount = drawingData.positions.Count;
            lineRenderer.SetPositions(drawingData.positions.ToArray());

            lineRenderer.startColor = drawingData.color;
            lineRenderer.endColor = drawingData.color;
            lineRenderer.startWidth = drawingData.width;
            lineRenderer.endWidth = drawingData.width;

        }

        Debug.Log("Drawings loaded from " + filePath);
    }

    public void GoHome(){
        SceneManager.LoadScene("MainMenu");
    }
}
