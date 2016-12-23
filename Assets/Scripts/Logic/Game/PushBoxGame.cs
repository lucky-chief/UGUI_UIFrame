using System.Collections.Generic;

public class PushBoxGame
{
    public static readonly PushBoxGame Instance = new PushBoxGame();
    public GridContainer gridContainer;
    public List<GridValue> nextGroup { get; private set; }
    public List<Grid> gridGroup;

    private PushBoxGame() {}

    public void CheckRemove(bool self)
    {
        if(self)
        {
            List<Grid> grids = FindTripletGrids(gridContainer.SelfGrids, true);//横向
            grids.AddRange(FindTripletGrids(gridContainer.SelfGrids, false));//纵向
            if(grids.Count > 0)
            {

            }
        }
    }

    List<Grid> FindTripletGrids(List<Grid> sortedGrids, bool horizontal)
    {
        List<Grid> retList = new List<Grid>();
        List<Grid> tempList = new List<Grid>();
        bool h = horizontal;
        int max = int.MinValue;
        int min = int.MaxValue;
        for (int i = 0; i < sortedGrids.Count; i++)
        {
            Grid grid = sortedGrids[i];
            int idx = h ? grid.Node.Y : grid.Node.X;
            if (idx > max)
            {
                max = h ? grid.Node.Y : grid.Node.X;
            }
            if (idx < min)
            {
                min = h ? grid.Node.Y : grid.Node.X;
            }
        }

        for (int i = min; i <= max; i++)
        {
            for (int j = 0; j < sortedGrids.Count; j++)
            {
                if (i == (h ? sortedGrids[j].Node.Y : sortedGrids[j].Node.X))
                {
                    tempList.Add(sortedGrids[j]);
                }
            }
            if(tempList.Count < 3)
            {
                tempList.Clear();
                continue;
            }
            else
            {
                tempList.Sort((Grid small, Grid big) =>
                {
                    return h ? small.Node.X - big.Node.X : small.Node.Y - big.Node.Y;
                });
                List<Grid> ls = new List<Grid>() { tempList[0] };
                for (int j = 1; j < tempList.Count; j++)
                {
                    if ((h ? tempList[j].Node.X - tempList[j - 1].Node.X : tempList[j].Node.Y - tempList[j - 1].Node.Y) == 1)
                    {
                        ls.Add(tempList[j]);
                    }
                    else
                    {
                        ls.Clear();
                        if (tempList.Count - j < 3) break;
                    }
                }
                if (ls.Count >= 3)
                {
                    retList.AddRange(ls);
                }
            }
        }

        return retList;
    }

    public List<GridValue> RandomNextGroup()
    {
        List<GridValue> retList = new List<GridValue>();
        List<int> random = new List<int> { 1, 2, 3, 4, 5, 6 };
        int rnd = UnityEngine.Random.Range(0, 6);
        retList.Add((GridValue)random[rnd]);
        rnd = UnityEngine.Random.Range(0, 6);
        retList.Add((GridValue)random[rnd]);
        random.RemoveAt(rnd);
        rnd = UnityEngine.Random.Range(0, 5);
        retList.Add((GridValue)random[rnd]);
        nextGroup = retList;
        return retList;
    }

}
