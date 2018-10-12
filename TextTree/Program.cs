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
        public static bool IsReady = false;
        static void Main(string[] args)
        {
            LoadText(@"C:\workspace\treeTestData\people.txt", Text);
            if(IsReady==true)
            {
                //SortTree(Text, Nodes);
                SortTreeOther(Text, Nodes);
                ReadNodes(Nodes);
                //GetNode("");
                //GetNodes("");
                //GetLeaves();
                //GetInternalNodes();
                //WriteOutlineFile(Nodes);
            }

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
            if(Text != null)
            {
                IsReady = true;
            }
        }

        public static void SortTree(List<string> text, List<Tree> nodes)
        {
            string tempName;
            for (int i=0; i < text.Count; i++)
            {
                tempName = text[i].Trim();

                nodes.Add(new Tree(tempName));
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
                    if (nodes[i - 1].Parent == null)
                    {
                        continue;
                    }
                    nodes[i - 1].Parent.Children.Add(nodes[i]);
                }
                else if((NumberOfOcc(text[i], "\t")) < (NumberOfOcc(text[i - 1], "\t")))
                {
                    int d = NumberOfOcc(text[i], "\t");
                    int j = nodes.Count-1;
                    while(nodes[j].Depth <= d)
                    {
                        j--;
                    }
                    nodes[i].Parent = nodes[j];
                    nodes[j].Children.Add(nodes[i]);

                }
            }
        }

        public static void SortTreeOther(List<string> text, List<Tree> nodes)
        {
            for (int i = 0; i < text.Count; i++)
            {

                if (NumberOfOcc(text[i], "\t") == 0)
                {
                    nodes.Add(AddNode(null, text[i]));
                }
                else if ((NumberOfOcc(text[i], "\t")) > (NumberOfOcc(text[i - 1], "\t")))
                {
                    nodes.Add(AddNode(nodes[nodes.Count-1], text[i]));
                    

                }
                else if ((NumberOfOcc(text[i], "\t")) == (NumberOfOcc(text[i - 1], "\t")))
                {
                    nodes.Add(AddNode(nodes[nodes.Count - 1].Parent, text[i]));
                }
                else if((NumberOfOcc(text[i], "\t")) < (NumberOfOcc(text[i - 1], "\t")))
                {
                    int d = NumberOfOcc(text[i], "\t");
                    int j = nodes.Count - 1;
                    while (nodes[j].Depth != d)
                    {
                        j--;
                    }
                    nodes.Add(AddNode(nodes[j].Parent, text[i]));

                }
                
            }
        }

        public static Tree AddNode(Tree parent, string name)
        {
            Tree tree=new Tree(name.Trim());
            if(parent != null)
            {
                tree.Parent = parent;
                tree.Depth = parent.Depth + 1;
                parent.Children.Add(tree);
            }
            else
            {
                tree.Depth = 0;
            }          
            return tree;
        }

        public static void RemoveNode(Tree node)
        {
            node.Parent = null;
            node.Children = null;
            
        }

        public static Tree GetNode(string name)
        {
            for (int i=0; i<Nodes.Count; i++)
            {
                if (Nodes[i].Name==name)
                {
                    //Console.WriteLine(Nodes[i].Name);
                    return Nodes[i];
                    
                }

            }
            return null; 
        }

        public static List<Tree> GetNodes(string name)
        {
            List<Tree> GotNodes=new List<Tree>();
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i].Name == name)
                {
                    GotNodes.Add(Nodes[i]);

                }

            }
            for (int j = 0; j < GotNodes.Count; j++)
            {
                    Console.WriteLine(GotNodes[j].Name);

            }
            return GotNodes;
        }

        public static List<Tree> GetLeaves()
        {
            List<Tree> GotNodes = new List<Tree>();
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (!Nodes[i].Children.Any())
                {
                    GotNodes.Add(Nodes[i]);

                }

            }
            for (int j = 0; j < GotNodes.Count; j++)
            {

                Console.WriteLine(GotNodes[j].Name);

            }
            return GotNodes;
        }

        public static List<Tree> GetInternalNodes()
        {
            List<Tree> GotNodes = new List<Tree>();
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i].Parent == null && !Nodes[i].Children.Any())
                {
                    GotNodes.Add(Nodes[i]);

                }

            }
            for (int j = 0; j < GotNodes.Count; j++)
            {
                
                    Console.WriteLine(GotNodes[j].Name);

            }
            return GotNodes;
        }

        public static void WriteOutlineFile (List<Tree> nodes)
        {
            using (StreamWriter sw = new StreamWriter(@"C:\workspace\WriteHierachy.txt"))
            {
                //basic way
                /*string tabs;
                for (int i = 0; i < nodes.Count; i++)
                {
                    tabs = new string('\t', nodes[i].Depth);
                    sw.WriteLine(tabs + nodes[i].Name);
                }*/

                //hierarichal way
                List<Tree> Roots = new List<Tree>();
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i].Depth == 0)
                    {
                        Roots.Add(nodes[i]);
                    }
                }
                foreach (Tree t in Roots)
                {
                    WriteChildren(t, sw);
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
            //basic way
            /*string tabs;
            for (int i=0; i<nodes.Count; i++)
            {
                tabs = new string('\t', nodes[i].Depth);
                Console.WriteLine(tabs+nodes[i].Name);
            }*/

            //hierarchical way
            List<Tree> Roots = new List<Tree>();
            for (int i = 0; i < nodes.Count; i++)
            {
                if(nodes[i].Depth==0)
                {
                    Roots.Add(nodes[i]);
                }
            }
            foreach(Tree t in Roots)
            {
                ReadChildren(t);
            }
            
        }

        public static void ReadChildren(Tree node)
        {
            string tabs;
            tabs = new string('\t', node.Depth);
            Console.WriteLine(tabs + node.Name);
            if (node.Children != null)
            {
                foreach (Tree t in node.Children)
                {
                    ReadChildren(t);
                }
            }
        }

        public static void WriteChildren(Tree node, StreamWriter sw)
        {
            string tabs;
            tabs = new string('\t', node.Depth);
            sw.WriteLine(tabs + node.Name);
            if (node.Children != null)
            {
                foreach (Tree t in node.Children)
                {
                    WriteChildren(t, sw);
                }
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
            Name = n;
        }
    }
    
}
