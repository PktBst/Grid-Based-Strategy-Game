using Cube;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Player
{
    public class PlayerMovementScript : MonoBehaviour
    {
        public Vector3 PlayerCurrentPos;
        public Vector3 SelectedTilePos;
        public GridData gridData;
        [SerializeField] float moveSpeed = 3f;
        [SerializeField] TextMeshProUGUI PlayerPosLabel;
        [SerializeField] TextMeshProUGUI SelectedTileLabel;

        private bool isMoving = false;
        private Queue<Vector3> pathQueue = new Queue<Vector3>();

        void Update()
        {
            PlayerCurrentPos = transform.position;
            PlayerPosLabel.text = "Player Pos : (" + PlayerCurrentPos.x.ToString("F2") + "," + PlayerCurrentPos.z.ToString("F2") + ")";

            if (Input.GetMouseButtonDown(0) && !isMoving)
            {
                ReadTileOnClick();
            }
        }

        private void ReadTileOnClick()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                CubeScript cubeScript = hit.collider.GetComponent<CubeScript>();
                if (cubeScript != null)
                {
                    SelectedTilePos = cubeScript.Pos;
                    SelectedTileLabel.text = "Selected Tile : (" + SelectedTilePos.x + "," + SelectedTilePos.z + ")";
                    Debug.Log("Clicked Tile Selected");

                    CalculateAndMovePlayerToSelectedTile();
                }
            }
        }

        void CalculateAndMovePlayerToSelectedTile()
        {
            isMoving = true;
            List<Vector3> path = FindPath(PlayerCurrentPos, SelectedTilePos);
            pathQueue = new Queue<Vector3>(path);
            StartCoroutine(MoveAlongPath());
        }

        //deqeues path queue for next move and move player one tile at a time
        IEnumerator MoveAlongPath()
        {
            while (pathQueue.Count > 0)
            {
                Vector3 nextPos = pathQueue.Dequeue();
                // let player have orignal Y position
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


        // A* implementation
        List<Vector3> FindPath(Vector3 start, Vector3 target)
        {
            Vector2Int startNode = new Vector2Int((int)start.x, (int)start.z);
            Vector2Int targetNode = new Vector2Int((int)target.x, (int)target.z);

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
            return path;
        }

        int Heuristic(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        List<Vector2Int> GetNeighbors(Vector2Int node)
        {
            List<Vector2Int> neighbors = new List<Vector2Int>
            {
                new Vector2Int(node.x + 1, node.y),
                new Vector2Int(node.x - 1, node.y),
                new Vector2Int(node.x, node.y + 1),
                new Vector2Int(node.x, node.y - 1)
            };
            return neighbors;
        }

        //validation for path 
        bool IsWalkable(Vector2Int pos)
        {
            if (pos.x < 0 || pos.x >= 10 || pos.y < 0 || pos.y >= 10)
                return false;

            return !gridData.GetValue(pos.x, pos.y);
        }

    }
}
