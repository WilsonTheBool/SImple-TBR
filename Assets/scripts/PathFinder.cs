using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PathFinder : MonoBehaviour
{
    BattleController BattleController;

    private void Start()
    {
        BattleController = FindObjectOfType<BattleController>();
        SavedNodes = new List<PathNode>(); 
    }

    private class PathNode
    {
        public PathNode parent;

        public Vector3Int position;

        public float cost;

        public int range;

        public float pCost;

        public int pathRnage;

        public float FCost
        {
            get
            {
                return pCost + cost + pathRnage;
            }
        }

        public PathNode(PathNode parent, Vector3Int position, float cost, int range)
        {
            this.parent = parent;
            this.position = position;
            this.cost = cost;
            this.range = range;

            this.pCost = 0;
            this.pathRnage = 0;

        }
    }

    List<PathNode> SavedNodes;

    int CurentRange;

    Vector3Int curentPos;
    public void PrintALLNodesRange()
    {
        
        
    }
    public List<Vector3Int> GetSavedPositions()
    {
        List<Vector3Int> list = new List<Vector3Int>();

        foreach(PathNode node in SavedNodes)
        {
            list.Add(node.position);
        }

        return list;
    }
   
    public List<Vector3Int> GetPathGlobal(Vector3Int end)
    {
        Vector3Int start = curentPos;
        PathNode curentNode = null;
        if (NodeListHasPos(start, SavedNodes) && NodeListHasPos(end, SavedNodes))
        {
            List<PathNode> openList = new List<PathNode>();
            List<PathNode> closedList = new List<PathNode>();
            
            openList.Add(GetNodetWithPos(start, SavedNodes));
            
            while (openList.Count > 0)
            {
                
                curentNode = GetLowestFCost(ref openList);


                openList.Remove(curentNode);
                closedList.Add(curentNode);

                if (curentNode.position == end)
                {
                    
                    break;
                    
                }
                
                if (curentNode.pathRnage <= CurentRange)
                {
                    foreach (Vector3Int nPos in GetNeighboursPosInSaved(curentNode.position))
                    {
                        PathNode neighbour = GetNodetWithPos(nPos, SavedNodes);
                        if (!closedList.Contains(neighbour))
                        {
                            
                            if (!openList.Contains(neighbour))
                            {
                                
                                neighbour.pCost = curentNode.pCost + costPerTile;
                                neighbour.pathRnage = curentNode.pathRnage + 1;
                                neighbour.parent = curentNode;
                                openList.Add(neighbour);

                            }
                            else
                            {
                                if (neighbour.FCost > curentNode.FCost + costPerTile)
                                {
                                    neighbour.pCost = curentNode.pCost + costPerTile;
                                    neighbour.pathRnage = curentNode.pathRnage + 1;
                                    neighbour.parent = curentNode;

                                }
                            }

                        }
                        else
                        {
                            if (neighbour.pathRnage > curentNode.pathRnage)
                            {
                                neighbour.pCost = curentNode.pCost + costPerTile;
                                neighbour.pathRnage = curentNode.pathRnage + 1;
                                neighbour.parent = curentNode;
                                openList.Add(neighbour);
                            }
                        }
                        
                    }
                }
                
            }
        }
        List<Vector3Int> path = new List<Vector3Int>();
        
        if(curentNode.position == end)
        {
            while(curentNode != null)
            {
                path.Add(curentNode.position);
                curentNode = curentNode.parent;
            }
        }

        path.Reverse();
        return path;

    }

    private PathNode GetLowestFCost(ref List<PathNode> list)
    {
        PathNode bestNode = null;
        float bestCost = float.MaxValue;

        foreach(PathNode node in list)
        {
            if(node.FCost < bestCost)
            {
                bestNode = node;
                bestCost = node.FCost;
            }
        }

        return bestNode;

    }

    public void UpdateSavedNodes(Vector3Int startPos, int range)
    {
        CurentRange = range;
        curentPos = startPos;
        SavedNodes = CreateNewPathPlane(startPos, range);

      
    }

    const float costPerTile = 1;

    private List<PathNode> CreateNewPathPlane(Vector3Int startPos, int range)
    {
        int curentRange = 0;
        List<PathNode> pathList = new List<PathNode>();
        List<PathNode> curentPathList = new List<PathNode>();

        curentPathList.Add(new PathNode(null, startPos, 0, curentRange));
        

        while (curentPathList.Count > 0)
        {
            List<PathNode> listToAdd = new List<PathNode>();

            foreach (PathNode node in curentPathList)
            {
                if(node.range + 1 <= range)
                {
                    foreach (Vector3Int neighbour in GetNeighboursPos(node.position))
                    {
                        PathNode neighbourNode = GetNodetWithPos(neighbour, pathList);
                        PathNode neighbourNode2 = GetNodetWithPos(neighbour, curentPathList);
                        PathNode neighbourNode3 = GetNodetWithPos(neighbour, listToAdd);

                        //если такого нода нет
                        if (neighbourNode == null && neighbourNode2 == null && neighbourNode3 == null)
                        {
                            listToAdd.Add(new PathNode(null, neighbour, range * costPerTile, node.range + 1));
                        }
                        
                    }
                }
                
            }

            pathList.AddRange(curentPathList);
            curentPathList.Clear();
            curentPathList.AddRange(listToAdd);

           
        }

        return pathList;
        

    }

    public bool SavedNodeContainsPosition(Vector3Int position)
    {
        foreach (PathNode node in SavedNodes)
        {
            if (node.position == position)
            {
                return true;
            }
        }
        return false;
    }

    private bool NodeListHasPos(Vector3Int pos, List<PathNode> list)
    {
        foreach(PathNode node in list)
        {
            if(node.position == pos)
            {
                return true;
            }
        }
        return false;
    }

    private PathNode GetNodetWithPos(Vector3Int pos, List<PathNode> list)
    {
        foreach (PathNode node in list)
        {
            if (node.position == pos)
            {
                return node;
            }
        }
        return null;
    }

    private List<Vector3Int> GetNeighboursPosInSaved(Vector3Int position)
    {
        List<Vector3Int> neighbourList = new List<Vector3Int>();
        if (NodeListHasPos(position + new Vector3Int(1, 0, 0),SavedNodes))
        {
            neighbourList.Add(position + new Vector3Int(1, 0, 0));
        }
        if (NodeListHasPos(position + new Vector3Int(0, 1, 0), SavedNodes))
        {
            neighbourList.Add(position + new Vector3Int(0, 1, 0));
        }
        if (NodeListHasPos(position + new Vector3Int(-1, 0, 0), SavedNodes))
        {
            neighbourList.Add(position + new Vector3Int(-1, 0, 0));
        }
        if (NodeListHasPos(position + new Vector3Int(0, -1, 0), SavedNodes))
        {
            neighbourList.Add(position + new Vector3Int(0, -1, 0));
        }

        return neighbourList;
    }
    private List<Vector3Int> GetNeighboursPos(Vector3Int position)
    {
        List<Vector3Int> neighbourList = new List<Vector3Int>();
        if(CanWalkOnTile(position + new Vector3Int(1, 0, 0)))
        {
            neighbourList.Add(position + new Vector3Int(1, 0, 0));
        }
        if (CanWalkOnTile(position + new Vector3Int(0, 1, 0)))
        {
            neighbourList.Add(position + new Vector3Int(0, 1, 0));
        }
        if (CanWalkOnTile(position + new Vector3Int(-1, 0, 0)))
        {
            neighbourList.Add(position + new Vector3Int(-1, 0, 0));
        }
        if (CanWalkOnTile(position + new Vector3Int(0, -1, 0)))
        {
            neighbourList.Add(position + new Vector3Int(0, -1, 0));
        }

        return neighbourList;
    }

    private bool CanWalkOnTile(Vector3Int position)
    {
        return BattleController.IsTileEmpty(position);
    }
}