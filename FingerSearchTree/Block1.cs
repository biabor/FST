namespace FingerSearchTree
{
    public class Block1
    {
        /// <summary>
        /// Pointer to the left block1.
        /// </summary>
        public Block1 Left { get; set; }

        /// <summary>
        /// Pointer to the right block1.
        /// </summary>
        public Block1 Right { get; set; }

        /// <summary>
        /// Pointer to the mate block1.
        /// </summary>
        public Block1 Mate { get; set; }

        /// <summary>
        /// Pointer to the father block2, that contains this.
        /// </summary>
        public Block2 Father { get; set; }

        /// <summary>
        /// The degree of this block1.
        /// </summary>
        public int Degree { get; set; } = 0;

        /// <summary>
        /// Says is this block1 is full or not.
        /// </summary>
        public bool IsFull { get => Bounds.Ai(Father.Node.Level) <= Degree; }

        /// <summary>
        /// Minimal value from this block1.
        /// </summary>
        public int Min { get => First == null ? int.MaxValue : First.Min; }

        /// <summary>
        /// Maximal value from this block1.
        /// </summary>
        public int Max { get => Last == null ? int.MinValue : Last.Max; }

        /// <summary>
        /// Pointer to the first child node.
        /// </summary>
        public Node First { get; set; }

        /// <summary>
        /// Pointer to the last child node.
        /// </summary>
        public Node Last { get; set; }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="father">The block2 that contains this block1.</param>
        public Block1(Block2 father)
        {
            Father = father;
        }

        /// <summary>
        /// Says if this block1 could contain the value given as parameter.
        /// </summary>
        /// <param name="value">the searched value.</param>
        /// <returns>true, if it could contains the value, false otherwise.</returns>
        internal bool ContainsValue(int value)
        {
            return Min <= value && value <= Max;
        }

        /// <summary>
        /// Searches for the child node that could contain the value.
        /// If no one is found, then it return the one with the largest value smaller than the one given as parameter.
        /// </summary>
        /// <param name="value">The searched value.</param>
        /// <returns>The node that could contain the value or the one that would be to the left.</returns>
        internal Node FindChildContaining(int value)
        {
            //Checking for the special case, so that we do not search just to find the last one.
            if (value > Max)
                return Last;

            Node node = First;
            while (node?.Father == this)
            {
                //If the node is found, then the we return it.
                if (node.ContainsValue(value))
                    return node;

                //If we arrive to a larger node, then we did not find and return the last one smaller.
                if (node.Min > value)
                    return node.Left;
                node = node.Right;
            }

            return Last;
        }

        /// <summary>
        /// Adds a new node into this block1 immediately to the left of leftP or to the right of rightP.
        /// </summary>
        /// <param name="leftP">Node to the left.</param>
        /// <param name="middle">Node that is added.</param>
        /// <param name="rightP">Node to the right.</param>
        internal void Add(Node leftP, Node middle, Node rightP)
        {
            middle.Father = this;

            if (Last == leftP)
                Last = middle;

            if (First == rightP)
                First = middle;

            Node left = null;
            Node right = null;

            if (leftP != null)
                left = leftP;
            else if (Left != null)
                left = Left.Last;
            else if (Father.Left != null)
                left = Father.Left.Last.Last;
            else if (Father.Node.Left != null)
                left = Father.Node.Left.Last.Last.Last;

            if (left != null)
                right = left.Right;
            else if (rightP != null)
                right = rightP;
            else if (Right != null)
                right = Right.First;
            else if (Father.Right != null)
                right = Father.Right.First.First;
            else if (Father.Node.Right != null)
                right = Father.Node.Right.First.First.First;

            if (left != null)
                left.Right = middle;
            middle.Left = left;
            middle.Right = right;
            if (right != null)
                right.Left = middle;

            Degree++;
            Father.Degree++;
            Father.Node.Degree++;
            Father.Node.Group.Degree++;
        }

        /// <summary>
        /// Removes the node from this block1.
        /// </summary>
        /// <param name="middle">The node to be removed.</param>
        internal void Remove(Node middle)
        {
            if (middle == Last)
                if (middle.Left?.Father == this)
                    Last = middle.Left;
                else
                    Last = null;

            if (middle == First)
                if (middle.Right?.Father == this)
                    First = middle.Right;
                else
                    First = null;

            Node left = middle.Left;
            Node right = middle.Right;

            if (left != null)
                left.Right = right;
            if (right != null)
                right.Left = left;

            Degree--;
            Father.Degree--;
            Father.Node.Degree--;
            Father.Node.Group.Degree--;

            if (First == null || Last == null)
            {
                Father.Remove(this);
                if (Mate != null)
                    Mate.Mate = null;
            }
        }

        /// <summary>
        /// Transfers a child node from this block1 to the mate block1. 
        /// It is assumed that mate is not null.
        /// </summary>
        internal void TransferToMate()
        {
            if (Mate == Right)
            {
                Node transferredNode = Last;
                Remove(transferredNode);
                Mate.Add(Mate.First?.Left, transferredNode, Mate.First);
            }
            else
            {
                Node transferredNode = First;
                Remove(transferredNode);
                Mate.Add(Mate.Last, transferredNode, Mate.Last?.Right);
            }
        }
    }
}
