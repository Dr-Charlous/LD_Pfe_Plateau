using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class PositionsCube : MonoBehaviour
{
    public Vector3 Position;
    public Vector3 Offset;
    public PositionsCube[] Neighbors;
    public List<PositionsCube> Done;
    public PositionsCube Previous;
    public GameObject Player;

    private void OnMouseDown()
    {

        Done.Clear();

        if (FindPath(Player.GetComponent<Player>().positionsCube, this))
        {
            List<PositionsCube> path = GetPath(this);

            Vector3 from = Position + Offset;

            for (int i = 0; i < path.Count; i++)
            {
                PositionsCube tile = path[i];

                Vector3 destination = tile.transform.position + tile.Offset;
                Player.transform.position = destination;
            }

            MoveEnd();

            foreach (var tile in path)
            {
                tile.Previous = null;
            }
        }

        Player.transform.position = transform.position + Offset;
    }

    void MoveEnd()
    {
        Player.GetComponent<Player>().positionsCube = this;
    }

    bool FindPath(PositionsCube start, PositionsCube end)
    {
        if (start == end)
            return true;

        Done.Add(start);

        foreach (var item in start.Neighbors)
        {
            if (Done.Contains(item) == false && FindPath(item, end))
            {
                item.Previous = start;

                return true;
            }
        }

        return false;
    }

    List<PositionsCube> GetPath(PositionsCube lastTile)
    {
        List<PositionsCube> path = new List<PositionsCube>();

        PositionsCube current = lastTile;
        while (current != null)
        {
            path.Insert(0, current);
            current = current.Previous;
        }

        path.RemoveAt(0);

        return path;
    }
}
