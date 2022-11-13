namespace FingerSearchTree
{
    public class Component
    {
        public bool Valid { get; set; }

        public Node Root { get; }

        public Component(Node root)
        {
            Root = root;
            Valid = true;
        }
    }
}
