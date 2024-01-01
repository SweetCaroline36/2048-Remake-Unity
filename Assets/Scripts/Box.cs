
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Node OccupiedNode;

    [SerializeField] private SpriteRenderer boxRenderer;
    [SerializeField] private Sprite one, two, three, four, five, six;

    [SerializeField] private int number;

    public Vector2 startingPos;

    //private Animator animator;

    void Start()
    {
        //animator = GetComponent<Animator>();
    }

    public void Init()
    {
        //if (GameManager.Instance.getGameOver() != true) canMove = true;
        number = Random.Range(1, 3)*2;
        //color = randomColor(LevelSelector.diff);
        //diceRenderer.color = color;
        //diceRenderer.sprite = randomSprite(number);
    }
    void Update()
    {
        
    }

    public int getNumber() { return number; }
    //public Color getColor() { return color; }

    public Vector3 getPos()
    {
        return new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10);
    }

    public void setSortingLayer(string layer)
    {
        boxRenderer.sortingLayerName = layer;
    }
}



