namespace FingerSearchTree
{
    public class Component
    {
        /// <summary>
        /// Indicates if the component is valid.
        /// </summary>
        public bool Valid { get; set; }

        /// <summary>
        /// The root of the component.
        /// </summary>
        public Node Root { get; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="root">The root of the component.</param>
        public Component(Node root)
        {
            Root = root;
            Valid = true;
        }
    }
}
