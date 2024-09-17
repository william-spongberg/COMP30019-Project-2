using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// show in editor
[ExecuteInEditMode]
public class NewBehaviourScript : MonoBehaviour
{

    public Shader shader;

    // My Own Presets
    [Range(0, 18)]
    public int preset = 0; 
    private int lastPreset = 0;
    private float[,] presetSettings = {
        {0, 0, 0, 0, 0},
        {16, 16, 16, 0.0f, 0},
        {12, 12, 12, 0.0f, 0},
        {8, 8, 8, 0.0f, 0},
        {6, 6, 6, 0.0f, 0},
        {5, 5, 5, 0.0f, 0},
        {4, 4, 4, 0.0f, 0},
        {3, 3, 3, 0.0f, 0},
        {2, 2, 2, 0.0f, 0},
        {5, 5, 5, 0.11f, 0},
        {5, 5, 5, 0.11f, 1},
        {5, 5, 5, 0.11f, 2},
        {2, 2, 2, 0.95f, 0},
        {2, 2, 2, 1.0f, 0},
        {2, 2, 2, 1.0f, 1},
        {4, 4, 4, 0.22f, 0},
        {4, 4, 4, 0.22f, 1},
        {4, 4, 4, 0.22f, 2},
        {4, 4, 4, 0.094f, 1},
        };

    // Color Quantization Setting Variables
    [Range(2, 16)]
    public int redColorCount = 2;
    [Range(2, 16)]
    public int greenColorCount = 2;
    [Range(2, 16)]
    public int blueColorCount = 2;

    // Dithering Setting Variables
    [Range(0.0f, 1.0f)]
    public float spread = 0.5f;
    [Range(0, 2)]
    public int bayerLevel = 0;

    // Downsampling Setting Variables


    


    private Material shaderMat;

    private void OnEnable() {
        shaderMat = new Material(shader);
        shaderMat.hideFlags = HideFlags.HideAndDontSave;
    }

    private void OnDisable() {
        shaderMat = null;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest) {
        
        // If preset gets switched change the settings to that new preset
        if (preset != lastPreset && preset != 0) {
            redColorCount   = (int) presetSettings[preset,0];
            greenColorCount = (int) presetSettings[preset,1];
            blueColorCount  = (int) presetSettings[preset,2];
            spread          =       presetSettings[preset,3];
            bayerLevel      = (int) presetSettings[preset,4];
            lastPreset = preset;
        }

        // Set all shader variables from Inspector here! for example:
        // shaderMat.SetInt("_MySettingVariable", mySettingVariable)
        // Where _MySettingVariable is instantiated in the shader file.
        shaderMat.SetInt("_RedColorCount", redColorCount);
        shaderMat.SetInt("_GreenColorCount", greenColorCount);
        shaderMat.SetInt("_BlueColorCount", blueColorCount);
        shaderMat.SetFloat("_Spread", spread);
        shaderMat.SetInt("_BayerLevel", bayerLevel);

        int width = src.width;
        int height = src.height;

        // If you want get or transform a RenderTexture before inputing into
        // Graphics.Blit, RenderTexture.GetTemporary to create one. (eg for down sampling)

        // No downsamples just yet, but this is where the loop would be.
        RenderTexture currentSource = src;

        // Apply the dither
        RenderTexture dither = RenderTexture.GetTemporary(width, height, 0, src.format);
        Graphics.Blit(currentSource, dither, shaderMat, 0);


        // Finally apply the shader bitmap material to the current source and
        // output to the given destination parameter.
        Graphics.Blit(dither, dest, shaderMat, 1);

        // Finally if you used RenderTexture.GetTemporary, use ReleaseTemporary
        // on all those textures here.
        RenderTexture.ReleaseTemporary(dither);
    }
}
