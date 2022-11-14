namespace FingerSearchTree
{
    public class Leaf : Node
    {
        /// <summary>
        /// The value of the leaf.
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// The minimal value of the leaf.
        /// </summary>
        public override int Min { get => Value; }

        /// <summary>
        /// The maximal value of the leaf.
        /// </summary>
        public override int Max { get => Value; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="father">The father block1 of this leaf.</param>
        /// <param name="value">The value of this leaf.</param>
        public Leaf(Block1 father, int value) : base(0)
        {
            Father = father;
            Value = value;
        }
    }
}
