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
        return null;
    }

    public Node RightNode()
    {
        if (X + 1 <= GridContainer.MAX_COL)
        {
            return new Node(X + 1, Y);
        }
        return null;
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
