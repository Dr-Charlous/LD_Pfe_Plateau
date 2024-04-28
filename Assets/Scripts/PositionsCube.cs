using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class PositionsCube : MonoBehaviour
{
    public GameManager GameManager;
    public Vector3 Position;
    public Vector3 Offset;
    public Transform[] Neighbors;
    public PositionsCube Previous;
    public GameObject Player;
    public bool IsInWay = false;

    private void OnMouseDown()
    {
        transform.DOLocalMoveY(transform.localPosition.y - 0.1f, 0.1f).SetLoops(2, LoopType.Yoyo);
        Player.GetComponent<Player>().Done.Clear();
        Player.GetComponent<Player>().Iterations = 0;
        IsInWay = false;

        if (FindCude(Player.GetComponent<Player>().CurrentCube, this) && Player.GetComponent<Player>().Iterations <= GameManager.Dice1.DiceValue + GameManager.Dice2.DiceValue)
        {
            IsInWay = true;

            List<PositionsCube> path = GetPath(this);

            DG.Tweening.Sequence sequence = DOTween.Sequence();

            Vector3 from = Player.GetComponent<Player>().CurrentCube.transform.position + Player.GetComponent<Player>().CurrentCube.Offset;

            for (int i = 0; i < path.Count; i++)
            {
                PositionsCube cube = path[i];

                Vector3 destination = cube.transform.position + cube.Offset;
                sequence.Append(Player.transform.DOMove(destination, 1.0f).SetEase(Ease.Linear));

                if (i > 0)
                    from = path[i - 1].transform.position + path[i - 1].Offset;

                Vector3 direction = destination - from;
                direction.Normalize();

                sequence.Join(Player.transform.DORotateQuaternion(Quaternion.LookRotation(-direction, Vector3.up), 0.3f));
            }

            sequence.AppendCallback(MoveEnd);

            foreach (var cube in path)
            {
                cube.Previous = null;
            }
        }
    }

    bool FindCude(PositionsCube startCube, PositionsCube endCube)
    {
        if (startCube == endCube)
            return true;

        Player.GetComponent<Player>().Done.Add(startCube);

        for (int i = 0; i < startCube.Neighbors.Length; i++)
        {
            if (!Player.GetComponent<Player>().Done.Contains(Neighbors[i].GetComponent<PositionsCube>()) && (Neighbors[i].GetComponent<PositionsCube>().FindCude(Neighbors[i].GetComponent<PositionsCube>(), endCube)))
            {
                Neighbors[i].GetComponent<PositionsCube>().Previous = startCube;
                Player.GetComponent<Player>().Iterations++;
                return true;
            }
        }

        return false;
    }

    List<PositionsCube> GetPath(PositionsCube lastCube)
    {
        List<PositionsCube> path = new List<PositionsCube>();

        PositionsCube current = lastCube;
        while (current != null)
        {
            path.Insert(0, current);
            current = current.Previous;
        }

        path.RemoveAt(0);
        return path;
    }

    void MoveEnd()
    {
        Player.GetComponent<Player>().CurrentCube = this;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < Neighbors.Length; i++)
        {
            Debug.DrawLine(transform.position + Offset, Neighbors[i].position + Neighbors[i].GetComponent<PositionsCube>().Offset, Color.red);
        }
        if (IsInWay)
            Debug.DrawLine(transform.position, transform.position + transform.right, Color.green);
    }
}
