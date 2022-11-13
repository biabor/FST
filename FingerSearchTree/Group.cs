namespace FingerSearchTree
{
    public class Group
    {
        public Group Left { get => First?.Left?.Group; }

        public Group Right { get => Last?.Right?.Group; }

        public Node First { get; set; }

        public Node Last { get; set; }

        public Block2 Block2 { get => First?.Father?.Father; }

        public Component Component { get; set; }

        public bool IsSplitGroup { get => Degree > 4 * Bounds.Fi(Level); }

        public int Degree { get; set; }

        public bool HasOnlyOneBlock1
        {
            get
            {
                if (First == null || First != Last)
                    return false;
                if (First.Blocks2Count != 1)
                    return false;
                return First.First.First == First.First.Last;
            }
        }

        public int Level { get => First == null ? -1 : First.Level; }

        public bool Valid { get; internal set; }

        public Group(Node node)
        {
            First = node;
            Last = node;
            Degree = node.Degree;
            Component = new Component(node);
            Valid = true;
        }

        public Group(Node node, Component component) : this(node)
        {
            if (component.Valid)
                Component = component;
        }

        internal void MultiBreak()
        {
            Component.Valid = false;
            Valid = false;
            if (Block2 != null)
                Component = Block2.Node.Component;
            else if (First != null)
                Component = new Component(First);
        }

        internal void Fuse(Group g)
        {
            bool left = Left == g;
            if (HasOnlyOneBlock1)
            {
                Block2 to = g.First.First;
                Block1 what = First.First.First;

                Node fatherNode = First;
                what.Father.Remove(what);
                Tree.DeleteNode(fatherNode);

                Block1? whatNeighbour;
                if (left)
                {
                    to.Add(to.Last, what, to.Last?.Right);
                    whatNeighbour = what.Left;
                }
                else
                {
                    to.Add(to.First?.Left, what, to.First);
                    whatNeighbour = what.Right;
                }

                if (whatNeighbour != null && ((what.IsFull && whatNeighbour.IsFull == false && whatNeighbour.Mate == null) || (what.IsFull == false && whatNeighbour.IsFull && whatNeighbour.Mate == null)))
                {
                    what.Mate = whatNeighbour;
                    whatNeighbour.Mate = what;
                }

                if (to.Pending && to.IsFull)
                {
                    to.Pending = false;
                    to.Mate = null;
                };
            }
            else if (g.HasOnlyOneBlock1)
                g.Fuse(this);
            else if (First.Blocks2Count == 1)
            {
                Node to = g.First;
                Block2 what = First.First;

                First.Remove(what);
                Tree.DeleteNode(First);

                if (what.Mate != null && what.Pending == false)
                    what.Mate.Mate = null;
                what.Mate = null;

                Block2? whatNeighbour;
                if (left)
                {
                    to.Add(to.Last, what, to.Last?.Right);
                    whatNeighbour = what.Left;
                }
                else
                {
                    to.Add(to.First?.Left, what, to.First);
                    whatNeighbour = what.Right;
                }

                if (whatNeighbour != null)
                {
                    if ((what.IsFull && whatNeighbour.IsFull == false && whatNeighbour.Mate == null) || (what.IsFull == false && whatNeighbour.IsFull && whatNeighbour.Mate == null))
                    {
                        what.Mate = whatNeighbour;
                        whatNeighbour.Mate = what;
                    }
                    else if (whatNeighbour.Mate != null && whatNeighbour.Pending && what.IsFull == false)
                    {
                        what.Mate = whatNeighbour;
                        what.Pending = true;
                    }
                }
            }
            else if (g.First.Blocks2Count == 1)
                g.Fuse(this);
        }

        internal void Share(Group g)
        {
            if (HasOnlyOneBlock1)
            {
                if (Left == g)
                {
                    Node from = g.Last;
                    Node to = First;
                    Block2 removed = to.First;
                    Block2 transferred = from.Last;
                    Block1 added = removed.First;

                    from.Remove(transferred);
                    to.Add(to.First?.Left, transferred, to.First);

                    if (transferred.Mate != null && transferred.Mate.Mate == transferred)
                        transferred.Mate.Mate = null;
                    transferred.Mate = null;
                    transferred.Pending = false;

                    removed.Remove(added);
                    transferred.Add(transferred.Last, added, transferred.Last?.Right);
                }
                else
                {
                    Node from = g.First;
                    Node to = Last;
                    Block2 removed = to.Last;
                    Block2 transferred = from.First;
                    Block1 added = removed.Last;

                    from.Remove(transferred);
                    to.Add(to.Last, transferred, to.Last?.Right);

                    if (transferred.Mate != null && transferred.Mate.Mate == transferred)
                        transferred.Mate.Mate = null;
                    transferred.Mate = null;
                    transferred.Pending = false;

                    removed.Remove(added);
                    transferred.Add(transferred.First?.Left, added, transferred.First);
                }
            }
            else
                g.Share(this);
        }

        internal bool CanBeFused(Group g)
        {
            if (Degree + g.Degree > 4 * Bounds.Fi(Level))
                return false;

            if (IsSplitGroup || g.IsSplitGroup)
                return false;

            if ((First == null || First != Last) && (g.First == null || g.First != g.Last))
                return false;

            if (Block2.Node != g.Block2.Node) return false;

            return (First.Blocks2Count == 1 && g.First.Blocks2Count <= 3) || (g.First.Blocks2Count == 1 && First.Blocks2Count <= 3);
        }

        internal void Add(Node left, Node middle)
        {
            if (left == Last)
                Last = middle;

            middle.Group = this;
            Degree += middle.Degree;
        }

        internal void Remove(Node node)
        {
            if (Last != First)
            {
                if (node == Last)
                    Last = node.Left;
                else if (node == First)
                    First = node.Right;
            }
            else
            {
                Last = null;
                First = null;
            }
            Degree -= node.Degree;
        }
    }
}
