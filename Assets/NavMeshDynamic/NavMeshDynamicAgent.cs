using System.Collections.Generic;
using UnityEngine;

using NodeTri = NavMeshDynamicGenerator.NodeTri;


public class NavMeshDynamicAgent : MonoBehaviour
{
    const int TryCountMax = 8000;

    struct NodeAStar
    {
        public int prev;
        public ChunkListIndex cli;
        public Vector3 pos;
        public float G;
        public float H;
        public float F { get { return G + H; } }

        public NodeAStar(ChunkListIndex cli, Vector3 pos, int prev = -1, float g = 0, float h = 0)
        {
            this.prev = prev;
            this.cli = cli;
            this.pos = pos;
            this.G = g;
            this.H = h;
        }

    }

    public NavMeshDynamicGenerator nmdGen;

    public float agentWidth;
    public float agentHeight;
    public float agentPerceptionRadius;

    Vector3 yOffset;
    float radius;

    #region AStar

    RaycastHit hit;

    Vector3 destination;

    List<NodeAStar> openList;
    List<NodeAStar> closedList;
    HashSet<int> checkedNodes;
    HashSet<ChunkListIndex> checkedTris;
    bool isDestinationReached;

    int tryCount;

    public bool IsDestinationReached { get { return isDestinationReached; } }

    public LayerMask groundLayers;

    List<Vector3> path;
    public List<Vector3> Path { get { return path; } }

    #endregion AStar


    private void Start()
    {
        Vector3 yOffset = Vector3.up * (agentHeight / 2);
        float radius = agentWidth / 2;

        #region AStar

        openList = new List<NodeAStar>();
        closedList = new List<NodeAStar>();
        checkedNodes = new HashSet<int>();
        checkedTris = new HashSet<ChunkListIndex>();

        path = new List<Vector3>();

        #endregion AStar
    }


    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
    }


    public void CalculatePath()
    {

        isDestinationReached = false;
        path.Clear();

        Vector3 startPos, destinationPos;
        ChunkListIndex startCLI, destinationCLI;


        if (!Physics.Raycast(transform.position + yOffset, Vector3.down, out hit, agentPerceptionRadius, groundLayers)) { return; }
        startPos = hit.point;
        if (!Physics.Raycast(destination, Vector3.down, out hit, agentPerceptionRadius, groundLayers)) { return; }
        destinationPos = hit.point;

        startCLI = FindClosestPointCLI(startPos);
        destinationCLI = FindClosestPointCLI(destinationPos);

        if(startCLI.Equals(destinationCLI)) { return; }

        if (startCLI.ListIndex == -1 || destinationCLI.ListIndex == -1) { return; }

        AStar(startCLI, destinationCLI);

    }


    ChunkListIndex FindClosestPointCLI(Vector3 position)
    {

        ChunkListIndex currCLI;
        ChunkListIndex closestPointCLI = new ChunkListIndex(Vector3Int.zero, 0, -1);

        List<NodeTri> leaf = null;

        int leafWidth = nmdGen.chunkSize / nmdGen.Triangles.Dimension;

        float minDist = float.MaxValue;
        float dist;
        minDist = float.MaxValue;


        for (int x = -1; x <= 1; x++) {
            for (int z = -1; z <= 1; z++) {
                for (int y = -1; y <= 1; y++) {

                    currCLI = nmdGen.Triangles.CalculateChunkLeafIndex(RoundVector3XZ(position + (leafWidth * new Vector3(x, y, z)), nmdGen.vertexMergeThreshold));
                    leaf = nmdGen.Triangles.GetLeaf(currCLI);

                    if (leaf == null) { continue; }

                    for (int i = 0; i < leaf.Count; i++)
                    {
                        currCLI.ListIndex = i;
                        dist = Vector3.Distance(position, nmdGen.Triangles.GetElement(currCLI).Center);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            closestPointCLI = currCLI;
                        }
                    }

                }
            }
        }

        return closestPointCLI;

    }


    void AStar(ChunkListIndex startCLI, ChunkListIndex destinationCLI)
    {

        openList.Clear();
        closedList.Clear();
        checkedTris.Clear();
        checkedNodes.Clear();

        int lowestFIndex;
        
        Vector3 destinationPos = nmdGen.Triangles.GetElement(destinationCLI).Center;

        NodeAStar startNode = new NodeAStar(startCLI, nmdGen.Triangles.GetElement(startCLI).Center, 0, Vector3.Distance(nmdGen.Triangles.GetElement(startCLI).Center, destinationPos));
        openList.Add(startNode);

        tryCount = TryCountMax;
        while (!isDestinationReached && tryCount > 0)
        {
            lowestFIndex = GetLowestFIndex();

            AddAdjacentNodes(lowestFIndex, closedList.Count, destinationCLI, destinationPos);

            closedList.Add(openList[lowestFIndex]);
            openList.RemoveAt(lowestFIndex);

            tryCount--;

            if(isDestinationReached || tryCount == 0 || openList.Count == 0)
            {
                break;
            }
        }
        
        if(isDestinationReached) {
            GeneratePath();
        }
        
    }


    int GetLowestFIndex()
    {
        int minInd = 0;
        float minVal = openList[0].F;

        for (int i = 1; i < openList.Count; i++)
        {

            if (openList[i].F < minVal)
            {
                minVal = openList[i].F;
                minInd = i;
            }
        }

        return minInd;
    }


    void AddAdjacentNodes(int openListIndex, int closedListIndex, ChunkListIndex destinationCLI, Vector3 destinationPos)
    {

        NodeAStar node = openList[openListIndex];
        NodeTri nodeTri = nmdGen.Triangles.GetElement(node.cli);
        Vector3 neighborPos;

        foreach(ChunkListIndex neighborCLI in nodeTri.Neighbors)
        {
            if(checkedTris.Contains(neighborCLI)) { continue; }

            checkedTris.Add(neighborCLI);

            neighborPos = nmdGen.Triangles.GetElement(neighborCLI).Center;

            if(Vector3.Distance(transform.position, neighborPos) > agentPerceptionRadius) { continue; }

            openList.Add(new NodeAStar(neighborCLI, neighborPos, closedListIndex, node.G + Vector3.Distance(node.pos, neighborPos), Vector3.Distance(neighborPos, destinationPos)));
            //Debug.DrawLine(nodeTri.Center, nodeTri.Center + Vector3.up * 2, Color.green);
            //Debug.DrawLine(nodeTri.Center, openList[openList.Count - 1].pos, Color.green);

            if (neighborCLI.Equals(destinationCLI))
            {
                isDestinationReached = true;
                return;
            }
        }
    }


    void GeneratePath()
    {
        path.Clear();

        Vector3 origin;
        Vector3 direction;

        int ind = closedList.Count - 1;
        int prev;

        while(ind > 0)
        {
            origin = closedList[ind].pos;
            path.Add(origin);

            prev = closedList[ind].prev;
            direction = closedList[prev].pos - origin;

            while (!Physics.SphereCast(new Ray(origin + yOffset, direction), radius, direction.magnitude, groundLayers) && ind > 0)
            {
                ind = prev;
                prev = closedList[ind].prev;
                direction = closedList[prev].pos - origin;

            }

        }
        path.Add(closedList[0].pos);
        path.Reverse();

        for(int i = 0; i < closedList.Count; i++)
        {
            Debug.DrawLine(closedList[i].pos, closedList[i].pos + 2 * yOffset, Color.red, 0.5f);
        }

    }


    static Vector3 RoundVector3XZ(Vector3 v3, float roundTo)
    {
        return new Vector3(RoundToNearestFloat(v3.x, roundTo), v3.y, RoundToNearestFloat(v3.z, roundTo));
    }


    static float RoundToNearestFloat(float f, float roundTo)
    {
        return Mathf.Round(f / roundTo) * roundTo;
    }


}
