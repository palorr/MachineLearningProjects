using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_3d_Assingment
{
    class Program
    {
        static void Main(string[] args)
        {
            string trainFilePath = @"C:\Users\archo\Desktop\MachineLearningProjects\AI_3d_Assingment\AI_3d_Assingment\Test Data\train.dat";
            string trainText = System.IO.File.ReadAllText(trainFilePath);

            int percentToLearnFromTrain = 100;

            Learner learner = new Learner(trainFilePath, percentToLearnFromTrain);
            Node root = learner.start();
            //root.printTree(root);

            Dictionary<string, int> testRow = new Dictionary<string, int>();
            testRow.Add("attr1", 0);
            testRow.Add("attr2", 1);
            testRow.Add("attr3", 1);
            testRow.Add("attr4", 0);
            testRow.Add("attr5", 1);
            testRow.Add("attr6", 0);

            root.ScanAndCalculate(testRow, root); 



        }
    }
}
