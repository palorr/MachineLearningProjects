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

            Console.WriteLine("Please give the path of the file to train the app!!");
            string trainFilePath = Console.ReadLine();
            Console.WriteLine("Now give the percentage (100 if you wish to learn the whole train file)");
            int percentToLearnFromTrain = Int32.Parse(Console.ReadLine());
            string trainText = System.IO.File.ReadAllText(trainFilePath);

            Learner learner = new Learner(trainFilePath, percentToLearnFromTrain);
            Node root = learner.start();

            Console.WriteLine("Ok the app has learned by the file!!");
            Console.WriteLine("Now for each attribute give the value (0 or 1 if you use the default train file train.dat)");
            Console.WriteLine("In other case you can use as many values as you want");
            
            Dictionary<string, int> testRow = new Dictionary<string, int>();

            Console.WriteLine("attr1: ");
            int attr1 = Int32.Parse(Console.ReadLine());

            Console.WriteLine("attr2: ");
            int attr2 = Int32.Parse(Console.ReadLine());

            Console.WriteLine("attr3: ");
            int attr3 = Int32.Parse(Console.ReadLine());

            Console.WriteLine("attr4: ");
            int attr4 = Int32.Parse(Console.ReadLine());

            Console.WriteLine("attr5: ");
            int attr5 = Int32.Parse(Console.ReadLine());

            Console.WriteLine("attr6: ");
            int attr6 = Int32.Parse(Console.ReadLine());


            testRow.Add("attr1", attr1);
            testRow.Add("attr2", attr2);
            testRow.Add("attr3", attr3);
            testRow.Add("attr4", attr4);
            testRow.Add("attr5", attr5);
            testRow.Add("attr6", attr6);

            Console.WriteLine("The result is: "+ root.ScanAndCalculate(testRow, root));

        }
    }
}
