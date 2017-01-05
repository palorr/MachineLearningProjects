using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_3d_Assingment
{
    class Node
    {
        public int valueOfPrevNode { get; set; }
        public string attrName { get; set; }
        public List<Node> children { get; set; }
        public int result { get; set; }   // the result attribute value (-1 if not leaf node)
        public double gain { get; set; }

        //ctor default
        public Node()
        {
            children = new List<Node>();
        }

        //ctor
        public Node(int attrValue, String attrName, int result, double gain)
        {
            this.valueOfPrevNode = attrValue;
            this.attrName = attrName;
            this.result = result;
            this.gain = gain;
        }

        //dont use it !!!
        public void printTree(Node node)
        {
            Console.WriteLine(node.attrName + " with value " + node.valueOfPrevNode + " and with gain " + node.gain + " and result " + node.result);
            printTree(node.children[1]);
        }

        public int ScanAndCalculate(Dictionary<string, int> testData, Node root)
        {
            Node traverser = root;
            string attrToFollow;
            List<Node> childrenOfTraverser;
            int valueToTest;

            if (traverser.valueOfPrevNode != -1)  // root has -1 value because it has not parents so the value of prev node cant lead anywhere
            {
                Console.WriteLine("this is not the root node !!");
                return -1;
            }

            while (traverser.result == -1)//if not -1 we are in a leaf node
            {
                attrToFollow = traverser.attrName;
                childrenOfTraverser = traverser.children;

                if (testData.ContainsKey(attrToFollow))
                    valueToTest = testData[attrToFollow]; //here 
                else
                {
                    Console.WriteLine("Cant match the attributes in the node with name: " + attrToFollow);
                    return -1; 
                }

                foreach (Node child in childrenOfTraverser)
                {
                    if(child.valueOfPrevNode == valueToTest)
                    {
                        traverser = child;
                        break; 
                    }
                }
            }//end while if traverser is a leaf !!! 

            
            return traverser.result;
        }

    }
}
