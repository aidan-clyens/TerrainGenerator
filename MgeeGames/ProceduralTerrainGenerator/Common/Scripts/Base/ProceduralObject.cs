using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ProceduralObjectDataBase : ScriptableObject {
    [Range(0.0f, 1.0f)]
    public float density;
}