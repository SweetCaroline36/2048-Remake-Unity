using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Box box;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = box.GetNumber().ToString();
    }
}
