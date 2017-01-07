using System;
using System.Collections.Generic;

/// <summary>
/// 3130160 Nikolaos Papadogoulas
/// 3130198 Archontellis-Rafael Sotirchellis
/// </summary>

namespace AI_3d_Assingment
{
    class Learner
    {
        string filename;
        int percentage;

        public Learner(string filename, int percentage)
        {
            this.filename = filename;
            this.percentage = percentage;
        }

        // lets start learning and create the root node 
        public Node Start()
        {
            if (filename == null)
            {
                Console.WriteLine("The file name is null");
            }
            if (percentage < 0 && percentage > 100)
            {
                Console.WriteLine("Incorect percentage");
            }

            Data data = new Data();
            data.prepare(filename, percentage);

            Dictionary<string, int[]> TrainingDictionary = new Dictionary<string, int[]>();

            //fill the setTrainingVector with the data 
            for (int i = 0; i < data.cols - 1; i++)
            {
                int[] trainingVector = new int[data.rowsNum];
                data.fillCol(trainingVector, i);
                TrainingDictionary.Add(data.Attrs[i], trainingVector);//key,value
            }

            int[] finalClass = new int[data.rowsNum];
            data.fillCol(finalClass, data.cols - 1);  

            Node rootNode = new Node();
            rootNode.valueOfPrevNode = -1; //this happens because there is not prev node

            LearnAndCreateTree(TrainingDictionary, finalClass, rootNode, data); //here we call the learnTree
            return rootNode;
        }

        //this funcion generates a decision tree recursively
        public void LearnAndCreateTree(Dictionary<string, int[]> TrainingDictionary, int[] finalClass, Node node, Data data)
        {
            if (ValidateFinalClass(finalClass, 0))//check if all values are 0
            {
                node.result = 0;
                return;
            }
            else if (ValidateFinalClass(finalClass, 1))//check if all values are 0
            {
                node.result = 1;
                return;
            }

            if (TrainingDictionary.Count == 1)
            {
                int pos = getCountPositives(finalClass);
                int neg = finalClass.Length - pos;

                if (pos >= neg)
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
                Dictionary<String, Double> attrsGains = new Dictionary<String, Double>();
                Dictionary<String, List<int>> mapAttributesValuesInListUnique = new Dictionary<String, List<int>>();

                double entropy = getEntropy(finalClass);// initial entropy

                foreach (KeyValuePair<string, int[]> pair in TrainingDictionary)//not sure
                {
                    
                    Dictionary<int, int> atrPositive = new Dictionary<int, int>();
                    Dictionary<int, int> atrNegative = new Dictionary<int, int>();
                    List<int> atrUnique = new List<int>();

                    int[] trainingClass = (int[])pair.Value;//key value pair -> value in trainingClass

                    for (int i = 0; i < trainingClass.Length; i++) // find entropies
                    {

                        if (!atrUnique.Contains(trainingClass[i]))
                            atrUnique.Add(trainingClass[i]);

                        if (finalClass[i] == 0)// its a positive
                        {
                            if (atrPositive.ContainsKey(trainingClass[i]))
                            {
                                atrPositive[trainingClass[i]]++;   //increase the value by 1
                            }
                            else
                            {
                                atrPositive.Add(trainingClass[i], 1);
                            }
                        }
                        else   //negative
                        {
                            if (atrNegative.ContainsKey(trainingClass[i]))
                            {
                                atrNegative[trainingClass[i]]++; //increase the value by 1
                            }
                            else
                            {
                                atrNegative.Add(trainingClass[i], 1);
                            }

                        }

                    }
                    mapAttributesValuesInListUnique.Add((String)pair.Key, atrUnique);

                    {//calculate the gain
                        double gain = entropy;
                        foreach (int tempAttr in atrUnique)
                        {
                            double entropyTemp = 0.0;
                            int positives = 0;
                            int negatives = 0;

                            if (atrPositive.ContainsKey(tempAttr))
                                positives = atrPositive[tempAttr];
                            else
                                positives = 0;

                            if (atrNegative.ContainsKey(tempAttr))
                                negatives = atrNegative[tempAttr];
                            else
                                negatives = 0; 

                            double val1 = (double)(positives) / (positives + negatives);
                            double val2 = (double)(negatives) / (positives + negatives);
                            entropyTemp = -(val1 * log2(val1)) - (val2 * log2(val2));

                            gain = gain - ((((double)positives + negatives) / trainingClass.Length) * entropyTemp);
                        }

                        attrsGains.Add((String)pair.Key, gain);
                    }
                }//end foreach

                // now lets select the maximum gain
                String attributeWithMAxGain = "";
                double maxGainValue = 0.0;
                int indexToChoose = 0;

                foreach (KeyValuePair<string, int[]> entry in TrainingDictionary)
                {
                    double tempGain = attrsGains[(String)entry.Key];
                    if (indexToChoose == 0)
                    {
                        maxGainValue = tempGain;
                        attributeWithMAxGain = (String)entry.Key;
                        indexToChoose++;
                    }
                    if (tempGain > maxGainValue)
                    {
                        maxGainValue = tempGain;
                        attributeWithMAxGain = (String)entry.Key;
                    }
                }
                node.attrName = attributeWithMAxGain;
                node.result = -1;
                node.gain = maxGainValue;

                //for each value of each attribute
                List<int> atrUniqueValuesForAttrMaxGain = mapAttributesValuesInListUnique[attributeWithMAxGain];

                foreach (int attrUniqueValue in atrUniqueValuesForAttrMaxGain)
                {
                    //split tha data 
                    Data childData = data.split(attributeWithMAxGain, attrUniqueValue);
                    
                    /////////////////////////////////////////
                    //with this check I avoid the infinity loop in case of non-pure final data
                    if (finalClass.Length == childData.rowsNum)
                    {
                        node.result = calcResultInCaseOfTerminalAndNotPure(finalClass);
                        return; 
                    }
                    /////////////////////////////////////////

                    Node NodeChild = new Node();
                    NodeChild.valueOfPrevNode = attrUniqueValue;// since its a child
                                                             
                    node.children.Add(NodeChild);

                    Dictionary<String, int[]> childTrainingDictionary = new Dictionary<String, int[]>();

                    for (int i = 0; i < childData.cols - 1; i++)
                    {
                        int[] trainingVectorChild = new int[childData.rowsNum];
                        childData.fillCol(trainingVectorChild, i);
                        childTrainingDictionary.Add(childData.Attrs[i], trainingVectorChild);
                    }

                    int[] FinalClassOfChild = new int[childData.rowsNum];
                    childData.fillCol(FinalClassOfChild, childData.cols - 1);//last column is the result

                    LearnAndCreateTree(childTrainingDictionary, FinalClassOfChild, NodeChild, childData); //recurse

                }

                return;

            }//end else
        }

        // If all the attributes in final class equals valueToChecked returns True
        public bool ValidateFinalClass(int[] finalClass, int valueToChecked)
        {
            for (int i = 0; i < finalClass.Length; i++)
            {
                if (finalClass[i] != valueToChecked)
                    return false;
            }
            return true;
        }
        
        //This function is used when we cant calculate a pure result but we are in a leaf node
        //Returns 0 or 1
        //I have to make it more clever but I need sleep (5/1/2017  4.50 am)
        public int calcResultInCaseOfTerminalAndNotPure(int[] finalClass)
        {
            int sum0 = 0,  sum1 = 0;

            foreach (int cell in finalClass)
            {
                if (cell == 0)
                    sum0++;
                else
                    sum1++;
            }
            if (sum0 > sum1)
                return 0;
            else
                return 1;
        }

        // Returns the count of positives in final class
        public int getCountPositives(int[] FinalClass)
        {
            int countPos = 0;
            for (int i = 0; i < FinalClass.Length; i++)
            {
                if (FinalClass[i] == 0)
                    countPos++;
            }
            return countPos;
        }

        // Calculates the entropy
        public double getEntropy(int[] array)
        {
            double entropy = 0.0;
            int positives = 0;
            int negatives = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 0)// its a positive
                {
                    positives++;
                }
                else
                {// FinalClass is negative
                    negatives++;
                }
            }
            double val1 = (double)(positives) / (positives + negatives);
            double val2 = (double)(negatives) / (positives + negatives);
            entropy = -(val1 * log2(val1)) - (val2 * log2(val2));
            return entropy;
        }

        //Log base 2 , need this for entropy 
        public static double log2(double num)
        {
            if (num <= 0)
                return 0.0;
            return (Math.Log(num) / Math.Log(2));
        }

    }
}
