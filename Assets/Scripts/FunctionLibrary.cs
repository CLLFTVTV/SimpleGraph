using UnityEngine;
using static UnityEngine.Mathf; //Import the static members of the Mathf class so we can use them without the Mathf. prefix.

//This class isn't going to be a component type. We're also not going to create an object instance of it.
//Instead we'll use it to provide a collection of publicly-accessible methods that represent math functions, similar to Unity's Mathf.
//To signify that this class is not to be used as an object template, we mark it as static, by writing the static keyword before class.
public static class FunctionLibrary
{
    static Function[] functions = { Wave, MultiWave, Ripple };
    public delegate float Function(float x, float t);
    public static Function GetFunction (int index) {
		return functions[index];
    }

    //By default methods are instance methods, which means that they have to be invoked on an object instance.
    //To make them work directly at the class level we have to mark it as static, just like FunctionLibrary itself.
    public static float Wave(float x, float t)
    {
        return Sin(PI * (x + t));
    }

    //Another, more complex function: a multi-wave.
    public static float MultiWave(float x, float t)
    {
        //We want to keep the original sine wave, and then add other functions on top of it.
        //To make that easy, we first assign in to a variable, then return that variable.
        float y = Sin(PI * (x + 0.5f * t));
        y += 0.5f * Sin(2f * PI * (x + t));
		return y * (2f / 3f);
    }

    //Ripple function
    public static float Ripple (float x, float t) {
		float d = Abs(x);
        float y = Sin(PI * (4f * d - t));
		return y / (1f + 10f * d);
	}

    
}
