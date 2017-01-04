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

        public Learner(string filename, int percentage)
        {
            this.filename = filename;
            this.percentage = percentage;
        }

        // lets start learning and create the root node 
        public TreeNode start()
        {
            if (filename == null)
            {
                Console.WriteLine("the file name is null");
            }
            if (percentage < 0 && percentage > 100)
            {
                Console.WriteLine("incorect percentage");
            }

            Data data = new Data();
            data.prepare(filename, percentage); // Data prepare method

            Dictionary<string, int[]> setTrainingVector = new Dictionary<string, int[]>();
            
            //in this for we fill the setTrainingVector with the data 
            for (int i = 0; i < data.cols - 1; i++)
            {
                int[] trainingVector = new int[data.rowsNum];
                data.fillArray(trainingVector, i);
                setTrainingVector.Add(data.Attrs[i], trainingVector);//key,value
            } 

            int[] finalClass = new int[data.rowsNum];
            data.fillArray(finalClass, data.cols - 1);  //all perfect until here 

            TreeNode rootNode = new TreeNode();
            rootNode.attrValue = -1;

            learnTree(setTrainingVector, finalClass, rootNode, data);
            return rootNode;
        }
        
        //this function generates a decision tree recursively
        public void learnTree(Dictionary<string, int[]> TrainingDictionary, int[] finalClass, TreeNode node, Data data)
        {
            if (checkFinalClass(finalClass, 0))//check if all values are 0
            {
                node.result = 0;
                return;
            }
            else if (checkFinalClass(finalClass, 1))//check if all values are 0
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
                Dictionary<String, Double> attributesGains = new Dictionary<String, Double>();
                Dictionary<String, List<int>> mapAttributesValuesInListUnique = new Dictionary<String, List<int>>();

                double entropy = getEntropy(finalClass);// initial entropy

                foreach (KeyValuePair<string, int[]> pair in TrainingDictionary)//not sure
                {
                    //entry.Value or entry.Key

                    Dictionary<int, int> atrPositive = new Dictionary<int, int>();
                    Dictionary<int, int> atrNegative = new Dictionary<int, int>();
                    List<int> atrUnique = new List<int>();

                    int[] trainingClass = (int[])pair.Value;//key value pair -> value in trainingClass

                    for (int i = 0; i < trainingClass.Length; i++) // find individual entropies
                    {
                        addOnlyUnique(atrUnique, trainingClass[i]);  // addOnlyUnique adds a value to the arraylist only if does not exists in the list

                        if (finalClass[i] == 0)// its a positive
                        {
                            if (atrPositive.ContainsKey(trainingClass[i]))
                            {
                                //atrPositive.Add(trainingClass[i], atrPositive[trainingClass[i]] + 1); //the key already exists so cracks
                                atrPositive[trainingClass[i]]++;   //increase the value by 1 (we love .net <3)
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
                                //atrNegative.Add(trainingClass[i], atrNegative[trainingClass[i]] + 1); // not fucking sure
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
                            if (atrPositive[tempAttr] != null)
                                positives = atrPositive[tempAttr];
                            if (atrNegative[tempAttr] != null)
                                negatives = atrNegative[tempAttr];

                            double val1 = (double)(positives) / (positives + negatives);
                            double val2 = (double)(negatives) / (positives + negatives);
                            entropyTemp = -(val1 * log2(val1)) - (val2 * log2(val2));

                            gain = gain - ((((double)positives + negatives) / trainingClass.Length) * entropyTemp);
                        }

                        attributesGains.Add((String)pair.Key, gain);
                    }
                }//end foreach

                // now lets select the maximum gain
                String attributeWithMAxGain = "";
                double maxGainValue = 0.0;
                int indexToChoose = 0;

                foreach (KeyValuePair<string, int[]> entry in TrainingDictionary)
                {
                    double tempGain = attributesGains[(String)entry.Key];
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

                // Now we will call this algorithm recursively for how many attributes values of max attribute.
                List<int> atrUniqueValuesForAttrMaxGain = mapAttributesValuesInListUnique[attributeWithMAxGain];

                foreach (int tempAtrUniqueValue in atrUniqueValuesForAttrMaxGain)
                {

                    TreeNode NodeChild = new TreeNode();
                    NodeChild.attrValue = tempAtrUniqueValue;// since its a child
                                                              // node
                    node.children.Add(NodeChild);
                    Data child = data.split(attributeWithMAxGain, tempAtrUniqueValue); //outlook , sunny

                    Dictionary<String, int[]> setTrainingVectorChild = new Dictionary<String, int[]>();
                    // Now i need a set of R training vectors
                    for (int i = 0; i < child.cols - 1; i++)
                    {
                        int[] trainingVectorChild = new int[child.rowsNum];
                        child.fillArray(trainingVectorChild, i);
                        setTrainingVectorChild.Add(child.Attrs[i], trainingVectorChild);
                    }

                    // i need final class vector
                    int[] FinalClassChild = new int[child.rowsNum];
                    child
                            .fillArray(FinalClassChild, child.cols - 1);

                    learnTree(setTrainingVectorChild, FinalClassChild, NodeChild, child); //recurse

                }

                return;

            }//end else
        }

        //log base 2 , need this for entropy 
        public static double log2(double num)
        {
            if (num <= 0)
                return 0.0;
            return (Math.Log(num) / Math.Log(2));
        }

        // If all the attributes in final class equals valueToChecked returns True
        public bool checkFinalClass(int[] FinalClass, int valueToChecked)
        {
            for (int i = 0; i < FinalClass.Length; i++)
            {
                if (FinalClass[i] != valueToChecked)
                    return false;
            }
            return true;
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

        // Returns entropy calculated for a given set of vector
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

        // Adds a value to the arraylist only if does not exists in the list
        public void addOnlyUnique(List<int> data, int val)
        {
            if (!data.Contains(val))
                data.Add(val);
        }
    }
}
