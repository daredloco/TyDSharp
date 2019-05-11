using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tyd
{

///<summary>
/// A TydNode that contains a collection of sub-nodes.
///</summary>
public abstract class TydCollection : TydNode, IEnumerable<TydNode>
{
    //Data
    protected List<TydNode> nodes = new List<TydNode>();
    protected Dictionary<string, string> attributes;

    //Properties
    public string AttributeClass
    {
        get { return GetAttributeOrNull("class"); }
        set { SetAttribute("class", value); }
    }

    public string AttributeHandle
    {
        get { return GetAttributeOrNull("handle"); }
        set { SetAttribute("handle", value); }
    }

    public string AttributeSource
    {
        get { return GetAttributeOrNull("source"); }
        set { SetAttribute("source", value); }
    }

    public int              Count
    {
        get { return nodes.Count; }
    }

    public List<TydNode>    Nodes
    {
        get { return nodes; }
        set { nodes = value; }
    }

    public TydNode          this[int index]
    {
        get { return nodes[index]; }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<TydNode> GetEnumerator()
    {
        foreach( TydNode n in nodes )
        {
            yield return n;
        }
    }

    public TydCollection(string name, TydNode parent, int docLine=-1) : base(name, parent, docLine)
    {
    }

    public void SetupAttributes( Dictionary<string,string>  attributes)
    {
        this.attributes = attributes;
    }

    /// <summary>
    /// Return all values in child nodes converted to type T, non recursively
    /// </summary>
    /// <param name="onlyStrings">Whether to throw an exception if non-strings are encountered</param>
    public IEnumerable<T> GetChildValues<T>(bool onlyStrings = true)
    {
        for (var i = 0; i < nodes.Count; i++)
        {
            var str = nodes[i] as TydString;
            if (str == null)
            {
                if (onlyStrings)
                {
                    throw new Exception(string.Format("Could not convert node in {0} as it is not a string", Name));
                }
                else
                {
                    continue;
                }
            }
            yield return str.GetValue<T>(Name);
        }
    }

    /// <summary>
    /// Return all values in child nodes, non recursively
    /// </summary>
    /// <param name="onlyStrings">Whether to throw an exception if non-strings are encountered</param>
    public IEnumerable<string> GetChildValues(bool onlyStrings = true)
    {
        for (var i = 0; i < nodes.Count; i++)
        {
            var str = nodes[i] as TydString;
            if (str == null)
            {
                if (onlyStrings)
                {
                    throw new Exception(string.Format("Node in {0} is not a string", Name));
                }
                else
                {
                    continue;
                }
            }
            yield return str.Value;
        }
    }

    /// <summary>
    /// Returns the value of the child with the specified name, converted to the type T, if it is a TydString
    /// </summary>
    /// /// <param name="required">Whether to throw an exception if the node does not exist</param>
    public T GetChildValue<T>(string name, bool required = true, T defaultValue = default(T))
    {
        for (var i = 0; i < nodes.Count; i++)
        {
            var child = nodes[i];
            
            if (name.Equals(child.Name))
            {
                return GetChildValue<T>(i);
            }
        }
        if (required)
        {
            throw new Exception(string.Format("Missing node {0} in {1}", name, Name));
        }
        return defaultValue;
    }


    /// <summary>
    /// Returns the value of the child with the specified name, if it is a TydString
    /// </summary>
    /// /// <param name="required">Whether to throw an exception if the node does not exist</param>
    public string GetChildValue(string name, bool required = true)
    {
        for (var i = 0; i < nodes.Count; i++)
        {
            var child = nodes[i];
            if (name.Equals(child.Name))
            {
                return GetChildValue(i);
            }
        }
        if (required)
        {
            throw new Exception(string.Format("Missing node {0} in {1}", name, Name));
        }
        return null;
    }

    /// <summary>
    /// Returns the value of the child at index idx, converted to the type T, if it is a TydString
    /// </summary>
    public T GetChildValue<T>(int idx)
    {
        if (idx < 0 || idx >= nodes.Count)
        {
            throw new Exception(string.Format("Index is out of bounds for {0}", Name));
        }
        var str = nodes[idx] as TydString;
        if (str == null)
        {
            throw new Exception(string.Format("Node {0} in {1} is not a string", nodes[idx].Name, Name));
        }
        return str.GetValue<T>();
    }

    /// <summary>
    /// Returns the value of the child at index idx, if it is a TydString
    /// </summary>
    public string GetChildValue(int idx)
    {
        if (idx < 0 || idx >= nodes.Count)
        {
            throw new Exception(string.Format("Index is out of bounds for {0}", Name));
        }
        var str = nodes[idx] as TydString;
        if (str == null)
        {
            throw new Exception(string.Format("Node {0} in {1} is not a string", nodes[idx].Name, Name));
        }
        return str.Value;
    }

    /// <summary>
    /// Returns the child with the specified name
    /// </summary>
    /// <param name="required">Whether to throw an exception if the node does not exist</param>
    public TydNode GetChild(string name, bool required = false)
    {
        for (var i = 0; i < nodes.Count; i++)
        {
            var child = nodes[i];
            if (name.Equals(child.Name))
            {
                return child;
            }
        }
        if (required)
        {
            throw new Exception(string.Format("Missing node {0} in {1}", name, Name));
        }
        return null;
    }


    /// <summary>
    /// Returns the child with the specified name as the specified Tyd Type
    /// </summary>
    /// <typeparam name="T">A valid Tyd node type</typeparam>
    /// <param name="required">Whether to throw an exception if the node does not exist</param>
    public T GetChild<T>(string name, bool required = false) where T : TydNode
    {
        for (var i = 0; i < nodes.Count; i++)
        {
            var child = nodes[i];
            if (name.Equals(child.Name))
            {
                return (T)child;
            }
        }
        if (required)
        {
            throw new Exception(string.Format("Missing node {0} in {1}", name, Name));
        }
        return null;
    }

    public IEnumerable<KeyValuePair<string, string>> GetAttributes()
    {
        if (attributes != null)
        {
            foreach (var pair in attributes)
            {
                yield return pair;
            }
        }
    }

    public void SetAttribute(string key, string value)
    {
        if (attributes == null)
        {
            attributes = new Dictionary<string, string>();
        }
        attributes[key] = value;
    }


    /// <summary>
    /// Remove an attribute from the table
    /// </summary>
    /// <param name="unset">Whether to remove or add with no value</param>
    public void UnsetAttribute(string key, bool unset)
    {
        if (attributes == null)
        {
            if (unset)
            {
                return;
            }
            attributes = new Dictionary<string, string>();
        }
        if (unset)
        {
            attributes.Remove(key);
        }
        else
        {
            attributes[key] = null;
        }
    }

    /// <summary>
    /// Try getting an attribute value
    /// </summary>
    public bool TryGetAttribute(string key, out string value)
    {
        if (attributes != null)
        {
            return attributes.TryGetValue(key, out value);
        }
        value = null;
        return false;
    }

    /// <summary>
    /// Whether the table has the attribute defined
    /// </summary>
    public bool HasAttribute(string key)
    {
        if (attributes != null)
        {
            return attributes.ContainsKey(key);
        }
        return false;
    }

    /// <summary>
    /// Returns the value of ann attribute or null
    /// </summary>
    public string GetAttributeOrNull(string key)
    {
        if (attributes != null)
        {
            string value;
            if (attributes.TryGetValue(key, out value))
            {
                return value;
            }
        }
        return null;
    }

    ///<summary>
    /// Add a node as a child of this node, and link it as a parent.
    ///</summary>
    public void AddChild( TydNode node )
    {
        nodes.Add(node);
        node.Parent = this;
    }

    protected void CopyDataFrom( TydCollection other )
    {
        other.docIndexEnd = docIndexEnd;
        other.attributes = other.attributes == null ? null : other.attributes.ToDictionary(x => x.Key, x => x.Value);
        for( int i=0; i<nodes.Count; i++ )
        {
            other.AddChild( nodes[i].DeepClone() );
        }
    }
}

}