using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_3d_Assingment
{
    class TreeNode
    {
        int attrValue { get; set; }
        string attrName { get; set; }
        List<TreeNode> children { get; set; }
        int result { get; set; }   // the result attribute value (-1 if not leaf node)
        double gain { get; set; }

        //ctor default
        public TreeNode()
        {
            children = new List<TreeNode>();
        }

        //ctor
        public TreeNode(int attrValue, String attrName, int result, double gain)
        {
            this.attrValue = attrValue;
            this.attrName = attrName;
            this.result = result;
            this.gain = gain;
        }

        //prints in the console the tree (testing perposes)
        public void printTree(TreeNode tn, int level)
        {
            if (tn.result == -1)
            {
                foreach (TreeNode temp in tn.children) // for all branches 
                {
                    int i = 0;
                    while (i < level)
                    {
                        i++;
                        Console.Write("| ");
                    }
                    Console.Write(tn.attrName);
                    Console.Write("=");
                    if (temp.result == -1)
                        Console.WriteLine(temp.attrValue + " :");
                    else
                        Console.Write(temp.attrValue + " : ");

                    printTree(temp, level + 1);// recursively with level + 1
                }

            }
            else
            {// it is a leaf node , hence terminate
                Console.Write(tn.result);
            }

        }

        public int ClassifyTest(Dictionary<string, int> testValues, TreeNode treeRoot)
        {
            int returnValue = -1;
            TreeNode treeTraverse = treeRoot;
            if (treeTraverse.attrName == null || treeTraverse.attrName.Length == 0)
            {
                return returnValue;// if root node = null ; 
            }

            while (treeTraverse != null)
            {
                if (treeTraverse.result != -1) // leaf node 
                {
                    returnValue = treeTraverse.result;
                    treeTraverse = null;
                    break;
                }
                else // not leaf node 
                {
                    string attrKey = treeTraverse.attrName;
                    if (testValues.ContainsKey(attrKey))
                    {
                        int tmpAttrValue = testValues[attrKey]; // den einai pinakas mhn skalwseis 
                        if (treeTraverse.children.Count == 0)
                        {
                            break;
                        }
                        bool vFound = false; // value has been founded 
                        foreach (TreeNode tmpChild in treeTraverse.children)
                        {
                            if (tmpChild.attrValue == tmpAttrValue)
                            {
                                vFound = true;
                                if (tmpChild.result != -1)
                                {
                                    returnValue = tmpChild.result;

                                    treeTraverse = null;
                                    break;
                                }
                                else
                                {
                                    treeTraverse = tmpChild;

                                    continue;
                                }
                            }
                        }
                        if (!vFound) break;

                    }
                    else
                    {
                        Console.WriteLine("Failed to find this attr");
                        break;
                    }
                }
            }

            return returnValue;
        }
    }
}
