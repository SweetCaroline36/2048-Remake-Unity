using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Box OccupiedBox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetBox(Box box)
    {
        if (box.OccupiedNode != null) box.OccupiedNode.OccupiedBox = null;
        //die.transform.position = transform.position;
        OccupiedBox = box;
        box.OccupiedNode = this;
        //DiceManager.Instance.removeRackDie(die);
    }

    public void RemoveBox(Box box)
    {
        if (box.OccupiedNode != null) box.OccupiedNode.OccupiedBox = null;
    }

    /*public void DisplayX()
    {
        xRenderer.enabled = true;
    }
    public void RemoveX()
    {
        xRenderer.enabled = false;
    }
    */
}
