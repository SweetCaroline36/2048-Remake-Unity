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

    private Dictionary<Vector2, Node> nodes;

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
        var rand = nodes.ElementAt(Random.Range(0, check.Count)).Key;
        return rand;
    }

    /*
    public List<Vector2> InvalidNodeVectors(Die activeDie)
    {
        var adjacentNodes = new List<Vector2>();
        //for every occupied node
        foreach (var node in nodes.Where(n => n.Value.OccupiedDie != null).ToDictionary(i => i.Key, i => i.Value))
        {
            //valid empty nodes adjacent to existing occupied nodes that don't match color and number 
            if (activeDie.getColor() != nodes[node.Key].OccupiedDie.getColor() && activeDie.getNumber() != nodes[node.Key].OccupiedDie.getNumber())
            {
                if (nodes.ContainsKey(node.Key + new Vector2(1, 0)) && nodes[node.Key + new Vector2(1, 0)].OccupiedDie == null)
                {
                    adjacentNodes.Add(node.Key + new Vector2(1, 0));
                }
                if (nodes.ContainsKey(node.Key + new Vector2(-1, 0)) && nodes[node.Key + new Vector2(-1, 0)].OccupiedDie == null)
                {
                    adjacentNodes.Add(node.Key + new Vector2(-1, 0));
                }
                if (nodes.ContainsKey(node.Key + new Vector2(0, 1)) && nodes[node.Key + new Vector2(0, 1)].OccupiedDie == null)
                {
                    adjacentNodes.Add(node.Key + new Vector2(0, 1));
                }
                if (nodes.ContainsKey(node.Key + new Vector2(0, -1)) && nodes[node.Key + new Vector2(0, -1)].OccupiedDie == null)
                {
                    adjacentNodes.Add(node.Key + new Vector2(0, -1));
                }
            }
            
        }
        return adjacentNodes;
    }

    public List<Node> InvalidNodes(List<Vector2> vectors)
    {
        var send = new List<Node>();
        foreach (Vector2 v in vectors) {
            send.Add(nodes[v]);
        }
        return send;
    }

    public void PlaceDieInGrid(Node node, Die die)
    {
        //die.OccupiedNode = node;
        node.SetDie(die);
        DiceManager.Instance.removeRackDie(die);
        DiceManager.Instance.setDiePos(die);
    }

    public void RemoveDiceFromGrid(Die die)
    {
        nodes[DiceManager.Instance.getDiePos(die)].RemoveDie(die);
    }

    public List<Die> CheckForStraightRow()
    {
        List<Die> rowCandidates = new List<Die>();
        List<Die> colCandidates = new List<Die>();
        List<Die> confirmedRow = new List<Die>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (nodes[new Vector2(x, y)].OccupiedDie != null)
                {
                    rowCandidates.Add(nodes[new Vector2(x, y)].OccupiedDie);
                }
                if (nodes[new Vector2(y, x)].OccupiedDie != null)
                {
                    colCandidates.Add(nodes[new Vector2(y, x)].OccupiedDie);
                }
            }

            if (rowCandidates.Count() != height) 
            {
                rowCandidates.Clear();
            } 
            else
            {
                foreach (Die candidates in rowCandidates)
                {
                    confirmedRow.Add(candidates);
                    //print(candidates.getNumber());
                }
                rowCandidates.Clear();
            }
            if (colCandidates.Count() != width)
            {
                colCandidates.Clear();
            }
            else
            {
                foreach (Die candidates in colCandidates)
                {
                    confirmedRow.Add(candidates);
                    //print(candidates.getNumber());
                }
                colCandidates.Clear();
            }
        }
        var test = confirmedRow.Distinct().ToList();
        return test;
    }
    */
}