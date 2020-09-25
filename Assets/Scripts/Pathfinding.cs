using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinding {

    public static List<Vector2> AStarSearch(float[,] map, Vector2 start, Vector2 goal) {
        List<Vector2> openList = new List<Vector2>();

        Dictionary<Vector2, float> gcosts = new Dictionary<Vector2, float>();
        Dictionary<Vector2, float> fcosts = new Dictionary<Vector2, float>();

        Dictionary<Vector2, Vector2> previousNodes = new Dictionary<Vector2, Vector2>();

        // Initialize values of all nodes to negative infinity
        for (int z = 0; z < map.GetLength(1); z++) {
            for (int x = 0; x < map.GetLength(0); x++) {
                gcosts.Add(new Vector2(x, z), -Mathf.Infinity);
                fcosts.Add(new Vector2(x, z), -Mathf.Infinity);
            }
        }

        gcosts[start] = 0f;
        fcosts[start] = Heuristic(map, start, goal);

        // Search attempts to maximize the height difference at each step in the path
        openList.Add(start);
        while (openList.Count > 0) {
            // Get current node from with greatest positive height difference in openList
            Vector2 node = openList[0];
            float maxScore = 0f;
            foreach (Vector2 n in openList) {
                if (fcosts[n] > maxScore) {
                    node = n;
                    maxScore = fcosts[n];
                }
            }

            // Break and retrace path if goal is found
            if (node == goal) {
                return GetPath(previousNodes, node);
            }
            openList.Remove(node);

            // Loop through neighbours of current node
            foreach (Vector2 neighbour in GetNeighbours(map, node)) {
                // If g(node) + cost(node, neighbour) > g(neighbour), selected neighbour node is improving
                float score = gcosts[node] + Cost(map, node, neighbour);
                if (score > gcosts[neighbour]) {
                    gcosts[neighbour] = score;
                    fcosts[neighbour] = score + Heuristic(map, neighbour, goal);

                    previousNodes[neighbour] = node;

                    if (!openList.Contains(neighbour)) {
                        openList.Add(neighbour);
                    }
                }
            }
        }

        return new List<Vector2>();
    }

    private static List<Vector2> GetNeighbours(float[,] map, Vector2 node) {
        int width = map.GetLength(0);
        int height = map.GetLength(1);

        List<Vector2> neighbours = new List<Vector2>();

        // Get left neighbour
        if (node.x > 0) {
            neighbours.Add(new Vector2(node.x - 1, node.y));
        }
        // Get right neighbour
        if (node.x < width - 1) {
            neighbours.Add(new Vector2(node.x + 1, node.y));
        }
        // Get top neighbour
        if (node.y > 0) {
            neighbours.Add(new Vector2(node.x, node.y - 1));
        }
        // Get bottom neighbour
        if (node.y < height - 1) {
            neighbours.Add(new Vector2(node.x, node.y + 1));
        }

        return neighbours;
    }

    private static float Cost(float[,] map, Vector2 point, Vector2 candidate) {
        return map[(int)point.x, (int)point.y] - map[(int)candidate.x, (int)candidate.y];
    }

    private static float Heuristic(float[,] map, Vector2 candidate, Vector2 goal) {
        return map[(int)candidate.x, (int)candidate.y] - map[(int)goal.x, (int)goal.y];
    }

    private static List<Vector2> GetPath(Dictionary<Vector2, Vector2> previousNodes, Vector2 node) {
        List<Vector2> path = new List<Vector2>();
        path.Add(node);

        while (previousNodes.ContainsKey(node)) {
            node = previousNodes[node];
            path.Add(node);
        }

        return path;
    }
}
