using System;
using System.ComponentModel;

namespace Tyd
{

///<summary>
/// Represents a record of a single string. Also used to represent records with null values.
///</summary>
public class TydString : TydNode
{
    //Data
    private string val;

    //Properties
    public string Value
    {
        get { return val; }
        set { this.val = value; }
    }

    public TydString(string name, string val, TydNode parent, int docLine=-1) : base(name, parent, docLine)
    {
        this.val = val;
    }

    public override TydNode DeepClone()
    {
        TydString c = new TydString(name, val, Parent, docLine);
        c.docIndexEnd = docIndexEnd;
        return c;
    }

    public override string ToString()
    {
        return string.Format("{0}=\"{1}\"", Name ?? "NullName", val);
    }

    /// <summary>
    /// Converts the string to a value of type T
    /// </summary>
    /// <param name="name">If this is a nameless record from list, you can supply the list name here for better exception messages</param>
    public T GetValue<T>(string name = null)
    {
        var t = typeof(T);
        var val = TypeDescriptor.GetConverter(t).ConvertFrom(Value);
        if (val != null)
        {
            return (T)val;
        }
        else
        {
            throw new Exception(string.Format("Could not convert node {1} to {0}", t.Name, name ?? Name));
        }
    }
}

}