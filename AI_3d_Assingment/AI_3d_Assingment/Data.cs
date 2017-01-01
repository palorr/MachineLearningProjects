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
        List<int[]> rows { get; set; }
        public int rowsNum { get; set; }

        //ctor
        public Data()
        {
            rows = new List<int[]>();
            Attrs = new List<String>();
        }

        //print the data 
        public void print()
        {
            foreach (String attr in Attrs)
            {
                Console.Write("\t" + attr);
            }
            foreach (int[] temp in rows)
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
            foreach (int[] temp in rows)
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
                }

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

            }



        }

        //with this function i remove the col with a spesific attr name
        public Data split(string attrName, int value)
        {
            Data dataReturn = new Data();
            List<int[]> rowsReturn = new List<int[]>();
            List<string> headersReturn = new List<string>();
            int attrIndex = 0;

            ////////////////////All headers except the selected///////////////////////

            foreach (string tmpHeader in Attrs)
            {
                if (!tmpHeader.Equals(attrName))
                    headersReturn.Add(tmpHeader);
            }

            dataReturn.cols = headersReturn.Count;
            dataReturn.Attrs = headersReturn;

            for (attrIndex = 0; attrIndex < dataReturn.Attrs.Count; attrIndex++)
            {
                if (attrName.Equals(dataReturn.Attrs[attrIndex]))
                    break;
            }

            foreach (int[] tmpRow in rows)
            {
                if (tmpRow[attrIndex] == value)
                {
                    int[] tmpCol = new int[headersReturn.Count];
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

            dataReturn.rows = rowsReturn;
            dataReturn.rowsNum = rowsReturn.Count;
            return dataReturn;
        }
        
    }
}
