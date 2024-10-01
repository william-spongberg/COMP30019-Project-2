using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Require camera
[RequireComponent(typeof(Camera))]
public class ApplyShader : MonoBehaviour
{
    [SerializeField]
    private Shader customShader;
    private Material customMaterial;

    void Start()
    {
        customMaterial = new Material(customShader);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, customMaterial);
    }
}