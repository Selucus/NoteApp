using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
public class DrawingLogic : MonoBehaviour 
{
    public Camera camera;
    public GameObject brush;

    LineRenderer lineRenderer;

    Vector2 lastPos;
    Color currentColour;

    Dictionary<string,Color> colourDict= new Dictionary<string, Color>();
    public GameObject widthSlider;
    
    

    void Start(){
        colourDict.Add("black",Color.black);
        colourDict.Add("white",Color.white);
        colourDict.Add("blue",Color.blue);
        colourDict.Add("red",Color.red);
        SetColour("black");
    }
    private void Update(){
        Draw();
    }
    void Draw(){
        // Check if the mouse is over a UI element
        if (EventSystem.current.IsPointerOverGameObject()) {
            return; // Do nothing if interacting with UI
        }
        
        if(Input.GetKeyDown(KeyCode.Mouse0) && Input.touchCount != 2){
            CreateBrush();
        }
        if(Input.GetKey(KeyCode.Mouse0) && Input.touchCount != 2){
            Vector2 mousePos = camera.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x,Input.mousePosition.y,10f));

            if(mousePos!=lastPos){
                AddAPoint(mousePos);
                lastPos = mousePos;
            }

            
        }
        else{
            lineRenderer = null;
        }
    }

    void CreateBrush(){
        GameObject brushInstance = Instantiate(brush);
        lineRenderer = brushInstance.GetComponent<LineRenderer>();
        lineRenderer.SetWidth(widthSlider.GetComponent<Slider>().value,widthSlider.GetComponent<Slider>().value);
        Vector2 mousePos = camera.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x,Input.mousePosition.y,10f));
        lineRenderer.SetColors(currentColour,currentColour);
        lineRenderer.SetPosition(0,mousePos);
        lineRenderer.SetPosition(1,mousePos);
    }

    void AddAPoint(Vector2 pointPos){
        
        lineRenderer.positionCount++;
        int positionIndex = lineRenderer.positionCount - 1;
        lineRenderer.SetPosition(positionIndex,pointPos);
    }

    public void SetColour(string colourString){
        currentColour = colourDict[colourString];
    }
}
