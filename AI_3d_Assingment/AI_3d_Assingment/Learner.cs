using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_3d_Assingment
{
    class Learner
    {
        string filename;
        int percentage;

        public Learner(string filename , int percentage)
        {
            this.filename = filename;
            this.percentage = percentage; 
        }

        public TreeNode start() // lets start learning
        {
            if (filename == null )
            {
                Console.WriteLine("the file name is null");
            }
            if (percentage<0 && percentage>100)
            {
                Console.WriteLine("incorect percentage");
            }

            Data data = new Data();
            data.prepare(filename , percentage );

            Dictionary<string, int[]> setTrainingVector = new Dictionary<string, int[]>();

            for (int i = 0; i < data.cols -1; i++)
            {
                int[] trainingVector = new int[data.rowsNum];
                data.fillArray(trainingVector, i);
                setTrainingVector.Add(data.Attrs[i], trainingVector); 
            }

            int[] finalClass = new int[data.rowsNum];
            data.fillArray(finalClass, data.cols - 1);

            TreeNode rootNode = new TreeNode();
            rootNode.attrValue = -1;

            learnTree(setTrainingVector, finalClass, rootNode, data);
            return rootNode;
        }
        //his function generates a decision tree recursively
        public void learnTree(Dictionary<string , int[]> setTrainingVector, int[] finalClass, TreeNode node, Data data)
        {
            if (checkFinalClass(finalClass, 0))//todo
            {
                node.result = 0;
                return;
            }
            else if (checkFinalClass(finalClass, 1))
            {
                node.result = 1;
                return;
            }

            if (setTrainingVector.Count == 1)//not sure 
            {
                int pos = getCountPositives(finalClass);//todo
                int neg = finalClass.Length - pos;

                if(pos >= neg)
                {
                    node.result = 0;
                    return;
                }
                else
                {
                    node.result = 1;
                    return;
                }
            }
            else
            {
                Dictionary<String, Double> attributesGains = new Dictionary<String, Double>();
                Dictionary<String, List<int>> mapAttributesValuesInListUnique = new Dictionary<String, List<int>>();

                double entropyS = getEntropy(finalClass);// initial entropy // todo

                foreach (KeyValuePair<string,  int[]> entry in setTrainingVector)//not sure
                {
                    // do something with entry.Value or entry.Key

                    Dictionary<int, int> atrPositive = new Dictionary<int, int>();
                    Dictionary<int, int> atrNegative = new Dictionary<int, int>();
                    List<int> atrUnique = new List<int>();

                    int[] trainingClass = (int[])entry.Value;

                }




            }
        }

    }
}
