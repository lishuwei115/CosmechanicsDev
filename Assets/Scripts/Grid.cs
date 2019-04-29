﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public static Grid instance;

    public LayerMask flamableMask;
    public Vector3 gridWorldSize;
    public GameObject fireEffect;
    public float nodeRadius;
    public Node[,] grid;
    List<Node> fires = new List<Node>();

    float nodeDiameter;
    public int gridSizeX, gridSizeY;

    [Header("Fire Statistics")]
    public float fireStartPercentage;
    public float fireTimer;

    [Header("Debug tools")]
    [SerializeField] bool GenerateGrid;
    [SerializeField] bool LightFire;
    [SerializeField] bool showGrid;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();
    }

    public void Update()
    {
        for(int i = 0; i < fires.Count; ++i)
        {
            onFire(fires[i]);            
        }
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool flameable = (Physics.CheckSphere(worldPoint, nodeRadius, flamableMask));
                grid[x, y] = new Node(flameable, worldPoint, x, y, fireTimer);
                
            }
        }
        
        //Debug.Log("Length of grid is " + grid.Length);
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }


    public List<Node> GetFlamableNeighbors(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
<<<<<<< HEAD
                    if(grid[checkX, checkY].isFlamable)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
=======
                    //Debug.Log("Here there be fire");
                    GameObject fireObject = Instantiate(fireEffect, firePos.worldPosition, Quaternion.Euler(0f, 0f, 0f));
                    Fire fireComponent = fireObject.GetComponent<Fire>();
                    fireComponent.thisNode = firePos;
                    fireComponent.fireLocation.nodes = GetNeighbors(firePos);
                    fires.Add(fireObject);
                    EndGameScore.instance.FiresActive(1);
                    firePos.isFlamable = false;
>>>>>>> dev
                }
            }
        }
        return neighbours;
    }

    public void GenerateLaserFire(Node firePos)
    {
        int chanceToStartFire = Random.Range(0, 100);

        if (chanceToStartFire < fireStartPercentage)
        {
            if (firePos.isFlamable)
            {
                firePos.fireTimer = fireTimer;
                firePos.isFlamable = false;
                fires.Add(firePos);
                GameObject fireObject = Instantiate(fireEffect, firePos.worldPosition, Quaternion.Euler(0f, 0f, 0f));
                //Fire fireComponent = fireObject.GetComponent<Fire>();
                //fireComponent.thisNode = firePos;
                //fireComponent.fireLocation.nodes = GetNeighbors(firePos);
                //fires.Add(fireObject);
                //firePos.isFlamable = false;
            }
        }
    }

    public void GenerateFire(Node firePos)
    {
        if(firePos.isFlamable && firePos != null)
        {
            firePos.fireTimer = fireTimer;
            firePos.isFlamable = false;
            fires.Add(firePos);
            GameObject fireObject = Instantiate(fireEffect, firePos.worldPosition, Quaternion.Euler(0f, 0f, 0f));
        }

    }


    public void onFire(Node firePos)
    {
        Collider[] castedObjects = Physics.OverlapSphere(firePos.worldPosition, 1);
        

        firePos.fireTimer -= Time.deltaTime;
        if(firePos.fireTimer < 0)
        {
            List<Node> flameableNeighbors = GetFlamableNeighbors(firePos);

            if (flameableNeighbors.Count > 0)
            {
                int index = Random.Range(0, flameableNeighbors.Count);
                Debug.Log("List size was :" + flameableNeighbors.Count + " Index chosen :" + index);
                GenerateFire(flameableNeighbors[index]);
            }
            firePos.fireTimer = fireTimer;
        }

    }


    private void OnDrawGizmos()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        if(GenerateGrid)
            CreateGrid();

        if(LightFire)
        {
            //GenerateFire();
            LightFire = false;
        }


        if (grid != null && showGrid)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.isFlamable) ? Color.white : Color.red;
                Gizmos.DrawWireCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
            foreach (Node fire in fires)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(fire.worldPosition, 1);
            }
        }

    }
}