using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random=UnityEngine.Random;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] private int width, height;
    private Vector2 center;
    [SerializeField] private Node nodePrefab;

    public Dictionary<Vector2, Node> nodes;

    void Awake()
    {
        Instance = this;
    }

    public void BoardInit()
    {
        width = 4;
        height = 4;
        center = new Vector2((float)width / 2 - 0.5f, (float)height / 2 - 0.5f);
        nodes = new Dictionary<Vector2, Node>();
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var node = Instantiate(nodePrefab, new Vector2(x, y), Quaternion.identity);
                node.name = $"Node {x} {y}";
                nodes[new Vector2(x, y)] = node;
            }
        }
        Camera.main.transform.position = new Vector3(center.x, center.y, -10);
        Camera.main.orthographicSize = 2.8f;

        GameManager.Instance.ChangeState(GameState.SpawnBoxes);
    }


    public Dictionary<Vector2, Node> FindValidSpaces()
    {
        //all empty nodes
        var freeNodes = nodes.Where(n => n.Value.OccupiedBox == null).ToDictionary(i => i.Key, i => i.Value);
        return freeNodes;
    }
    
    public Vector2 GetRandomOpenNodeVector() 
    {
        var check = FindValidSpaces();
        var rand = check.ElementAt(Random.Range(0, check.Count)).Key;
        return rand;
    }
    public Node ReturnNodeOfVector(Vector2 vector) {
        return nodes[vector];
    }

    public bool IsNodeEmpty(Vector2 coords) {
        return nodes[coords].OccupiedBox == null;
    }
}