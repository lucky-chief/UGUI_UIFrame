using System.Collections;

public class Node {

    public int X;
    public int Y;

    public Node(int x,int y)
    {
        X = x;
        Y = y;
    }

    public string Name
    {
        get { return string.Format("{0}-{1}", X, Y); }
    }

    public bool Equal(Node node)
    {
        return node.X == X && node.Y == Y;
    }

    public override string ToString()
    {
        return "(Node.X,Node.Y) = (" + X + ", " + Y + ")";
    }

    public Node LeftNode()
    {
        if(X - 1 >= 0)
        {
            return new Node(X - 1, Y);
        }
        return new Node(0, Y);
    }

    public Node RightNode()
    {
        if (X + 1 <= GridContainer.MAX_COL)
        {
            return new Node(X + 1, Y);
        }
        return new Node(GridContainer.MAX_COL - 1,Y);
    }

    public Node BottomNode()
    {
        if (Y -1 >= 0)
        {
            return new Node(X, Y - 1);
        }
        return new Node(X,0);
    }

    public Node TopNode()
    {
        if (Y + 1 <= GridContainer.MAX_ROW)
        {
            return new Node(X, Y + 1);
        }
        return new Node(X,GridContainer.MAX_ROW - 1);
    }

    public int Distance(Node node)
    {
        return UnityEngine.Mathf.Abs(Y - node.Y);
    }

    public static Node NameToNode(string name)
    {
        string[] coords = name.Split('-');
        if(coords.Length != 2)
        {
            return null;
        }
        return new Node(int.Parse(coords[0]), int.Parse(coords[1]));
    }

}
