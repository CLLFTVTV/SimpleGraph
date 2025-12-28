using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Graph : MonoBehaviour
{
    //We need a prefab to represent each point in the graph. We'll use a simple Unity cube for this.
    [SerializeField]
    Transform pointPrefab = default;

    //The resolution determines how many points we will use to represent the graph.
    [SerializeField, Range(10, 200)]
    int resolution = 10;

    //To animate the graph we'll have to adjust its points as time progresses. We could do this by deleting all points and creating new ones each update,
    //but that's an inefficient way to do this. It's much better to keep using the same points, adjusting their positions each update.
    // To make this possible we're going to use an array to keep a reference to our points.
    Transform[] points;

    [SerializeField, Range(0, 2)]
    int function;

    void Awake()
    {
        
        //The step variable determines the distance between each point in the graph.
        float step = 2f / resolution;

		var position = Vector3.zero;
		var scale = Vector3.one * step;

        //Initialize the points array and create the points.
        points = new Transform[resolution];

		for (int i = 0; i < points.Length; i++) {
			Transform point = points[i] = Instantiate(pointPrefab);
			
            //Since points only move up and down, we keep the define the x position in Awake, and y position in Update.
			position.x = (i + 0.5f) * step - 1f;
            //position.y = position.x * position.x * position.x;
			
            point.localPosition = position;
			point.localScale = scale;
            point.SetParent(transform, false);
		}
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);

        float time = Time.time;
        for (int i = 0; i < points.Length; i++) {
            Transform point = points[i];
            Vector3 position = point.localPosition;

            //We can add time to our base graph but adding increasing time will cause the graph to go up and out of view.
            //We need a function that oscillates over time, in a fixed range. The sine function is perfect for this.
            //To animate this function, add the current game time to X before calculating the sine function. It's found via Time.time.
            //If we scale the time by π as well the function will repeat every two seconds. So use f(x,t)=sin(π(x+t)), where t is the elapsed game time.
            // This will advance the sine wave as time progresses, shifting it in the negative X direction.
            position.y = f(position.x, time);
			point.localPosition = position;
        }
    }
}