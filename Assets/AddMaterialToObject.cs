using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AddMaterialToObject : MonoBehaviour
{
    public Material materialToAdd;
    public SpriteRenderer SpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {

        print(SpriteRenderer.materials.Length);

        var materials = SpriteRenderer.materials;

        Material[] newMaterials = new Material[materials.Length + 1];

        Array.Copy(materials, 0, newMaterials, 0, materials.Length);

        newMaterials[newMaterials.Length - 1] = materialToAdd;

        SpriteRenderer.materials = newMaterials;

        print(SpriteRenderer.materials.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
