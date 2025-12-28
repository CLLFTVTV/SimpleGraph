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

    [SerializeField]
	FunctionLibrary.FunctionName function;

    void Awake () {
		float step = 2f / resolution;
		var scale = Vector3.one * step;
		//var position = Vector3.zero;
		points = new Transform[resolution * resolution];
		for (int i = 0; i < points.Length; i++) {
			Transform point = points[i] = Instantiate(pointPrefab);
			point.localScale = scale;
			point.SetParent(transform, false);
		}
	}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update () {
		FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);
		float time = Time.time;
		float step = 2f / resolution;
		float v = 0.5f * step - 1f;
		for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) {
			if (x == resolution) {
				x = 0;
				z += 1;
				v = (z + 0.5f) * step - 1f;
			}
			float u = (x + 0.5f) * step - 1f;
			points[i].localPosition = f(u, v, time);
		}
	}
}