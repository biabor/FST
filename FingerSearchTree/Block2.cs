namespace FingerSearchTree
{
    public class Block2
    {
        /// <summary>
        /// Pointer to the left block2.
        /// </summary>
        public Block2 Left { get; set; }

        /// <summary>
        /// Pointer to the right block2.
        /// </summary>
        public Block2 Right { get; set; }

        /// <summary>
        /// Pointer to the mate block2.
        /// </summary>
        public Block2 Mate { get; set; }

        /// <summary>
        /// Pointer to the node which contains this block2.
        /// </summary>
        public Node Node { get; set; }

        /// <summary>
        /// Pointer to the first child-block2.
        /// </summary>
        public Block1 First { get; set; }

        /// <summary>
        /// Pointer to the last child-block2
        /// </summary>
        public Block1 Last { get; set; }

        /// <summary>
        /// The degree of this block2.
        /// </summary>
        public int Degree { get; set; } = 0;

        /// <summary>
        /// Says if the block2 is full or not.
        /// </summary>
        public bool IsFull { get => Node.Group.IsSplitGroup ? Degree >= Bounds.BiP(Node.Level) : Degree >= Bounds.Fi(Node.Level); }

        /// <summary>
        /// Indicates whether this block2 is pending.
        /// </summary>
        public bool Pending { get; set; }

        /// <summary>
        /// The smallest value in this block2.
        /// </summary>
        public int Min { get => First == null ? int.MaxValue : First.Min; }

        /// <summary>
        /// The largest value in this block2.
        /// </summary>
        public int Max { get => Last == null ? int.MinValue : Last.Max; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="node">The father of this block2.</param>
        public Block2(Node node)
        {
            Node = node;
        }

        /// <summary>
        /// Indicates if this block2 could contain the value.
        /// </summary>
        /// <param name="value">The value we are searching for.</param>
        /// <returns>true, if it could contain the value, false otherwise.</returns>
        internal bool ContainsValue(int value)
        {
            return Min <= value && value <= Max;
        }

        /// <summary>
        /// Searches for the child-node in this block2 that could contain the value.
        /// </summary>
        /// <param name="value">The value we are searching for.</param>
        /// <returns>The node that could contain the value, or the largest one with a smaller value.</returns>
        internal Node FindChildContaining(int value)
        {
            if (value > Max)
                return Last.FindChildContaining(value);


            Block1 block1 = First;
            while (block1?.Father == this)
            {
                if (block1.ContainsValue(value))
                    return block1.FindChildContaining(value);

                if (block1.Min > value)
                    return block1.Left.FindChildContaining(value);
                block1 = block1.Right;
            }

            return Last.FindChildContaining(value);
        }

        /// <summary>
        /// Adds block1 middle in this block2, immediately to the right of leftP or immedialty to the left of rightP.
        /// </summary>
        /// <param name="leftP">The block1 that should be to the left.</param>
        /// <param name="middle">The block1 that we are inserting.</param>
        /// <param name="rightP">The block1 that should be to the right.</param>
        internal void Add(Block1 leftP, Block1 middle, Block1 rightP)
        {
            middle.Father = this;

            if (leftP == Last)
                Last = middle;
            if (rightP == First)
                First = middle;

            Block1 left = null;
            Block1 right = null;

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
            Node.Degree += middle.Degree;
            Node.Group.Degree += middle.Degree;
        }

        /// <summary>
        /// Removes the block1 middle from the blocks contained in this block2.
        /// </summary>
        /// <param name="middle">The block1 that is going to be removed.</param>
        internal void Remove(Block1 middle)
        {
            if (middle == Last)
                Last = middle.Left;

            if (middle == First)
                First = middle.Right;

            Block1 left = middle.Left;
            Block1 right = middle.Right;

            if (left != null)
                left.Right = right;
            if (right != null)
                right.Left = left;

            Degree -= middle.Degree;
            Node.Degree -= middle.Degree;
            Node.Group.Degree -= middle.Degree;

            if (First == null || Last == null)
            {
                Node.Remove(this);
                if (Pending == false && Mate != null)
                    Mate.Mate = null;
            }
        }

        /// <summary>
        /// Transfers a child node from this block2 to the mate.
        /// </summary>
        internal void TransferToMate()
        {
            if (Mate.First == null || Mate.Last == null)
                Mate.Add(null, new Block1(Mate), null);

            if (Mate == Right)
            {
                Block1 from = Last;
                Block1 to = Mate.First;
                Node transferred = from.Last;

                bool wasFromFull = from.IsFull;
                bool wasToFull = to.IsFull;

                from.Remove(transferred);

                if (wasFromFull)
                    if (from.Mate != null)
                        from.Mate.TransferToMate();
                    else if (from.Left != null)
                        if (from.Left.Mate == null)
                        {
                            from.Mate = from.Left;
                            from.Left.Mate = from;
                            from.Mate.TransferToMate();
                        }
                        else
                        {
                            Node shared = from.Left.Last;
                            bool wasLeftFull = from.Left.IsFull;
                            from.Left.Remove(shared);
                            from.Add(from.First?.Left, shared, from.First);
                            if (wasLeftFull)
                                from.Left.Mate.TransferToMate();
                        }

                to.Add(to.First?.Left, transferred, to.First);

                if (wasToFull)
                {
                    if (to.Mate == null)
                    {
                        to.Mate = new Block1(to.Father) { Mate = to };
                        to.Father.Add(to, to.Mate, to.Right);
                    }
                    to.TransferToMate();
                }

                if (to.IsFull && to.Mate != null && to.Mate.IsFull)
                {
                    to.Mate.Mate = null;
                    to.Mate = null;
                }
            }
            else
            {
                Block1 from = First;
                Block1 to = Mate.Last;
                Node transferred = from.First;

                bool wasFromFull = from.IsFull;
                bool wasToFull = to.IsFull;

                from.Remove(transferred);

                if (wasFromFull)
                    if (from.Mate != null)
                        from.Mate.TransferToMate();
                    else if (from.Right != null)
                        if (from.Right.Mate == null)
                        {
                            from.Mate = from.Right;
                            from.Right.Mate = from;
                            from.Mate.TransferToMate();
                        }
                        else
                        {
                            Node shared = from.Right.First;
                            bool wasRightFull = from.Right.IsFull;
                            from.Right.Remove(shared);
                            from.Add(from.Last, shared, from.Last?.Right);
                            if (wasRightFull)
                                from.Right.Mate.TransferToMate();
                        }

                to.Add(to.Last, transferred, to.Last?.Right);

                if (wasToFull)
                {
                    if (to.Mate == null)
                    {
                        to.Mate = new Block1(to.Father) { Mate = to };
                        to.Father.Add(to, to.Mate, to.Right);
                    }
                    to.TransferToMate();
                }

                if (to.IsFull && to.Mate != null && to.Mate.IsFull)
                {
                    to.Mate.Mate = null;
                    to.Mate = null;
                }
            }

            if (Degree == 0 || First == null || Last == null)
            {
                if (Mate != null)
                    Mate.Mate = null;
                Mate = null;
            }
        }

        /// <summary>
        /// Transfers a child-block1 from this block2 to the to-block2.
        /// </summary>
        /// <param name="to">The block2 we are transferring to.</param>
        internal void Transfer(Block2 to)
        {
            if (Right == to)
            {
                Block1 transferred = Last;
                Remove(transferred);
                to.Add(to.First?.Left, transferred, to.First);

                if (transferred.Mate != null)
                {
                    transferred = transferred.Mate;
                    Remove(transferred);
                    to.Add(to.First?.Left, transferred, to.First);
                }
            }
            else
            {
                Block1 transferred = First;
                Remove(transferred);
                to.Add(to.Last, transferred, to.Last?.Right);

                if (transferred.Mate != null)
                {
                    transferred = transferred.Mate;
                    Remove(transferred);
                    to.Add(to.Last, transferred, to.Last?.Right);
                }
            }

            if (Degree == 0 || First == null || Last == null)
            {
                if (Mate != null)
                    Mate.Mate = null;
                Mate = null;
            }
        }

        /// <summary>
        /// Indicates whether ir will be possible to break the block2 pair while keeping the invariant that all the nodes in a group belong to the same block1 mentaind.
        /// </summary>
        /// <returns>True, if it is possible to break the pair, false otherwise.</returns>
        internal bool IsBreakPossible()
        {
            if (Mate == Right)
                return Last.Last.Group != Last.Last.Right.Group;
            else
                return First.First.Group != First.First.Left.Group;
        }
    }
}
