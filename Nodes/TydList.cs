namespace Tyd
{

///<summary>
/// Contains an ordered collection of anonymous TydNodes.
/// Generally used to represent lists of items.
///</summary>
public class TydList : TydCollection
{
    public TydList(string name, TydNode parent, int docLine=-1) : base(name, parent, docLine)
    {
    }

    public override TydNode DeepClone()
    {
        TydList c = new TydList(name, Parent, docLine);
        CopyDataFrom(c);
        return c;
    }

    public override string ToString()
    {
        return string.Format("{0}({1}, {2})", Name, "TydList", Count);
    }
}

}