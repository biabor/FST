namespace FingerSearchTree
{
    public class Block2
    {
        public Block2 Left { get; set; }

        public Block2 Right { get; set; }

        public Block2 Mate { get; set; }

        public Node Node { get; set; }

        public Block1 First { get; set; }

        public Block1 Last { get; set; }

        public int Degree { get; set; } = 0;

        public bool IsFull { get => Node.Group.IsSplitGroup ? Degree >= Bounds.BiP(Node.Level) : Degree >= Bounds.Fi(Node.Level); }

        public bool Pending { get; set; }

        public int Min { get => First == null ? int.MaxValue : First.Min; }

        public int Max { get => Last == null ? int.MinValue : Last.Max; }

        public Block2(Node node)
        {
            Node = node;
        }

        internal bool ContainsValue(int value)
        {
            return Min <= value && value <= Max;
        }

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
                        Mate.Add(to, to.Mate, to.Right);
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
                        Mate.Add(to, to.Mate, to.Right);
                    }
                    to.TransferToMate();
                }

                if (to.IsFull && to.Mate != null && to.Mate.IsFull)
                {
                    to.Mate.Mate = null;
                    to.Mate = null;
                }
            }
        }

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
        }

        internal bool IsBreakPossible()
        {
            if (Mate == Right)
                return Last.Last.Group != Last.Last.Right.Group;
            else
                return First.First.Group != First.First.Left.Group;
        }
    }
}
