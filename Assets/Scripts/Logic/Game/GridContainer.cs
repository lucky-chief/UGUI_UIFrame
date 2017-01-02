using UnityEngine;
using System.Collections.Generic;

public class GridContainer : MonoBehaviour
{
    public const int MAX_ROW = 12;
    public const int MAX_COL = 6;
    public const int GRID_WIDTH = 145;
    public const int GRID_HEIGHT = 120;
    public const int GRID_GAP = 5;

    public float gridMoveSpeed = 20f;

    private KeyboardInput keyboardInput;
    private Dictionary<int, Grid> rockDic = new Dictionary<int, Grid>();
    private List<Grid> selfGridAddedList = new List<Grid>();
    private List<Grid> enemyGridAddedList = new List<Grid>();
    private List<Node> middleNodeList = new List<Node>() {new Node(0,5), new Node(1, 5), new Node(2, 5), new Node(3, 5), new Node(4, 5), new Node(5, 5),
                                                          new Node(0,6), new Node(1, 6), new Node(2, 6), new Node(3, 6), new Node(4, 6), new Node(5, 6)};
    private List<int> selfColDestNode = new List<int>(MAX_COL) { 4, 4, 4, 4, 4, 4 };
    private List<int> enemyColDestNode = new List<int>(MAX_COL) { 7, 7, 7, 7, 7, 7 };

    void OnEnable()
    {
        keyboardInput = Game.Instance().GetUpdateObj<KeyboardInput>() as KeyboardInput;
        keyboardInput.handleInput += HandleInput;
    }

    void OnDisable()
    {
        keyboardInput.handleInput -= HandleInput;
    }

    void Awake()
    {
        PushBoxGame.Instance.gridContainer = this;
    }
    // Use this for initialization
    void Start()
    {
        PushBoxGame.Instance.RandomNextGroup();
        InitMap();
        SpawnGridGroup(new Node(2, 0));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<Grid> SelfGrids
    {
        get { return selfGridAddedList; }
    }

    public List<Grid> EnemyGrids
    {
        get { return enemyGridAddedList; }
    }

    public List<int> SelfColDestNode
    {
        get { return SelfColDestNode; }
    }

    public List<int> EnemyColDestNode
    {
        get { return enemyColDestNode; }
    }

    //放置地图中间两行的箱子
    public void InitMap(List<Grid> initedGrids = null)
    {
        if (null == initedGrids)
        {
            foreach (Node node in middleNodeList)
            {
                Grid grid = Grid.Spawn(node.X % 2 == 0 ? (5 == node.Y ? GridValue.grid_zero : (GridValue)Random.Range(1, 7)) : (5 == node.Y ? (GridValue)Random.Range(1, 7) : GridValue.grid_zero), node);
                if (grid.Value == GridValue.grid_zero)
                {
                    rockDic.Add(grid.Node.X, grid);
                }
                else
                {
                    if (grid.Node.Y == 5)
                    {
                        selfGridAddedList.Add(grid);
                    }
                    else
                    {
                        enemyGridAddedList.Add(grid);
                    }
                }
            }
        }
        else
        {

        }
    }

    public List<Grid> AddGrid(Node midNode)
    {
        List<Grid> retList = new List<Grid>();
        List<GridValue> nextGroup = PushBoxGame.Instance.nextGroup;
        retList.Add(Grid.Spawn(nextGroup[0], midNode.LeftNode()));
        retList.Add(Grid.Spawn(nextGroup[1], midNode));
        //retList.Add(Grid.Spawn(nextGroup[2], midNode.RightNode()));
        return retList;
    }

    public void SpawnGridGroup(Node midNode)
    {
        PushBoxGame.Instance.gridGroup = AddGrid(midNode);
        PushBoxGame.Instance.RandomNextGroup();
    }

    public void MoveRock(int colIdx,int step)
    {
        Grid rock = rockDic[colIdx];
        rock.Move(new Node(colIdx, rock.Node.Y + step), 20, step > 0);
    }

    public void UpdateSelfNodes(int colIdx)
    {
        Grid rock = rockDic[colIdx];
        List<Grid> grids = new List<Grid>(MAX_ROW - 1);
        for (int i = 0; i < selfGridAddedList.Count; i++)
        {
            Grid gd = selfGridAddedList[i];
            if (gd.Node.X == colIdx)
            {
                grids.Add(gd);
            }
        }
        selfColDestNode[colIdx] = rock.Node.Y - grids.Count - 1;
        if (0 == grids.Count) return;

        grids.Sort((Grid small, Grid big) =>
        {
            return big.Node.Y - small.Node.Y;
        });
        int idx = 0;
        for (int i = rock.Node.Y - 1; i > rock.Node.Y - grids.Count - 1; i--)
        {
            if (grids[idx].Node.Y < i)
            {
                grids[idx].Move(new Node(colIdx, i), 20);
            }
            idx++;
        }
    }

    public bool Contains(Grid grid, bool self = true)
    {
        List<Grid> tgtList = self ? selfGridAddedList : enemyGridAddedList;
        return tgtList.Exists((Grid it) =>
        {
            return it.Node.Equal(grid.Node);
        });
    }

    public void GridInputHandle(HandleType handleType)
    {
        if (handleType == HandleType.handleLeft || handleType == HandleType.handleRight)
        {
            MoveGridGroup(handleType == HandleType.handleLeft);
        }
        else if (handleType == HandleType.hanelePush)
        {
            List<Grid> grids = PushBoxGame.Instance.gridGroup;
            for (int i = 0; i < grids.Count; i++)
            {
                //Node node = MoveDestinateNode(grids[i].Node, true);
                Node node = new Node(grids[i].Node.X, selfColDestNode[grids[i].Node.X]);
                grids[i].Move(node, 20);
            }
            SpawnGridGroup(grids[(int)(0.5f * (grids.Count))].Node.RightNode());
        }
    }

    public static Vector3 NodeToPosition(Node node)
    {
        float xPos = (node.X + 0.5f) * GRID_WIDTH + node.X * GRID_GAP;
        float yPos = (node.Y + 0.5f) * GRID_HEIGHT + node.Y * GRID_GAP;
        return new Vector3(xPos, yPos, 0);
    }

    public static Node PositionToNode(Vector3 pos)
    {
        int nodeX = (int)(pos.x / GRID_WIDTH);
        int nodeY = (int)(pos.y / GRID_HEIGHT);
        return new Node(nodeX, nodeY);
    }

    void HandleInput(HandleType type)
    {
        GridInputHandle(type);
    }

    private void MoveGridGroup(bool left)
    {
        List<Grid> grids = PushBoxGame.Instance.gridGroup;
        if (grids.Count > 0)
        {
            if (left)
            {
                if (grids[0].Node.X - 1 < 0) return;
                for (int i = 0; i < grids.Count; i++)
                {
                    grids[i].SetNode(grids[i].Node.LeftNode());
                }
            }
            else
            {
                if (grids[grids.Count - 1].Node.X + 1 >= GridContainer.MAX_COL) return;
                for (int i = 0; i < grids.Count; i++)
                {
                    grids[i].SetNode(grids[i].Node.RightNode());
                }
            }
        }
    }
}
