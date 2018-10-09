﻿using System;
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
            LoadText(@"C:\workspace\treeTestData\people.txt", Text, IsReady);
            if(IsReady==true)
            {
                SortTree(Text, Nodes);
                ReadNodes(Nodes);
            }
            
            Console.ReadKey();
        }

        public static void LoadText(string path, List<string> text, bool ready)
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
                ready = true;
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
