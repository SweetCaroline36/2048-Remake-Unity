using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BoxManager : MonoBehaviour
{
    public static BoxManager Instance;

    [SerializeField] private Box boxPrefab;

    //private Dictionary<Vector2, Box> boxes;
    private Dictionary<Vector2, Box> boxes = new Dictionary<Vector2, Box>();
    private bool anythingMoved = false;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    void Update() {
        if(!GameManager.Instance.getGameOver()) {
            if(Input.GetKeyDown("d") || Input.GetKeyDown("right")){
                MoveBoxes(Direction.Right);
            }
            if(Input.GetKeyDown("a") || Input.GetKeyDown("left")){
                MoveBoxes(Direction.Left);
            }
            if(Input.GetKeyDown("s") || Input.GetKeyDown("down")){
                MoveBoxes(Direction.Down);
            }
            if(Input.GetKeyDown("w") || Input.GetKeyDown("up")){
                MoveBoxes(Direction.Up);
            }
        }
    }

    public void SpawnBox() {
        Vector2 randNode = GridManager.Instance.GetRandomOpenNodeVector();
        var box = Instantiate(boxPrefab, randNode, Quaternion.identity);
        box.PlayAnimation("Spawn");
        boxes[randNode] = box;
        Node node = GridManager.Instance.ReturnNodeOfVector(randNode);
        box.Init();
        node.SetBox(box);
    }

    private IEnumerator WaitAndSpawn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SpawnBox();
        if(boxes.Count == 16 && noMoreMoves()) {
            GameManager.Instance.GameOver();
        }
    }
    private IEnumerator WaitAndDestroy(float waitTime, Box box)
    {
        yield return new WaitForSeconds(waitTime);
        RemoveBox(box);
    }

    public void CreateTwoBoxes() {
        SpawnBox();
        SpawnBox();
    }

    private bool noMoreMoves() {
        bool canMove = false;
        for (int i = 0; i<4; i++) {
            for (int j = 0; j<3; j++) {
                if(boxes.ContainsKey(new Vector2(i+1,j)) && boxes[new Vector2(i,j)].GetNumber() == boxes[new Vector2(i+1,j)].GetNumber()) {
                    canMove = true;
                }
                if(boxes.ContainsKey(new Vector2(i,j+1)) && boxes[new Vector2(i,j)].GetNumber() == boxes[new Vector2(i,j+1)].GetNumber()) {
                    canMove = true;
                }
            }
        }
        return !canMove;
    }

    public void MergeBoxes(Box movingBox, Box destBox) {
        destBox.SetNumber(destBox.GetNumber() * 2);
        destBox.HasMerged = true;
        movingBox.OccupiedNode.RemoveBox(movingBox);
        boxes.Remove(new Vector2(movingBox.transform.position.x, movingBox.transform.position.y));
        movingBox.SlideToPosition(new Vector2(destBox.transform.position.x, destBox.transform.position.y), true);
        destBox.PlayAnimation("Merge");
    }

    public void RemoveBox(Box box) {
        box.OccupiedNode.RemoveBox(box);
        boxes.Remove(new Vector2(box.transform.position.x, box.transform.position.y));
        Destroy(box.gameObject);
    }

    public void MoveBox(Vector2 oldCoords, Vector2 newCoords) {
        if(oldCoords != newCoords) {
            Box oldBox = boxes[oldCoords];      //set box variable
            oldBox.SlideToPosition(newCoords, false);
            //oldBox.transform.position = newCoords;  //move box to new square
            oldBox.OccupiedNode.RemoveBox(oldBox);  //remove box reference from previous node
            GridManager.Instance.ReturnNodeOfVector(newCoords).SetBox(oldBox);  //add box reference to new node
            boxes[newCoords] = oldBox;  //update box dictionary
            boxes.Remove(oldCoords); 
        }   //remove old dictionary reference
    }

    public void MoveBoxes(Direction direction) {
        switch(direction) {
            case Direction.Up:
                //for each of 12 places below the top row
                for(int i = 0; i<4; i++) {
                    for(int j = 2; j>=0; j--) {
                        //if theres a box
                        if(boxes.ContainsKey(new Vector2(i, j))) {
                            var oldCoords = new Vector2(i, j);
                            bool moved = false;
                            for(int k = 1; k<=3-j; k++) {
                                //Debug.Log("checking if " + i + ", " + (j+k) + " is empty");
                                if(!GridManager.Instance.IsNodeEmpty(new Vector2(i,j+k))) {
                                    //Debug.Log("found box at " + i + ", " + (j+k));
                                    var targetBox = boxes[new Vector2(i, j+k)];
                                    var movingBox = boxes[oldCoords];
                                    if(targetBox.GetNumber() == movingBox.GetNumber() && !targetBox.HasMerged) {
                                        //Debug.Log("merging");
                                        MergeBoxes(movingBox, targetBox);
                                        anythingMoved = true;
                                    } else if (oldCoords.y != j+k-1) {
                                        MoveBox(oldCoords, new Vector2(i, j+k-1));
                                        anythingMoved = true;
                                    }
                                    moved = true;
                                    break;
                                }
                            }
                            if(!moved && oldCoords.y != 3) {
                                MoveBox(oldCoords, new Vector2(i, 3));
                                moved = true;
                                anythingMoved = true;
                            }
                        }
                    }
                }
                foreach (var box in boxes.Values) {
                    box.HasMerged = false;
                }
                break;
            case Direction.Down:
                for(int i = 0; i<4; i++) {
                        for(int j = 1; j<=3; j++) {
                            //if theres a box
                            if(boxes.ContainsKey(new Vector2(i, j))) {
                                var oldCoords = new Vector2(i, j);
                                bool moved = false;
                                for(int k = 1; k<=j; k++) {
                                    //Debug.Log("checking if " + i + ", " + (j+k) + " is empty");
                                    if(!GridManager.Instance.IsNodeEmpty(new Vector2(i,j-k))) {
                                        //Debug.Log("found box at " + i + ", " + (j+k));
                                        var targetBox = boxes[new Vector2(i, j-k)];
                                        var movingBox = boxes[oldCoords];
                                        if(targetBox.GetNumber() == movingBox.GetNumber() && !targetBox.HasMerged) {
                                            MergeBoxes(movingBox, targetBox);
                                            anythingMoved = true;
                                        } else if (oldCoords.y != j-k+1) {
                                            MoveBox(oldCoords, new Vector2(i, j-k+1));
                                            anythingMoved = true;
                                        }
                                        moved = true;
                                        break;
                                    }
                                }
                                if(!moved && oldCoords.y != 0) {
                                    MoveBox(oldCoords, new Vector2(i, 0));
                                    moved = true;
                                    anythingMoved = true;
                                }
                            }
                        }
                    }
                    foreach (var box in boxes.Values) {
                    box.HasMerged = false;
                    }
                break;
            case Direction.Left:
                for(int j = 0; j<4; j++) {
                        for(int i = 1; i<=3; i++) {
                            //if theres a box
                            if(boxes.ContainsKey(new Vector2(i, j))) {
                                var oldCoords = new Vector2(i, j);
                                bool moved = false;
                                for(int k = 1; k<=i; k++) {
                                    //Debug.Log("checking if " + i + ", " + (j+k) + " is empty");
                                    if(!GridManager.Instance.IsNodeEmpty(new Vector2(i-k,j))) {
                                        //Debug.Log("found box at " + i + ", " + (j+k));
                                        var targetBox = boxes[new Vector2(i-k, j)];
                                        var movingBox = boxes[oldCoords];
                                        if(targetBox.GetNumber() == movingBox.GetNumber() && !targetBox.HasMerged) {
                                            MergeBoxes(movingBox, targetBox);
                                            anythingMoved = true;
                                        } else if (oldCoords.x != i-k+1) {
                                            MoveBox(oldCoords, new Vector2(i-k+1, j));
                                            anythingMoved = true;
                                        }
                                        moved = true;
                                        break;
                                    }
                                }
                                if(!moved && oldCoords.x != 0) {
                                    MoveBox(oldCoords, new Vector2(0, j));
                                    moved = true;
                                    anythingMoved = true;
                                }
                            }
                        }
                    }
                    foreach (var box in boxes.Values) {
                    box.HasMerged = false;
                    }
                break;
            case Direction.Right:
                for(int j = 0; j<4; j++) {
                        for(int i = 2; i>=0; i--) {
                            //if theres a box
                            if(boxes.ContainsKey(new Vector2(i, j))) {
                                var oldCoords = new Vector2(i, j);
                                bool moved = false;
                                for(int k = 1; k<=3-i; k++) {
                                    //Debug.Log("checking if " + i + ", " + (j+k) + " is empty");
                                    if(!GridManager.Instance.IsNodeEmpty(new Vector2(i+k,j))) {
                                        //Debug.Log("found box at " + i + ", " + (j+k));
                                        var targetBox = boxes[new Vector2(i+k, j)];
                                        var movingBox = boxes[oldCoords];
                                        if(targetBox.GetNumber() == movingBox.GetNumber() && !targetBox.HasMerged) {
                                            MergeBoxes(movingBox, targetBox);
                                            anythingMoved = true;
                                        } else if (oldCoords.x != i+k-1) {
                                            MoveBox(oldCoords, new Vector2(i+k-1, j));
                                            anythingMoved = true;
                                        }
                                        moved = true;
                                        break;
                                    }
                                }
                                if(!moved && oldCoords.x != 3) {
                                    MoveBox(oldCoords, new Vector2(3, j));
                                    moved = true;
                                    anythingMoved = true;
                                }
                            }
                        }
                    }
                    foreach (var box in boxes.Values) {
                        box.HasMerged = false;
                    }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
        if(anythingMoved) {
            IEnumerator spawnNew = WaitAndSpawn(0.1f);
            StartCoroutine(spawnNew);
            anythingMoved = false;
        }
    }
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}