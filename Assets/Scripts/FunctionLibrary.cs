using UnityEngine;
using static UnityEngine.Mathf; //Import the static members of the Mathf class so we can use them without the Mathf. prefix.

//This class isn't going to be a component type. We're also not going to create an object instance of it.
//Instead we'll use it to provide a collection of publicly-accessible methods that represent math functions, similar to Unity's Mathf.
//To signify that this class is not to be used as an object template, we mark it as static, by writing the static keyword before class.
public static class FunctionLibrary
{
    public enum FunctionName { Wave, MultiWave, Ripple }

    static Function[] functions = { Wave, MultiWave, Ripple };
    public delegate Vector3 Function (float u, float v, float t);
    public static Function GetFunction (FunctionName name) {
		return functions[(int)name];
	}

    //By default methods are instance methods, which means that they have to be invoked on an object instance.
    //To make them work directly at the class level we have to mark it as static, just like FunctionLibrary itself.
    public static Vector3 Wave (float u, float v, float t) {
		Vector3 p;
		p.x = u;
		p.y = Sin(PI * (u + v + t));
		p.z = v;
		return p;
	}

    //Another, more complex function: a multi-wave.
    public static Vector3 MultiWave (float u, float v, float t) {
		Vector3 p;
		p.x = u;
		p.y = Sin(PI * (u + 0.5f * t));
		p.y += 0.5f * Sin(2f * PI * (v + t));
		p.y += Sin(PI * (u + v + 0.25f * t));
		p.y *= 1f / 2.5f;
		p.z = v;
		return p;
	}

    //Ripple function
   public static Vector3 Ripple (float u, float v, float t) {
		float d = Sqrt(u * u + v * v);
		Vector3 p;
		p.x = u;
		p.y = Sin(PI * (4f * d - t));
		p.y /= 1f + 10f * d;
		p.z = v;
		return p;
	}
}
