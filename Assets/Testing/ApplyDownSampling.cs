using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyDownSampling : MonoBehaviour
{

    // Point Clamp Sampling Shader
    public Shader shader;

    // Downsampling Setting Variables
    [Range(0,8)]
    public int downSamples;

    // Not really sure what this is
    public bool pointFilterDown;

    


    private Material shaderMat;

    private void OnEnable() {
        shaderMat = new Material(shader);
        shaderMat.hideFlags = HideFlags.HideAndDontSave;
    }

    private void OnDisable() {
        shaderMat = null;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest) {
        
        int width = src.width;
        int height = src.height;

        // If you want get or transform a RenderTexture before inputing into
        // Graphics.Blit, RenderTexture.GetTemporary to create one. (eg for down sampling)
        

        // Repeatedly downsample here.
        RenderTexture currentSource = src;
        RenderTexture[] textures = new RenderTexture[8];

        for (int i = 0; i < downSamples; i++) {
            // Half the width and height
            width /= 2;
            height /= 2;

            // Break if we can't reduce height more.
            if (height < 2) {
                break;
            }

            // We set the current destination to the next reduced resolution.
            RenderTexture currentDestination = textures[i] = RenderTexture.GetTemporary(width, height, 0, src.format);

            // Blit (with or without point filtering) the current source RenderTexture to the smaller current destination RenderTexture
            if (pointFilterDown)
                Graphics.Blit(currentSource, currentDestination, shaderMat);
            else
                Graphics.Blit(currentSource, currentDestination);

            // Set the new current source to the current destination for the next loop.
            currentSource = currentDestination;
        }


        // Finally apply the shader bitmap material to the current source and
        // output to the given destination parameter.
        Graphics.Blit(currentSource, dest, shaderMat);

        // Finally if you used RenderTexture.GetTemporary, use ReleaseTemporary
        // on all those textures here.
        for (int i = 0; i < downSamples; ++i) {
            RenderTexture.ReleaseTemporary(textures[i]);
        }
    }
}