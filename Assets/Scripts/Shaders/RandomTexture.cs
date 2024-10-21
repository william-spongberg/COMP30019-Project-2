using UnityEngine;

public class RandomTexture : MonoBehaviour
{
    [SerializeField] private Texture2D[] textures;
    [SerializeField] private Material targetMaterial;
    [SerializeField] private float changeTextureThreshold = 0.25f;

    void Start()
    {
        GetRandomTexture();
    }

    void Update()
    {
        // change texture randomly
        if (Random.Range(0, 100) < changeTextureThreshold * 100)
        {
            GetRandomTexture();
        }
    }

    private void GetRandomTexture()
    {
        // randomly grab texture from array and assign to material
        Texture2D randomTexture = textures[Random.Range(0, textures.Length)];
        targetMaterial.SetTexture("_Texture", randomTexture);
    }
}