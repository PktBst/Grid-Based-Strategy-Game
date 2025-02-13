using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAIScript : MonoBehaviour, IAI
{
    private GameObject Player;
    private Vector3 PlayerCurrentPos;
    private Vector3 LastPlayerPos;

    [SerializeField] float moveSpeed = 1f;
    public GridData gridData;
    private bool isMoving = false;
    private Queue<Vector3> pathQueue = new Queue<Vector3>();

    public void FollowPlayer()
    {
        if (!isMoving)
            CalculateAndMovePlayerToSelectedTile();
    }

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (Player == null) return;
        PlayerCurrentPos = Player.transform.position;

        if (PlayerCurrentPos != LastPlayerPos && !isMoving)
        {
            LastPlayerPos = PlayerCurrentPos;
            FollowPlayer();
        }
    }

    void CalculateAndMovePlayerToSelectedTile()
    {
        List<Vector3> path = FindPath(transform.position, PlayerCurrentPos);

        if (path.Count > 0)
        {
            pathQueue = new Queue<Vector3>(path);
            StartCoroutine(MoveAlongPath());
        }
    }

    IEnumerator MoveAlongPath()
    {
        isMoving = true;
        while (pathQueue.Count > 0)
        {
            Vector3 nextPos = pathQueue.Dequeue();
            Vector3 targetPos = new Vector3(nextPos.x, transform.position.y, nextPos.z);

            while (Vector3.Distance(transform.position, targetPos) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
                yield return null;
            }

            transform.position = targetPos;
        }
        isMoving = false;
    }

    List<Vector3> FindPath(Vector3 start, Vector3 playerPos)
    {
        Vector2Int startNode = new Vector2Int(Mathf.RoundToInt(start.x), Mathf.RoundToInt(start.z));
        Vector2Int playerNode = new Vector2Int(Mathf.RoundToInt(playerPos.x), Mathf.RoundToInt(playerPos.z));

        if (startNode == playerNode) return new List<Vector3>();

        return AStarPathfinding(startNode, playerNode);
    }

    List<Vector3> AStarPathfinding(Vector2Int startNode, Vector2Int targetNode)
    {
        List<Vector2Int> openList = new List<Vector2Int> { startNode };
        HashSet<Vector2Int> closedList = new HashSet<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, int> gScore = new Dictionary<Vector2Int, int> { { startNode, 0 } };
        Dictionary<Vector2Int, int> fScore = new Dictionary<Vector2Int, int> { { startNode, Heuristic(startNode, targetNode) } };

        while (openList.Count > 0)
        {
            openList.Sort((a, b) => fScore[a].CompareTo(fScore[b]));
            Vector2Int current = openList[0];
            openList.RemoveAt(0);
            closedList.Add(current);

            if (current == targetNode)
                return ReconstructPath(cameFrom, current);

            foreach (Vector2Int neighbor in GetNeighbors(current))
            {
                if (closedList.Contains(neighbor) || !IsWalkable(neighbor))
                    continue;

                int tentativeGScore = gScore[current] + 1;

                if (!openList.Contains(neighbor))
                    openList.Add(neighbor);
                else if (tentativeGScore >= gScore.GetValueOrDefault(neighbor, int.MaxValue))
                    continue;

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, targetNode);
            }
        }
        return new List<Vector3>();
    }

    List<Vector3> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        List<Vector3> path = new List<Vector3>();

        while (cameFrom.ContainsKey(current))
        {
            path.Add(new Vector3(current.x, 0, current.y));
            current = cameFrom[current];
        }
        path.RemoveAt(0);
        path.Reverse();
        return path;
    }

    int Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    List<Vector2Int> GetNeighbors(Vector2Int node)
    {
        return new List<Vector2Int>
        {
            new Vector2Int(node.x + 1, node.y),
            new Vector2Int(node.x - 1, node.y),
            new Vector2Int(node.x, node.y + 1),
            new Vector2Int(node.x, node.y - 1)
        };
    }

    bool IsWalkable(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= 10 || pos.y < 0 || pos.y >= 10)
            return false;
        return !gridData.GetValue(pos.x, pos.y);
    }
}
