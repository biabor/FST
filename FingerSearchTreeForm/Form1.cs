using FingerSearchTree;
using System.Diagnostics;

namespace FingerSearchTreeForm
{
    public partial class Form1 : Form
    {
        private Leaf lastLeaf;
        List<int> elements = new List<int>();

        public Form1()
        {
            InitializeComponent();
            lastLeaf = Tree.CreateTree();
        }

        private void search__Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                int value = int.Parse(inputOutput_.Text);
                lastLeaf = Tree.Search(lastLeaf, value);
                if (lastLeaf.Value == value)
                    inputOutput_.Text = "Found";
                else if (lastLeaf.Value < value && (lastLeaf.Right != null || (lastLeaf.Right as Leaf).Value > value))
                    inputOutput_.Text = "Not Found";
                else
                    inputOutput_.Text = "Wrong";
            }
            catch (Exception ex)
            {
                inputOutput_.Text = ex.Message;
            }

            Cursor = Cursors.Default;
        }

        private void add__Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                int value = int.Parse(inputOutput_.Text);
                lastLeaf = Tree.Search(lastLeaf, value);
                if (lastLeaf.Value < value && (lastLeaf.Right == null || (lastLeaf.Right as Leaf).Value > value))
                {
                    lastLeaf = Tree.Insert(lastLeaf, value);
                    if (lastLeaf.Value == value)
                        inputOutput_.Text = "Ok";
                    else
                        inputOutput_.Text = "Not Ok";
                }
                else
                    inputOutput_.Text = "Wrong";
            }
            catch (Exception ex)
            {
                inputOutput_.Text = ex.Message;
            }
            Cursor = Cursors.Default;
        }

        private void delete__Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                int value = int.Parse(inputOutput_.Text);
                lastLeaf = Tree.Search(lastLeaf, value);
                if (lastLeaf.Value == value)
                {
                    lastLeaf = Tree.Delete(lastLeaf);
                    if (lastLeaf.Value < value && (lastLeaf.Right == null || (lastLeaf.Right as Leaf).Value > value))
                        inputOutput_.Text = "Ok";
                    else
                        inputOutput_.Text = "Not Ok";
                }
                else
                    inputOutput_.Text = "Wrong";
            }
            catch (Exception ex)
            {
                inputOutput_.Text = ex.Message;
            }
            Cursor = Cursors.Default;
        }

        private void insert__Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            //try
            //{
            var searchTimmer = new Stopwatch();
            var insertTimmer = new Stopwatch();
            int howMany = int.Parse(inputOutput_.Text);

            Random rnd = new Random();
            int value = int.MaxValue;

            while (elements.Count < howMany)
            {
                searchTimmer.Start();
                lastLeaf = Tree.Search(lastLeaf, value);
                searchTimmer.Stop();
                if (lastLeaf.Value < value && (lastLeaf.Right == null || (lastLeaf.Right as Leaf).Value > value))
                {
                    insertTimmer.Start();
                    lastLeaf = Tree.Insert(lastLeaf, value);
                    insertTimmer.Stop();
                    elements.Add(value);
                }
                value = rnd.Next();
                if (test_.Checked && only_.Checked == false && Test() == false)
                {
                    inputOutput_.Text = "Something went wrong: Either a left/right pointer or there are too few/too many elements";
                    return;
                }
            }

            if (test_.Checked && only_.Checked && Test() == false)
            {
                inputOutput_.Text = "Something went wrong: Either a left/right pointer or there are too few/too many elements";
            }
            //}
            //catch (Exception ex)
            //{
            //    inputOutput_.Text = ex.Message;
            //}
            Cursor = Cursors.Default;
        }

        private void remove__Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                var searchTimmer = new Stopwatch();
                var insertTimmer = new Stopwatch();
                var deleteTimmer = new Stopwatch();

                int howMany = int.Parse(inputOutput_.Text);

                Random rnd = new Random();
                int value = rnd.Next();

                while (elements.Count < howMany)
                {
                    searchTimmer.Start();
                    lastLeaf = Tree.Search(lastLeaf, value);
                    searchTimmer.Stop();
                    if (lastLeaf.Value < value && (lastLeaf.Right == null || (lastLeaf.Right as Leaf).Value > value))
                    {
                        insertTimmer.Start();
                        lastLeaf = Tree.Insert(lastLeaf, value);
                        insertTimmer.Stop();
                        elements.Add(value);
                    }
                    value = rnd.Next();
                    if (test_.Checked && only_.Checked == false && Test() == false)
                    {
                        inputOutput_.Text = "Something went wrong: Either a left/right pointer or there are too few/too many elements";
                        return;
                    }
                }

                if (test_.Checked && only_.Checked && Test() == false)
                {
                    inputOutput_.Text = "Something went wrong: Either a left/right pointer or there are too few/too many elements";
                }

                while (elements.Count != 0)
                {
                    int index = rnd.Next(elements.Count);
                    value = elements[index];
                    searchTimmer.Start();
                    lastLeaf = Tree.Search(lastLeaf, value);
                    searchTimmer.Stop();
                    if (lastLeaf.Value == int.MinValue)
                        lastLeaf = lastLeaf.Right as Leaf;
                    deleteTimmer.Start();
                    lastLeaf = Tree.Delete(lastLeaf);
                    deleteTimmer.Stop();
                    elements.RemoveAt(index);
                    if (test_.Checked && only_.Checked == false && Test() == false)
                    {
                        inputOutput_.Text = "Something went wrong: Either a left/right pointer or there are too few/too many elements";
                        return;
                    }
                }
                if (test_.Checked && only_.Checked && Test() == false)
                {
                    inputOutput_.Text = "Something went wrong: Either a left/right pointer or there are too few/too many elements";
                }
            }
            catch (Exception ex)
            {
                inputOutput_.Text = ex.Message;
            }
            Cursor = Cursors.Default;
        }

        private void random__Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            //try
            //{
            Random rnd = new Random();
            int howMany = int.Parse(inputOutput_.Text);

            for (int i = 0; i < howMany; i++)
            {
                bool insert = false;
                if (elements.Count == 0)
                    insert = true;
                else
                    insert = rnd.Next(2) == 1;

                if (insert)
                {
                    int value = rnd.Next();
                    lastLeaf = Tree.Search(lastLeaf, value);
                    if (lastLeaf.Value < value && (lastLeaf.Right == null || (lastLeaf.Right as Leaf).Value > value))
                    {
                        lastLeaf = Tree.Insert(lastLeaf, value);
                        elements.Add(value);
                    }
                }
                else
                {
                    int index = rnd.Next(elements.Count);
                    int value = elements[index];
                    lastLeaf = Tree.Search(lastLeaf, value);
                    if (lastLeaf.Value == value)
                    {
                        lastLeaf = Tree.Delete(lastLeaf);
                    }
                }

                if (test_.Checked && only_.Checked == false && Test() == false)
                {
                    inputOutput_.Text = "Something went wrong: Either a left/right pointer or there are too few/too many elements";
                    return;
                }
            }

            if (test_.Checked && only_.Checked && Test() == false)
            {
                inputOutput_.Text = "Something went wrong: Either a left/right pointer or there are too few/too many elements";
            }
            //}
            //catch (Exception ex)
            //{
            //    inputOutput_.Text = ex.Message;
            //}
            Cursor = Cursors.Default;
        }

        private bool Test()
        {
            int count = 0;
            bool ok = true;
            while (lastLeaf.Left != null)
                lastLeaf = lastLeaf.Left as Leaf;
            while (lastLeaf.Right != null)
            {
                if (lastLeaf.Min > lastLeaf.Right.Min)
                    ok = false;
                lastLeaf = lastLeaf.Right as Leaf;
                count++;
            }
            if (count != elements.Count)
                ok = false;
            count = 0;
            while (lastLeaf.Left != null)
            {
                if (lastLeaf.Min < lastLeaf.Min)
                    ok = false;
                lastLeaf = lastLeaf.Left as Leaf;
                count++;
            }
            if (count != elements.Count)
                ok = false;

            return ok;
        }

        private bool TestFind(int value)
        {
            while (lastLeaf.Left != null)
                lastLeaf = lastLeaf.Left as Leaf;
            while (lastLeaf.Right != null)
            {
                if (lastLeaf.Value == value)
                    return true;
                lastLeaf = lastLeaf.Right as Leaf;
            }
            return false;
        }
    }
}