namespace FingerSearchTree
{
    public class Node
    {
        public Node Left { get; set; }

        public Node Right { get; set; }

        public Block1 Father { get; set; }

        public Node FatherNode { get => Father?.Father.Node; }

        public Block2 First { get; set; }

        public Block2 Last { get; set; }

        public int Blocks2Count { get; set; }

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

        public Component Component { get => Group.Component; }

        public int Level { get; internal set; }

        public int Degree { get; set; } = 0;

        public virtual int Min { get => First == null ? int.MaxValue : First.Min; }

        public virtual int Max { get => Last == null ? int.MinValue : Last.Max; }

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

        public Node(int level)
        {
            group_ = new Group(this);
            Level = level;
        }

        internal bool ContainsValue(int value)
        {
            if (Father == null)
                return true;
            return Min <= value && value <= Max;
        }

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
        }

        internal Node Split()
        {
            Node newNode = new Node(Level);

            if (Father == null)
            {
                Node root = new Node(Level + 1);
                Block2 block2 = new Block2(root);
                Block1 block1 = new Block1(block2);

                block1.Add(null, this, null);
                block2.Add(null, block1, null);
                root.Add(null, block2, null);
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

            if (lastBlock2.Pending == false && lastBlock2.Mate != null && lastBlock2.Mate == newNode.First && lastBlock2.Mate.Mate == lastBlock2)
            {
                Remove(lastBlock2);
                newNode.Add(newNode.First?.Left, lastBlock2, newNode.First);
                lastBlock2 = Last;
            }

            if (lastBlock2.Pending && lastBlock2.Mate != null && lastBlock2.Mate == newNode.First)
            {
                Remove(lastBlock2);
                newNode.Add(newNode.First?.Left, lastBlock2, newNode.First);
            }

            Group.Add(this, newNode);

            return newNode;
        }
    }
}
