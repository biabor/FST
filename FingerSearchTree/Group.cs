namespace FingerSearchTree
{
    public class Group
    {
        /// <summary>
        /// The group that is positioned to the left.
        /// </summary>
        public Group Left { get => First?.Left?.Group; }

        /// <summary>
        /// The group that is positioned to the right.
        /// </summary>
        public Group Right { get => Last?.Right?.Group; }

        /// <summary>
        /// The first node in this group.
        /// </summary>
        public Node First { get; set; }

        /// <summary>
        /// The last node in this group.
        /// </summary>
        public Node Last { get; set; }

        /// <summary>
        /// The block2 the nodes of this group are containd in.
        /// </summary>
        public Block2 Block2 { get => First?.Father?.Father; }

        /// <summary>
        /// The component the nodes in this group are contained in.
        /// </summary>
        public Component Component { get; set; }

        /// <summary>
        /// Indicates if the group is a split group or a fusion group.
        /// </summary>
        public bool IsSplitGroup { get => Degree > 4 * Bounds.Fi(Level); }

        /// <summary>
        /// The degree of the nodes in this group.
        /// </summary>
        public int Degree { get; set; }

        /// <summary>
        /// Indicates if this group contains only one node with only one block2 with only one block1.
        /// </summary>
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

        /// <summary>
        /// The height this group is on.
        /// </summary>
        public int Level { get => First == null ? -1 : First.Level; }

        /// <summary>
        /// Indicates if this group is valid or not.
        /// </summary>
        public bool Valid { get; internal set; }

        /// <summary>
        /// Default constuctor.
        /// </summary>
        /// <param name="node">The initial node of this group</param>
        public Group(Node node)
        {
            First = node;
            Last = node;
            Degree = node.Degree;
            Component = new Component(node);
            Valid = true;
        }

        /// <summary>
        /// Constructor if the group was invalid but the component was.
        /// </summary>
        /// <param name="node">The initial node of this group.</param>
        /// <param name="component">The component this group should be contiand in.</param>
        public Group(Node node, Component component) : this(node)
        {
            if (component.Valid)
                Component = component;
        }

        /// <summary>
        /// Breaks the group and the component this goup is root of and adds the nodes of this group in the component of their father.
        /// </summary>
        internal void MultiBreak()
        {
            Component.Valid = false;
            Valid = false;
            if (Block2 != null)
                Component = Block2.Node.Component;
            else if (First != null)
                Component = new Component(First);
        }

        /// <summary>
        /// Fuses the node from this group with the node from group g.
        /// It is assumed that both groups can be fused together.
        /// </summary>
        /// <param name="g">The group to be fused with.</param>
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

        /// <summary>
        /// Shares the blocks2 of the node in this group with the node in group g.
        /// It is assumed that both groups contain only one node and that one of them contains only one block1,
        /// </summary>
        /// <param name="g"></param>
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

        /// <summary>
        /// Indicates whether this group can be fused with group g.
        /// </summary>
        /// <param name="g">The group we are trying to fuse with.</param>
        /// <returns>true if the two groups can be fused, false otherwise.</returns>
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

        /// <summary>
        /// Adds the node middle in this group, imediately to the right on left.
        /// </summary>
        /// <param name="left">The node right to which the new node is inserted.</param>
        /// <param name="middle">The node to be inserted in this group.</param>
        internal void Add(Node left, Node middle)
        {
            if (left == Last)
                Last = middle;

            middle.Group = this;
            Degree += middle.Degree;
        }

        /// <summary>
        /// Removes this node from the group.
        /// </summary>
        /// <param name="node">The node to be removed.</param>
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
