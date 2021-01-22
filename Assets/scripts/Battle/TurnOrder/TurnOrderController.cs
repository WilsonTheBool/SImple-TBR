using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TurnOrderController : MonoBehaviour
{

    public BattleController BattleController { get; set; }

    public event Action TurnOrderUpdated;

    private void Start()
    {
        SetUp();
    }

    void SetUp()
    {
        BattleController = GetComponent<BattleController>();
        turnOrderCharacters = new List<Character>();

        BattleController.BattleObjectsChanged += UpdateTurnOrder;
        BattleController.SomeInitiativeChanged += UpdateTurnOrder;
        BattleController.SomeTurnEnded += UpdateTurnOrder;
        BattleController.SomeTurnEnded += BattleController_SomeTurnEnded;

        UpdateTurnOrder(BattleController, null);

        BattleController.SetActiveCharacter(turnOrderCharacters[0]);
    }

    private void BattleController_SomeTurnEnded(object sender, EventArgs e)
    {
        if(turnOrderCharacters.Count > 0)
        {
            BattleController.SetActiveCharacter(turnOrderCharacters[0]);
        }
       
    }

    public List<Character> turnOrderCharacters;

    public int EndTurnINdex;


    void UpdateTurnOrder(object battleController, EventArgs e)
    {
        turnOrderCharacters.Clear();

        SetNewTurnOrderCharacters();

        TurnOrderUpdated?.Invoke();

    }



    public int GetLastActiveCharacterIndex()
    {

        return EndTurnINdex;
    }

    Character GetNextActiveCharacter()
    {
        if (turnOrderCharacters.Count > 0)
            return turnOrderCharacters[0];
        else
            return null;
    }

    public int GetIndexOFCharacter(Character ch)
    {
        return turnOrderCharacters.IndexOf(ch) + 1;
    }

    void SetNewTurnOrderCharacters()
    {
        var comparer = new CharacterInitiativeComparer();

        List<Character> characters = BattleController.GetAllActiveCharacters();

        if(characters.Count > 0)
        {
            characters.Sort(comparer);

            turnOrderCharacters.AddRange(characters);

            EndTurnINdex = characters.Count - 1;

            characters.AddRange(BattleController.GetAllInactiveCharacters());

            characters.Sort(comparer);

            turnOrderCharacters.AddRange(characters);
        }
        else
        {
            GetNewTurnCycle();
            SetNewTurnOrderCharacters();
        }
        


    }

    public event EventHandler NewTurnCycle;
    private void GetNewTurnCycle()
    {
        NewTurnCycle?.Invoke(this, null);
    }

    private class CharacterInitiativeComparer : IComparer<Character>
    {
        public int Compare(Character x, Character y)
        {
            int c = -x.Initiative.CompareTo(y.Initiative);
            if(c == 0)
            {
                c = -x.speed.CompareTo(y.speed);

                if(c == 0)
                {
                    return x.name.CompareTo(y.name);
                }
                
            }
            return c;
        }
    }
}
