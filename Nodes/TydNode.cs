namespace Tyd
    {
    ///<summary>
    /// Root class of all Tyd _nodes.
    ///</summary>
    public abstract class TydNode
        {
        //Data
        protected string _name;          //Can be null for anonymous _nodes

        //Data for error messages
        public int DocLine = -1;        //Line in the doc where this node starts
        public int DocIndexEnd = -1;    //Index in the doc where this node ends

        //Access
        public TydNode Parent { get; set; }

        public string Name
            {
            get { return _name; }
            }

        public int LineNumber
            {
            get { return DocLine; }
            }

        public string FullTyd
            {
            get { return TydToText.Write(this); }
            }

        //Construction
        public TydNode(string name, TydNode parent = null, int docLine = -1)
            {
            Parent = parent;
            _name = name;
            DocLine = docLine;
            }

        public abstract TydNode DeepClone();
        }
    }