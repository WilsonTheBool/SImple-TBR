using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckCharacterIsActiveText : MonoBehaviour
{
    Text text;
    public Character owner;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        text.text = "Actors count: " + owner.battleController.ActorsCount.ToString();
    }
}
