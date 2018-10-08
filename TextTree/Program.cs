using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
namespace TextTree
{
    class Program
    {
        public static List<string> Text = new List<string>();
        public static List<Tree> Nodes = new List<Tree>();
        bool IsReady = false;
        static void Main(string[] args)
        {
            LoadText(@"C:\workspace\people.txt", Text);
            SortTree(Text, Nodes);
            ReadNodes(Nodes);
            Console.ReadKey();
        }

        public static void LoadText(string path, List<string> text)
        {
            string line;
            
            using (StreamReader sr = new StreamReader(path))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    text.Add(line);
                }

            }
        }

        public static void SortTree(List<string> text, List<Tree> nodes)
        {
            for (int i=0; i < text.Count; i++)
            {
                nodes.Add(new Tree(text[i].Replace("\t","")));
                nodes[i].Depth = NumberOfOcc(text[i], "\t");
                if (NumberOfOcc(text[i], "\t")==0)
                {
                    
                    continue;
                }
                else if ((NumberOfOcc(text[i], "\t"))> (NumberOfOcc(text[i-1], "\t")))
                {
                    nodes[i].Parent=nodes[i-1];
                    nodes[i - 1].Children.Add(nodes[i]);
                }
                else if ((NumberOfOcc(text[i], "\t")) == (NumberOfOcc(text[i - 1], "\t")))
                {
                    nodes[i].Parent = nodes[i - 1].Parent;
                    nodes[i - 1].Parent.Children.Add(nodes[i]);
                }
            }
        }

        public static int NumberOfOcc(string text, string pattern)
        {
            int count = 0;
            int i = 0;
            while((i=text.IndexOf(pattern, i))!= -1)
            {
                i += pattern.Length;
                count++;
            }
            return count;
        }

        public static void ReadNodes(List<Tree> nodes)
        {
            //one way
            string tabs;
            for (int i=0; i<nodes.Count; i++)
            {
                tabs = new string('\t', nodes[i].Depth);
                Console.WriteLine(tabs+nodes[i].Name);
            }
        }
    }

    class Tree
    {
        public string Name;
        public Tree Parent;
        public List<Tree> Children = new List<Tree>();
        public int Depth;

        public Tree(string n)
        {
            n = Name;
        }
    }
    
}
