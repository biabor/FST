namespace FingerSearchTree
{
    public class Block1
    {
        public Block1 Left { get; set; }

        public Block1 Right { get; set; }

        public Block1 Mate { get; set; }

        public Block2 Father { get; set; }

        public int Degree { get; set; } = 0;

        public bool IsFull { get => Bounds.Ai(Father.Node.Level) <= Degree; }

        public int Min { get => First == null ? int.MaxValue : First.Min; }

        public int Max { get => Last == null ? int.MinValue : Last.Max; }

        public Node First { get; set; }

        public Node Last { get; set; }

        public Block1(Block2 father)
        {
            Father = father;
        }

        internal bool ContainsValue(int value)
        {
            return Min <= value && value <= Max;
        }

        internal Node FindChildContaining(int value)
        {
            if (value > Max)
                return Last;

            Node node = First;
            while (node?.Father == this)
            {
                if (node.ContainsValue(value))
                    return node;

                if (node.Min > value)
                    return node.Left;
                node = node.Right;
            }

            return Last;
        }

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
