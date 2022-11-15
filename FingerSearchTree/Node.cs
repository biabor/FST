namespace FingerSearchTree
{
    public class Node
    {
        /// <summary>
        /// Pointer to the left adjacent node.
        /// </summary>
        public Node Left { get; set; }

        /// <summary>
        /// Pointer to the right adjacent node.
        /// </summary>
        public Node Right { get; set; }

        /// <summary>
        /// Pointer to the father block1.
        /// </summary>
        public Block1 Father { get; set; }

        /// <summary>
        /// Pointer to the fathernode.
        /// </summary>
        public Node FatherNode { get => Father?.Father.Node; }

        /// <summary>
        /// Pointer to the first child block2.
        /// </summary>
        public Block2 First { get; set; }

        /// <summary>
        /// Pointer to the last child block2.
        /// </summary>
        public Block2 Last { get; set; }

        /// <summary>
        /// Indicates how many blocks2 this node contains.
        /// </summary>
        public int Blocks2Count { get; set; }

        /// <summary>
        /// The group this node is contained in. 
        /// It always points to a valid group in a valid component.
        /// </summary>
        private Group group_;
        public Group Group
        {
            get
            {
                if (group_.Valid && group_.Component.Valid == false)
                {
                    group_.Component = new Component(this);
                }

                if (group_.Valid == false && group_.Component.Valid == false)
                {
                    group_.Remove(this);
                    group_ = new Group(this);
                }

                if (group_.Valid == false && group_.Component.Valid)
                {
                    group_.Remove(this);
                    group_ = new Group(this, group_.Component);
                }

                return group_;
            }
            set => group_ = value;
        }

        /// <summary>
        /// Component this group is contained in. 
        /// It always points to a valid component.
        /// </summary>
        public Component Component { get => Group.Component; }

        /// <summary>
        /// The height of this node.
        /// </summary>
        public int Level { get; internal set; }

        /// <summary>
        /// The number of child nodes this node contains.
        /// </summary>
        public int Degree { get; set; } = 0;

        /// <summary>
        /// The smallest value in the subtree rooted at this node.
        /// </summary>
        public virtual int Min { get => First == null ? int.MaxValue : First.Min; }

        /// <summary>
        /// The largest value in the subtree rooted at this node.
        /// </summary>
        public virtual int Max { get => Last == null ? int.MinValue : Last.Max; }

        /// <summary>
        /// Indicates if this node contains two or more block2 pairs.
        /// </summary>
        public bool ContainsAtLeastTwoBlock2Pairs
        {
            get
            {
                switch (Blocks2Count)
                {
                    case 0: return false;
                    case 1: return false;
                    case 2: return First.Mate != Last && First != Last.Mate;
                    case 3:
                        if (First.Right.Pending) return true;
                        return First.Pending == false && Last.Pending == false;
                    case 4: return First.Pending && First.Right.Pending == false && Last.Left.Pending == false && Last.Pending;
                    default: return true;
                }
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="level">The level of this node.</param>
        public Node(int level)
        {
            group_ = new Group(this);
            Level = level;
        }

        /// <summary>
        /// Indicates if the node could contain the given value.
        /// </summary>
        /// <param name="value">The searched value.</param>
        /// <returns>true, if the value could be in the subtree rooted at this node, false otherwise.</returns>
        internal bool ContainsValue(int value)
        {
            if (Father == null)
                return true;
            return Min <= value && value <= Max;
        }

        /// <summary>
        /// Searches for the child node that contains the parameter value.
        /// </summary>
        /// <param name="value">The searched value.</param>
        /// <returns>The node that contains the value or if it does not exists then the one with the largest value smaller than the one searched.</returns>
        internal Node FindChildContaining(int value)
        {
            if (value > Max)
                return Last.FindChildContaining(value);

            Block2 block2 = First;
            while (block2?.Node == this)
            {
                if (block2.ContainsValue(value))
                    return block2.FindChildContaining(value);

                if (block2.Min > value)
                    return block2.Left.FindChildContaining(value);
                block2 = block2.Right;
            }
            return Last.FindChildContaining(value);
        }

        /// <summary>
        /// Adds the block2 middle in this node, immediately to the right of leftP or imediately to the left of rightP.
        /// </summary>
        /// <param name="leftP">The block2 that is to the left.</param>
        /// <param name="middle">The block2 to be inserted.</param>
        /// <param name="rightP">The block2 that is to the right.</param>
        internal void Add(Block2 leftP, Block2 middle, Block2 rightP)
        {
            middle.Node = this;

            if (leftP == Last)
                Last = middle;
            if (rightP == First)
                First = middle;

            Block2 left = null;
            Block2 right = null;

            if (leftP != null)
                left = leftP;

            if (left != null)
                right = left.Right;
            else if (rightP != null)
                right = rightP;

            if (left != null)
                left.Right = middle;
            middle.Left = left;
            middle.Right = right;
            if (right != null)
                right.Left = middle;

            Degree += middle.Degree;
            Group.Degree += middle.Degree;
            Blocks2Count += 1;
        }

        /// <summary>
        /// Removes the block2 from this node.
        /// </summary>
        /// <param name="middle">The block2 to be removed.</param>
        internal void Remove(Block2 middle)
        {
            if (middle == Last)
                Last = middle.Left;

            if (middle == First)
                First = middle.Right;

            Block2 left = middle.Left;
            Block2 right = middle.Right;

            if (left != null)
                left.Right = right;
            if (right != null)
                right.Left = left;

            Degree -= middle.Degree;
            Group.Degree -= middle.Degree;
            Blocks2Count -= 1;

            if(First==null||Last== null)
            {
                Tree.DeleteNode(this);
            }
        }

        /// <summary>
        /// Splits this node into two, by moving the rightmost block2 pair in the new node.
        /// </summary>
        /// <returns>The new node, that contains the rightmost block2 pair of this one.</returns>
        internal Node Split()
        {
            Node newNode = new Node(Level);

            if (Father == null)
            {
                Node root = new Node(Level + 1);
                Block2 block2 = new Block2(root);
                Block1 block1 = new Block1(block2);

                root.Add(null, block2, null);
                block2.Add(null, block1, null);
                block1.Add(null, this, null);
            }

            Block2 lastBlock2 = Last;
            if (lastBlock2.Pending)
            {
                Remove(lastBlock2);
                newNode.Add(newNode.First?.Left, lastBlock2, newNode.First);
                lastBlock2 = Last;
            }

            Remove(lastBlock2);
            newNode.Add(newNode.First?.Left, lastBlock2, newNode.First);
            lastBlock2 = Last;

            if (lastBlock2.Pending == false && lastBlock2.Mate == newNode.First && lastBlock2.Mate?.Mate == lastBlock2)
            {
                Remove(lastBlock2);
                newNode.Add(newNode.First?.Left, lastBlock2, newNode.First);
                lastBlock2 = Last;
            }

            if (lastBlock2.Pending)
            {
                Remove(lastBlock2);
                newNode.Add(newNode.First?.Left, lastBlock2, newNode.First);
            }

            Group.Add(this, newNode);

            return newNode;
        }
    }
}
