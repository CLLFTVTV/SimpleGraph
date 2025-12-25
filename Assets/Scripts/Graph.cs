using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Graph : MonoBehaviour
{
    [SerializeField]
    Transform pointPrefab = default;

    [SerializeField, Range(10, 100)]
    int resolution = 10;

    void Awake()
    {

        float step = 2f / resolution;
		var position = Vector3.zero;
		var scale = Vector3.one * step;
		for (int i = 0; i < resolution; i++) {
			Transform point = Instantiate(pointPrefab);
			position.x = (i + 0.5f) * step - 1f;
			position.y = position.x * position.x * position.x;
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
        
    }
}
