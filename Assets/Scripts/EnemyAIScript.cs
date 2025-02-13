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
        CalculateAndMovePlayerToSelectedTile();
    }

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        PlayerCurrentPos = Player.transform.position;

        if (PlayerCurrentPos != LastPlayerPos && !isMoving)
        {
            LastPlayerPos = PlayerCurrentPos;
            FollowPlayer();
        }
    }

    void CalculateAndMovePlayerToSelectedTile()
    {
        isMoving = true;
        List<Vector3> path = FindPath(transform.position, PlayerCurrentPos);

        if (path.Count > 0)
        {
            pathQueue = new Queue<Vector3>(path);
            StartCoroutine(MoveAlongPath());
        }
        else
        {
            isMoving = false;
        }
    }

    IEnumerator MoveAlongPath()
    {
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

    //updated A* for enemy
    List<Vector3> FindPath(Vector3 start, Vector3 playerPos)
    {
        Vector2Int startNode = new Vector2Int((int)start.x, (int)start.z);
        Vector2Int playerNode = new Vector2Int((int)playerPos.x, (int)playerPos.z);

        // Get valid adjacent tiles around the player
        List<Vector2Int> adjacentTiles = GetNeighbors(playerNode).FindAll(IsWalkable);

        // If no adjacent tile is walkable, return an empty path
        if (adjacentTiles.Count == 0) return new List<Vector3>();

        // If already in an adjacent tile, don’t move
        if (adjacentTiles.Contains(startNode)) return new List<Vector3>();

        // Find the closest adjacent tile to the enemy
        Vector2Int targetNode = adjacentTiles.OrderBy(t => Heuristic(startNode, t)).First();

        return AStarPathfinding(startNode, targetNode);
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
                else if (tentativeGScore >= gScore[neighbor])
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

        path.Reverse();

        // Remove last tile in path so the enemy stops at an adjacent tile
        if (path.Count > 0) path.RemoveAt(path.Count - 1);

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
