//Unity has its own syntax for shader assets, which is overall roughly like C# but it's a mix of different languages.
//It begins with the Shader keyword followed by a string defining a menu item for the shader.
//Strings are written inside double quotes. We'll use Graph/Point Surface. After that comes a code block for the shader's contents.
Shader "Graph/Point Surface" {
    //To make configutation options appear in the editor we have to add a Properties block at the top of the shader, above the sub-shader.
    //Write _Smoothness in here, followed by ("Smoothness", Range(0,1)) = 0.5.
    //This gives it the Smoothness label, exposes it as a slider with the 0–1 range, and sets its default to 0.5.
    Properties {
		_Smoothness ("Smoothness", Range(0,1)) = 0.5
	}

    //a Shader can contain multiple subShaders, we only need one
    subShader {
        //The sub-shader of a surface shader needs a code section written in a hybrid of CG and HLSL, two shader languages.
        //This code must be enclosed by the CGPROGRAM and ENDCG keywords.
        CGPROGRAM
        
        //The first needed statement is a compiler directive, known as a pragma. It's written as #pragma followed by a directive.
        //In this case we need #pragma surface ConfigureSurface Standard fullforwardshadows,
        //which instructs the shader compiler to generate a surface shader with standard lighting and full support for shadows.
        //ConfigureSurface refers to a method used to configure the shader, which we'll have to create.
        #pragma surface ConfigureSurface Standard fullforwardshadows
        //This code will NOT work in URP. It will turn pink.
        //BRP uses #pragma surface (CGPROGRAM).
        //URP uses #pragma vertex/fragment (HLSLPROGRAM).
        //If we want to make this shader work in URP, we need to create a separate shader. We could write one ourselves, but that's currently very hard and likely to break when upgrading to a newer URP version.
        //The best approach is to use Unity's shader graph package to visually design a shader. URP depends on this package so it was automatically installed along with the URP package.

        //We follow that with the #pragma target 3.0 directive, which sets a minimum for the shader's target level and quality.
        #pragma target 3.0

        //We're going to color our points based on their world position.
        //To make this work in a surface shader we have to define the input structure for our configuration function.
        //It has to be written as struct Input followed by a code block and then a semicolon. Inside the block we declare a single struct field,
        //specifically float3 worldPos. It will contain the world position of what gets rendered.
        //The float3 type is the shader equivalent of the Vector3 struct.
        struct Input {
            float3 worldPos;
        };

        //Below that we define our ConfigureSurface method, although in the case of shaders it's always referred to as a function, not as a method.
        //It is a void function with two parameters.
        //First is an input parameter that has the Input type that we just defined.
        //The second parameter is the surface configuration data, with the type SurfaceOutputStandard.
        //The second parameter must have the inout keyword written in front of its type,
        //which indicates that it's both passed to the function and used for the result of the function.
        float _Smoothness;
        void ConfigureSurface(Input input, inout SurfaceOutputStandard surface) {
            //Now that we have a functioning shader create a material for it, named Point Surface.
            //Set it to use our shader, by selecting Graph / Point Surface via the Shader dropdown list in the header of its inspector.

            //The material is currently solid matte black. We can make it look more like the default material by setting surface.Smoothness to 0.5 in our configuration function.
            // When writing shader code we do not have to add the f suffix to float values.
            //We can also make smoothness configurable, as if adding a field for it and using that in the function.
            //The default style is to prefix shader configuration options with an underscore and capitalize the next letter, so we'll use _Smoothness.
            //To make this configuration option appear in the editor we have to add a Properties block at the top of the shader, above the sub-shader.
            surface.Smoothness = _Smoothness;

            //To adjust the color of our points we have to modify surface.Albedo.
            //As both albedo and the world position have three components we can directly use the position for albedo.
            //Now the world X position controls the point's red color component, the Y position controls the green color component, and Z controls blue.
            //[X, Y, Z]
            //[R, G, B]
            //But our graph's X domain is −1–1, and negative color components make no sense. So we have to halve the position and then add ½ to make the colors fit the domain.
            //We can do this for all three dimension at once (by adding "* 0.5 + 0.5").
            //The result is bluish because all cube faces have Z coordinates close to zero, which sets their blue color component close to 0.5.
            //We can eliminate blue by only including the red and green channels when setting the albedo.
            //This can be done in shaders by only assigning to surface.Albedo.rg and only using input.worldPos.xy. That way the blue component stays zero.
            //As red plus green results in yellow this will make the points start near black at the bottom left,
            //turn green as Y initially increases quicker than X, turn yellow as X catches up, turn slightly orange as X increases faster, and finally end near bright yellow at the top right.
            //Saturate is a shader function that clamps values to the 0–1 range.

            //Adding a Third Dimension to Color:
            //Currently our shader only uses the X and Y world position to set the color, leaving the Z position unused.
            //With Z no longer constant, change our Point Surface shader so it also modifies the blue albedo component, by removing the .rg and .xy code from the assignment.
            surface.Albedo = saturate(input.worldPos * 0.5 + 0.5);
        }

        ENDCG
    }

    //Below the sub-shader we also want to add a fallback to the standard diffuse shader, by writing FallBack "Diffuse".
    //This means that if the GPU can't handle our shader, it will use the standard diffuse shader instead.
    FallBack "Diffuse"
}