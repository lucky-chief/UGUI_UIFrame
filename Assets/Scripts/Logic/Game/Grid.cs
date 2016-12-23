using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum GridValue
{
    grid_zero,//
    grid_one,
    grid_two,
    grid_three,
    grid_four,
    grid_five,
    grid_six,
}

public class Grid : MonoBehaviour {
    public Text valeText;
    private GridValue value;
    private Node node;
    private bool moving;
    private Node destNode;
    private float speed;
    private Vector3 moveDir;
    private Node moveDestNode;
    private bool self;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(moving)
        {
            if(node.Distance(destNode) == 1)
            {
                if(self)
                {
                    if(GridContainer.NodeToPosition(moveDestNode).y >= transform.localPosition.y)
                    {
                        Move();
                    }
                    else
                    {
                        moving = false;
                        SetNode(moveDestNode);
                        PushBoxGame.Instance.gridContainer.SelfGrids.Add(this);
                        PushBoxGame.Instance.CheckRemove(true);
                    }
                }
                else
                {
                    if (GridContainer.NodeToPosition(moveDestNode).y <= transform.localPosition.y)
                    {
                        Move();
                    }
                    else
                    {
                        moving = false;
                        SetNode(moveDestNode);
                        PushBoxGame.Instance.gridContainer.enemyGrids.Add(this);
                    }
                }
            }
            else
            {
                Move();
            }
        }
	}

    public GridValue Value
    {
        get { return this.value; }
        
        set{
            this.value = value;
            if (null != valeText)
            {
                valeText.text = (int)value + "";
            }
        }
    }

    public Node Node
    {
        get { return node; }
        set
        {
            node = value;
        }
    }

    public void Move(Node destNode, float speed, bool self = true)
    {
        this.speed = speed;
        this.destNode = destNode;
        this.self = self;
        moveDestNode = new Node(destNode.X, destNode.Y - 1);
        Vector3 pos = GridContainer.NodeToPosition(node);
        moveDir = (GridContainer.NodeToPosition(destNode) - pos).normalized;
        moveDir.x = moveDir.z = 0;
        moving = true;
    }

    public void SetNode(Node node)
    {
        this.node = node;
        transform.localPosition = GridContainer.NodeToPosition(node);
    }

    public static Grid Spawn(GridValue gridValue,Node node)
    {
        GameObject model = Resources.Load<GameObject>(gridValue == GridValue.grid_zero ? "Prefabs/Game/Rock" : "Prefabs/Game/Grid");
        GameObject grid = GameObject.Instantiate<GameObject>(model);
        grid.transform.SetParent(PushBoxGame.Instance.gridContainer.transform);
        grid.transform.localScale = Vector3.one;
        Vector2 pos = GridContainer.NodeToPosition(node);
        grid.transform.localPosition = new Vector3(pos.x,pos.y,0);
        Grid gridCom = grid.GetComponent<Grid>();
        gridCom.Value = gridValue;
        gridCom.Node = node;
        //grid.GetComponent<RectTransform>().sizeDelta = new Vector2(GridContainer.GRID_WIDTH,GridContainer.GRID_HEIGHT);
        return gridCom;
    }

    private void Move()
    {
        transform.Translate(moveDir * speed * Time.deltaTime, Space.Self);
        node = GridContainer.PositionToNode(transform.localPosition);
    }
}
