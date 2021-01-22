using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOrderShowerMain : MonoBehaviour
{

    TurnOrderController TurnOrder;

    BattleController BattleController;

    public Sprite allyNodeImage;

    public Sprite enemyNodeImage;


    public Sprite EnemyOpenedNode;
    public Sprite AllyOpenNode;


    public TurnOrderNode[] allNodes;

    public GameObject endTurnButton;
    RectTransform endTurnTransform;

    

    public int GetCharacterFirstPosition(Character character)
    {
        return TurnOrder.turnOrderCharacters.IndexOf(character);
    }

    private void Awake()
    {
        TurnOrder = FindObjectOfType<TurnOrderController>();

        BattleController = FindObjectOfType<BattleController>();

        TurnOrder.TurnOrderUpdated += TurnOrder_TurnOrderUpdated;

        endTurnTransform = endTurnButton.GetComponent<RectTransform>();
    }

    

    

    private void TurnOrder_TurnOrderUpdated()
    {
        UpdateTurnOrder();
    }

    public float nextTurnButtonOffset;

    void UpdateTurnOrder()
    {
        for(int index = 0; index < allNodes.Length; index++)
        {
            TurnOrderNode node = allNodes[index];

            List<Character> characters = TurnOrder.turnOrderCharacters;

            if(characters.Count > index)
            {
                node.SetUp(characters[index], index, this);
                float value = 1;

                if(index > TurnOrder.GetLastActiveCharacterIndex())
                {
                    value = 0.6f;
                }

                node.SetColor(new Color(value, value, value));
            }
            else
            {
                node.SetAlpha(0);
            }
        }

        int endIndex = TurnOrder.GetLastActiveCharacterIndex();

        if(endIndex >= 0 && endIndex < allNodes.Length)
        {
            endTurnButton.SetActive(true);
            endTurnTransform.position = new Vector3(endTurnTransform.position.x, allNodes[endIndex].transform.position.y + nextTurnButtonOffset, endTurnTransform.position.z);
        }
        else
        {
            endTurnButton.SetActive(false);
        }

        foreach(TurnOrderNode node in allNodes)
        {
            if(node.index > endIndex && endIndex >= 0)
            {
                node.PushDown(pushAmmount);
            }
            else
            {
                node.ReturnOnNormalPosition();
            }
        }
    }

    public float pushAmmount;

   

    

    TurnOrderNode GetNodeFromCharacter(Character ch)
    {
        foreach (TurnOrderNode node in allNodes)
        {
            if (node.character = ch)
            {
                return node;
            }
        }

        return null;
    }

   

    Vector3 GetEndTurnButtonPosition()
    {
        return new Vector3();
    }


}

