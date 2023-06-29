using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour {
    public float waterLevel;
    public float waveStrength;
    public float waveSpeed;
    
    // Waves are in the X-direction
    public float offset = 0;

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    void Awake() {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        meshRenderer.material.SetInt("_UseScriptOffset", 1);
        meshRenderer.material.SetFloat("_WaveStrength", waveStrength);
        meshRenderer.material.SetFloat("_WaveSpeed", waveSpeed);
        meshRenderer.material.SetFloat("_Offset", offset);
    }

    void Update() {
        offset += waveSpeed * Time.deltaTime;
        meshRenderer.material.SetFloat("_Offset", offset);
    }

    public float GetWaveHeight(Vector3 pos) {
        return waterLevel + waveStrength * (Mathf.Cos(pos.y) + Mathf.Cos(pos.x + offset));
    }
}
