using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoxManager : MonoBehaviour
{
    public static BoxManager Instance;

    [SerializeField] private Box boxPrefab;

    private Dictionary<Box, Vector2> boxes;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        boxes = new Dictionary<Box, Vector2>();
    }

    void Update() {
        if(Input.GetKeyDown("d")){
            Debug.Log("d");
        }
    }

    public void SpawnBox() {
        var box = Instantiate(boxPrefab, GridManager.Instance.GetRandomOpenNodeVector(), Quaternion.identity);
    }

    public void CreateTwoBoxes() {
        SpawnBox();
        SpawnBox();
    }

    /*    public void CreateNewDiceRack()
    {
        int diceCount = difficulty + 3;
        diceLeft = diceCount;

        for (int i = 0; i < diceCount; i++)
        {
            Vector3 spawnPos = new Vector3(i * 1.2f + 0.3f + (-.1f * difficulty), -1.7f, -0.5f);
            var die = Instantiate(dicePrefab, spawnPos, Quaternion.identity);
            die.Init();
            die.startingPos = spawnPos;
            dice[die] = spawnPos;
            addRackDie(die);
        }
        snapController.findDice();
        GameManager.Instance.ChangeState(GameState.PlayDice);
    }

    public Vector2 getDiePos(Die die)
    {
        return dice[die];
    }
    public void setDiePos(Die die)
    {
        if(die.OccupiedNode != null)
        {
            dice[die] = new Vector2(die.OccupiedNode.transform.position.x, die.OccupiedNode.transform.position.y);
            snapController.removeDraggable(die);
        }
    }

    public void addRackDie(Die dice)
    {
        rackDice.Add(dice);
    }
    public void removeRackDie(Die die)
    {
        if(rackDice.Contains(die))
        {
            rackDice.Remove(die);
        }
    }
    public List<Die> getRackDice()
    {
        return rackDice;
    }
    
    public int getDiceLeft()
    {
        return diceLeft;
    }
    public void setDiceLeft(int newAmount)
    {
        diceLeft = newAmount;
    }

    public void stopDragger()
    {
        foreach (Die die in dice.Keys.ToList())
        {
            die.canMove = false;
        }
    }
    */
}
