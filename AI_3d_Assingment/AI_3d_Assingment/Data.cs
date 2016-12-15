using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_3d_Assingment
{
    class Data
    {
        //matrix implementation
        int cols { get; set; }
        List<String> Attrs { get; set; }
        List<int[]> rows { get; set; }
        int rowsNum { get; set; }

        //ctor
        public Data()
        {
            rows = new List<int[]>();
            Attrs = new List<String>();
        }

        //print
        public void printHeader()
        {
            foreach (String temp in Attrs)
            {
                Console.WriteLine("\t" + temp);
            }
            foreach (int[] temp in rows)
            {
                Console.WriteLine("");
                for (int i = 0; i < cols; i++)
                {
                    Console.WriteLine("\t" + temp[i]);
                }
            }
            Console.WriteLine("");
        }

        //FIlls the given array with values for which index is matched to the given index
        public void fillArray(int[] arrayToFill, int indexToFetch)
        {
            int arrIndex = 0;
            foreach (int[] temp in rows)
            {
                arrayToFill[arrIndex++] = temp[indexToFetch];
            }
        }

        //reads the file an prepare the our matrix data 
        public void prepare(String @filePath, int percentage)
        {
            string[] lines = System.IO.File.ReadAllLines(filePath);
            //////////////////////////////headers reader////////////////////////////////
            {
                string[] headers = lines[0].Split(' ');
                cols = headers.Length;
                foreach (string header in headers)
                {
                    Attrs.Add(header);
                }
            }
            ////////////////////////////////data reader/////////////////////////////////
            {
                int[] tmpRow = new int[cols];

                for (int i = 1; i < lines.Length; i++)
                {
                    string tmpLine = lines[i];
                    string[] tmpElements = tmpLine.Split(' ');

                    for (int j = 0; j < tmpElements.Length; j++)
                    {
                        try
                        {
                            tmpRow[j] = Int32.Parse(tmpElements[j]);
                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine(e.Message, " Cant parse the string");
                        }

                    }

                    rows.Add(tmpRow);
                }
            }
            /////////////////////////////calculate percentage/////////////////////////////
            {
                int finalRowsNum = (percentage * rows.Count) / 100;

                if (!(finalRowsNum == rows.Count))
                {
                    for (int i = rows.Count - 1; i > finalRowsNum; i--)
                    {
                        rows.RemoveAt(i);
                    }
                }
            }

            rowsNum = rows.Count; 
            //////////////////////////////////////////////////////////////////////////////






        }




    }
}
