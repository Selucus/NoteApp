using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class CameraManager : MonoBehaviour
{
    public Rigidbody2D rb;

    public float calibrationTime = 5.0f;
    public Vector2 baseForce;
    public bool Calibrated = false;

    public Vector2 velocity;
    
    private int numNoMoveFrames;

    public float ZeroAccelerationThreshold;
    public int numNoMoveThreshold;
    public Vector2 movementSpeed;

    private Queue<float> Magnitudes = new Queue<float>(10);
    private int numZeros = 0;

    public float zoomOutMin = 1;
    public float zoomOutMax = 8;
    public GameObject countdown;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Calibrate());
        
        Magnitudes.Enqueue(0f);
        Magnitudes.Enqueue(0f);
        Magnitudes.Enqueue(0f);
        Magnitudes.Enqueue(0f);
        Magnitudes.Enqueue(0f);
        Magnitudes.Enqueue(0f);
        numZeros = 6;
    }
    
    void FixedUpdate()
    {
        if (Calibrated)
        {
            Vector2 force = GetForce();

            if (force.magnitude == 0)
            {
                numZeros++;
            }

            if (numZeros > numNoMoveThreshold) {
                rb.linearVelocity = Vector2.zero;
            }
            
            Vector2 velchange = (force - baseForce) * Time.fixedDeltaTime;

            rb.linearVelocity -=  new Vector2(velchange.x * movementSpeed.x, velchange.y * movementSpeed.y);

            Magnitudes.Enqueue(force.magnitude);
            
            if (Magnitudes.Dequeue() == 0) {
                numZeros--;
            }

   
        }
    }

    IEnumerator Calibrate()
    {
        Vector2 averageBase = new Vector2(0,0);
        countdown.SetActive(true);
        for (int i = 0; i < calibrationTime / Time.fixedDeltaTime; i++)
        {
            averageBase += GetForce();
            
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            countdown.GetComponent<TMP_Text>().text = (calibrationTime - (i * Time.fixedDeltaTime)).ToString();
        }
        
        baseForce = averageBase / (calibrationTime / Time.fixedDeltaTime);
        Calibrated = true;
        countdown.SetActive(false);
    }
    
    public Vector2 GetForce()
    {
        Vector2 force = new Vector2(0,0);
        
        foreach (var evnt in Input.accelerationEvents)
        {
            if (evnt.deltaTime != 0)
            {
                Vector3 currForce = evnt.acceleration / evnt.deltaTime;
            
                force += new Vector2(currForce.x, currForce.y);
            }
        }

        return lessThan(force, baseForce) || (force - baseForce).magnitude < ZeroAccelerationThreshold ? new Vector2(0,0) : force - baseForce;
    }
    void Update(){
        if(Input.touchCount == 2){
            Debug.Log("2 inputs");
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;  
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;  


            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;
            zoom(difference * 0.01f);
        }
        zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    bool lessThan(Vector2 a, Vector2 b)
    {
        return Mathf.Abs(a.x) < Mathf.Abs(b.x) && Mathf.Abs(a.y) < Mathf.Abs(b.y);
    }

    void zoom(float increment){
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin,zoomOutMax);
    }

    public void reset(){
        transform.position = new Vector3(0,0,-10);
        baseForce = Vector2.zero;
        velocity = Vector2.zero;

        Calibrated = false;
        StartCoroutine(Calibrate());
        
        Magnitudes.Enqueue(0f);
        Magnitudes.Enqueue(0f);
        Magnitudes.Enqueue(0f);
        Magnitudes.Enqueue(0f);
        Magnitudes.Enqueue(0f);
        Magnitudes.Enqueue(0f);

    }
}