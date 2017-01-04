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
        public int cols { get; set; }
        public List<String> Attrs { get; set; }
        List<int[]> indexOfEachCol { get; set; } //to periexomeno tis kathe sthlhs 
        public int rowsNum { get; set; }

        //ctor
        public Data()
        {
            indexOfEachCol = new List<int[]>();
            Attrs = new List<String>();
        }

        //print the data 
        public void print()
        {
            foreach (String attr in Attrs)
            {
                Console.Write("\t" + attr);
            }
            foreach (int[] temp in indexOfEachCol)
            {
                Console.WriteLine("");
                for (int i = 0; i < cols; i++)
                {
                    Console.Write("\t" + temp[i]);
                }
            }
            Console.WriteLine("");
        }

        //FIlls the given array with values for which index is matched to the given index
        public void fillArray(int[] arrayToFill, int indexToFetch)
        {
            int arrIndex = 0;
            foreach (int[] temp in indexOfEachCol)
            {
                arrayToFill[arrIndex++] = temp[indexToFetch];
            }
        }

        //reads the file an prepare our matrix data 
        public void prepare(String @filePath, int percentage)
        {
            Console.WriteLine(System.IO.File.Exists(filePath) ? "File exists." : "File does not exist.");

            if (System.IO.File.Exists(filePath))
            {
                string[] lines = System.IO.File.ReadAllLines(filePath);
                //////////////////////////////headers reader////////////////////////////////
                {
                    string[] headers = lines[0].Split('\t');
                    cols = headers.Length + 1;
                    foreach (string header in headers)
                    {
                        Attrs.Add(header);
                    }
                }//all perfect until here 

                Attrs.Add("Result");
                ////////////////////////////////data reader/////////////////////////////////
                {

                    for (int i = 1; i < lines.Length; i++)
                    {
                        int[] tmpRow = new int[cols];
                        string tmpLine = lines[i];
                        string[] tmpElements = tmpLine.Split('\t');

                        for (int j = 0; j < tmpElements.Length ; j++)
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

                        indexOfEachCol.Add(tmpRow);
                    }
                }
                /////////////////////////////calculate percentage/////////////////////////////
                {
                    int finalRowsNum = (percentage * indexOfEachCol.Count) / 100;

                    if (!(finalRowsNum == indexOfEachCol.Count))
                    {
                        for (int i = indexOfEachCol.Count - 1; i > finalRowsNum; i--)
                        {
                            indexOfEachCol.RemoveAt(i);
                        }
                    }
                }

                rowsNum = indexOfEachCol.Count;

            }



        }

        //with this function i remove the col with a spesific attr name
        public Data split(string attrName, int value)//outlook , sunny
        {
            Data dataReturn = new Data();
            List<int[]> rowsReturn = new List<int[]>();
            List<string> headersToReturn = new List<string>();
            int attrIndex = 0;

            foreach (string tmpHeader in Attrs)
            {
                headersToReturn.Add(tmpHeader);
            }

            dataReturn.cols = headersToReturn.Count;
            dataReturn.Attrs = headersToReturn;

            for (attrIndex = 0; attrIndex < dataReturn.Attrs.Count; attrIndex++)
            {
                if (attrName.Equals(dataReturn.Attrs[attrIndex]))
                    break;
            }

            foreach (int[] tmpRow in indexOfEachCol)
            {
                if (tmpRow[attrIndex] == value)
                {
                    int[] tmpCol = new int[headersToReturn.Count];
                    int indexTmpCol = 0;
                    for (int i = 0; i < cols; i++)
                    {
                        if (!(i == attrIndex))
                        {
                            tmpCol[indexTmpCol] = tmpRow[i];
                            indexTmpCol++;
                        }
                    }
                    rowsReturn.Add(tmpRow);
                }
            }

            dataReturn.indexOfEachCol = rowsReturn;
            dataReturn.rowsNum = rowsReturn.Count;
            return dataReturn;
        }
        
    }
}
