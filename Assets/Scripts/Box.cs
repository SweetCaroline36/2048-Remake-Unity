
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Box : MonoBehaviour
{
    public Node OccupiedNode;

    [SerializeField] private SpriteRenderer boxRenderer;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Animator animator;
    [SerializeField] private int number;
    private Color color;
    private Color textColor;
    public bool HasMerged = false;
    private bool merging = false;
    private float moveDuration = 0.09f;
    private float elapsedTime;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private bool lerping = false;

    //private Animator animator;

    void Start()
    {
        //animator = GetComponent<Animator>();
        //boxRenderer.color = new Color(240f/255f,231f/255f,219f/255f);
        //text.color = new Color(94f/255f,90f/255f,83f/255f);
        //animator = GetComponent<Animator>();
    }

    public void Init()
    {
        //if (GameManager.Instance.getGameOver() != true) canMove = true;
        number = Random.Range(0, 10) == 0? 4: 2;
        boxRenderer.color = boxColorGetter(number);
        text.color = textColorGetter(number);
        startPosition = transform.position;
    }
    void Update()
    {
        if(lerping) {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / moveDuration;

            transform.position = Vector2.Lerp(startPosition, endPosition, percentageComplete);
            if(new Vector2(transform.position.x, transform.position.y) == endPosition) {
                lerping = false;
                if (merging) {
                    Destroy(this.gameObject);
                }
            }
        }
    }

    public int GetNumber() { return number; }
    public void SetNumber(int newNumber) { 
        number = newNumber; 
        color = boxColorGetter(number);
        textColor = textColorGetter(number);
        boxRenderer.color = color;
        text.color = textColor;
    }

    public void SlideToPosition(Vector2 destination, bool merging) {
        startPosition = transform.position;
        endPosition = destination;
        elapsedTime = 0;
        this.merging = merging;
        lerping = true;
    }

    public void PlayAnimation(string anim) {
        animator.Play(anim, 0, 0);
    }
    //public Color getColor() { return color; }

   /*public Vector3 getPos()
    {
        return new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10);
    }

    public void setSortingLayer(string layer)
    {
        boxRenderer.sortingLayerName = layer;
    }
    */

    private Color boxColorGetter(int number) {
        if(number == 2) {
            return new Color(237f/255f,228f/255f,219f/255f);
        } else if (number == 4) {
            return new Color(243f/255f,222f/255f,194f/255f);
        } else if (number == 8) {
            return new Color(251f/255f,210f/255f,156f/255f);
        } else if (number == 16) {
            return new Color(250f/255f,178f/255f,115f/255f);
        } else if (number == 32) {
            return new Color(251f/255f,115f/255f,75f/255f);
        } else if (number == 64) {
            return new Color(251f/255f,73f/255f,20f/255f);
        } else if (number == 128) {
            return new Color(251f/255f,217f/255f,137f/255f);
        } else if (number == 256) {
            return new Color(251f/255f,207f/255f,107f/255f);
        } else if (number == 512) {
            return new Color(251f/255f,199f/255f,79f/255f);
        } else if (number == 1024) {
            return new Color(251f/255f,199f/255f,79f/255f);
        } else if (number == 2048) {
            return new Color(251f/255f,181f/255f,20f/255f);
        } else {
            return Color.black;
        }
    }
    private Color textColorGetter(int number) {
        if (number == 2) {
            return new Color(121f/255f,112f/255f,102f/255f);
        } else if (number == 4) {
            return new Color(122f/255f,110f/255f,106f/255f);
        } else if (number == 8) {
            return new Color(255f/255f,244f/255f,218f/255f);
        } else if (number == 16) {
            return new Color(255f/255f,244f/255f,218f/255f);
        } else if (number >= 32) {
            return new Color(242f/255f,243f/255f,255f/255f);
        } else {
            return Color.black;
        }
    }
}



