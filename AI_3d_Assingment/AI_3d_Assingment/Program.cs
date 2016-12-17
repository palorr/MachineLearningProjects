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
            string filePath = @"C:\Users\archo\Desktop\MachineLearningProjects\AI_3d_Assingment\AI_3d_Assingment\Test Data\train.dat";
            string text = System.IO.File.ReadAllText(filePath);
            //Console.WriteLine(text);

            Data testData = new Data();
            testData.prepare(filePath, 100);
            //testData.print();

            testData.split("attr1",0);
            testData.print();
        }
    }
}
