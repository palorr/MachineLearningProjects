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
            string filePath = @"C:\Users\archo\Desktop\MachineLearningProjects\AI_3d_Assingment\AI_3d_Assingment\Test Data\test.dat"; 
            string text = System.IO.File.ReadAllText(filePath);
            //string text = System.IO.File.ReadAllText(@"C:\Users\archo\Desktop\MachineLearningProjects\AI_3d_Assingment\AI_3d_Assingment\Test Data\test.dat");
            Console.WriteLine(text);
        }
    }
}
