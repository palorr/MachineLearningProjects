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

            string testFilePath = @"C:\Users\archo\Desktop\MachineLearningProjects\AI_3d_Assingment\AI_3d_Assingment\Test Data\test.dat";
            string testText = System.IO.File.ReadAllText(testFilePath);

            int percentToLearnFromTrain = 100;

            Learner learner = new Learner(trainFilePath, percentToLearnFromTrain);
            TreeNode root = learner.start();

            if (root != null)
            {
                root.printTree(root, 0);

                //{// Calculating the accuracy on Training Set
                //    Data matrixTests = new Data();
                //    matrixTests.prepare(trainFilePath, 100);
                //    // matrixTests.printMatrix();
                //    matrixTests.getTestAccuracy(root, "Training Set");

                //}
                //{// Calculating the accuracy on Test Set
                //    Data matrixTests2 = new Data();
                //    matrixTests2.prepare(testFilePath, 100);
                //    // matrixTests.printMatrix();
                //    matrixTests2.getTestAccuracy(root, "Test Set");
                //}
            }
            else
            {
                Console.WriteLine("failed");
            }


        }
    }
}
