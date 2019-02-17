using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Formal_Language.Classes;

namespace Formal_Language.UserControls
{
    public partial class PushDown : UserControl
    {
        string[,] actions;
        string[,] salt;
        string[,] productions;
        Stack<string> stack = new Stack<string>();
        int contorCOd = 0;
       List <string> codIntermediar=new List<string>();
        string[] terminals;
        string[] states;
        string[] terminaleSalt;
        string[] startArray;
        int contor = 1;
        List<string> result = new List<string>();

        Stack<string> entryStack;
        string fileFirstRow;

        public PushDown()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Used to open a file, and populate arrays based on the info found in the file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Array used for every Split()
                string[] words;


                // ----- Store all rows in an array
                System.IO.StreamReader streamReader = new System.IO.StreamReader(openFileDialog1.FileName);
                string fileContents = streamReader.ReadToEnd();
                string[] fileRows = fileContents.Split(
                    new[] { Environment.NewLine },
                    StringSplitOptions.None
                );

                // ----- Store first line (Entry stack) for reinitialization
                fileFirstRow = fileRows[0];

                // ----- Second line (states)
                words = fileRows[1].Split();

                states = new string[words.Length];

                for (int index = 0; index < words.Length; index++)
                {
                    states[index] = words[index];
                }


                // ----- Third line (terminals)
                words = fileRows[2].Split();

                terminals = new string[words.Length];

                for (int index = 0; index < words.Length; index++)
                {
                    terminals[index] = words[index];
                }


                // ----- Fourth line -> Fourth line + number of states
                // ----- (states / terminals combinations, aka actions)
                actions = new string[states.Length, terminals.Length];

                int fileLine = 3;
                for (; fileLine < 3 + states.Length; fileLine++)
                {
                    words = fileRows[fileLine].Split();

                    for (int secondIndex = 0; secondIndex < words.Length; secondIndex++)
                    {
                        actions[fileLine - 3, secondIndex] = words[secondIndex];
                    }
                }


                // ----- (terminale salt)
                words = fileRows[fileLine].Split();
                terminaleSalt = new string[words.Length];

                for (int index = 0; index < words.Length; index++)
                {
                    terminaleSalt[index] = words[index];
                }

                salt = new string[states.Length, terminaleSalt.Length];


                // ----- (states / terminale salt)
                int oldFileLine = ++fileLine;
                for (; fileLine < oldFileLine + states.Length; fileLine++)
                {
                    words = fileRows[fileLine].Split();

                    for (int secondIndex = 0; secondIndex < words.Length; secondIndex++)
                    {
                        salt[fileLine - oldFileLine, secondIndex] = words[secondIndex];
                    }
                }


                // ----- Productions
                int size = fileRows.Length - fileLine;

                // Line    -> Production number (begins from 1)
                // Column1 -> String that will be replaced
                // Column2 -> Replacement string
                productions = new string[size + 1, 2];

                int productionNumber = 1;
                for (; fileLine < fileRows.Length - 1; fileLine++, productionNumber++)
                {
                    words = fileRows[fileLine].Split();

                    productions[productionNumber, 0] = words[1];
                    productions[productionNumber, 1] = words[0];
                }


                // ----- Last line (start elements)
                words = fileRows[fileRows.Length - 1].Split();

                startArray = new string[words.Length];
                for (int index = 0; index<words.Length; index++)
                {
                    startArray[index] = words[index];
                }


                // If reading was successfull, enable the generate button
                buttonGenerate.Enabled = true;
            }

            else
            {
                textBoxOutput.Text = "Data couldn't be read... Make sure you selected a file.";
            }
        }


        /// <summary>
        /// Used to generate the push down string. Begins with the elements found
        /// in 'startArray', then calls the recursive function GeneratePushDown.
        /// Lastly, the result will be shown in a textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            labelResultStatus.Text = "";

            result = new List<string>();

            // ----- Reinitialize entry stack
            entryStack = new Stack<string>();
            string[] words = fileFirstRow.Split();

            for (int index = words.Length - 1; index >= 0; index--)
            {
                entryStack.Push(words[index]);
            }


            // ----- Add start elements
            foreach (string startElement in startArray)
                result.Add(startElement);


            // ----- Generate push down string and output the result
            result = GeneratePushDown(result);

            string stringResult = "";
            foreach (string resultElement in result)
                stringResult += resultElement + " ";
            stringResult += "\r\n";
            foreach (string resultElement in codIntermediar)
                stringResult += resultElement + " " + "\r\n";

            textBoxOutput.Text = stringResult;
        }


        /// <summary>
        /// Recursive function meant to generate the push down string.
        /// </summary>
        /// <param name="list">The list with the elements.</param>
        /// <returns></returns>
        List<string> GeneratePushDown(List<string> list)
        {
            string lastElement = list.Last();
           
            // If the last element is a number
            try
            {
                if (lastElement == "11" || lastElement == "10")
                {
                    int ceva = 0;
                }
                int line = Convert.ToInt32(lastElement);
                
                string intrare = entryStack.Peek();
                int column = UsefulStuff.GetElemetIndexInArray(intrare, terminals);

                // Salt
                if (column < 0)
                {
                    column = UsefulStuff.GetElemetIndexInArray(intrare, terminaleSalt);

                    if (salt[line, column] != "NULL")
                    {
                        list.Add( salt[line, column] );
                        return GeneratePushDown(list);
                    }

                    // If this is reached, something went wrong
                    // End recursion with current list
                    labelResultStatus.ForeColor = System.Drawing.Color.Red;
                    labelResultStatus.Text = "Fail";
                    return list;
                }

                // Action
                else
                {
                    string action = actions[line, column];

                    // The generated string is accepted
                    if (action == "acc")
                    {
                        labelResultStatus.ForeColor = System.Drawing.Color.Green;
                        labelResultStatus.Text = "Success";
                        return list;
                    }

                    // Deplasare
                    if (action[0] == 'd')
                    {
                        list.Add( entryStack.Pop() );
                        string remaining = "";
                        for (int index = 1; index < action.Length; index++)
                        {
                            remaining += action[index];
                        }
                        list.Add( remaining );

                        return GeneratePushDown(list);
                    }

                    // Reducere
                    /*
                    if (action[0] == 'r')
                    {
                        string remaining = "";
                        for (int index = 1; index < action.Length; index++)
                        {
                            remaining += action[index];
                        }
                        int productionNumber = Convert.ToInt32( remaining );

                        string stringToBeReplaced   = productions[productionNumber, 0];
                        int stringToBeReplacedIndex = UsefulStuff.GetElementIndexInList(stringToBeReplaced, list);

                        // Remove all elements starting from the string's index
                        list.RemoveRange(stringToBeReplacedIndex, list.Count - stringToBeReplacedIndex);

                        // Add the replacement string
                        list.Add( productions[productionNumber, 1] );

                        return GeneratePushDown(list);
                    }
                    */
                    
                    if (action[0] == 'r')
                    {
                        string operatie123 = "";
                        string op123, op213;
                        string remaining = "";
                        for (int index = 1; index < action.Length; index++)
                        {
                            remaining += action[index];
                        }
                        int productionNumber = Convert.ToInt32(remaining);

                        string production = productions[productionNumber, 1];
                        string stringToBeReplaced = productions[productionNumber, 0];
                        int stringToBeReplacedIndex = UsefulStuff.GetElementIndexInList(stringToBeReplaced, list);

                        // Remove all elements starting from the string's index
                        list.RemoveRange(stringToBeReplacedIndex, list.Count - stringToBeReplacedIndex);

                        // Add the replacement string
                        list.Add(productions[productionNumber, 1]);
                        if (stringToBeReplaced == "a" || stringToBeReplaced == "id")
                            stack.Push(stringToBeReplaced);
                      
                        for (int i = 0; i < stringToBeReplaced.Length; i++)
                        {
                            string ceva = stringToBeReplaced[i].ToString();
                            if (stringToBeReplaced[i] == '+' || stringToBeReplaced[i] == '-' || stringToBeReplaced[i] == '/' || stringToBeReplaced[i] == '*')
                            {
                                operatie123 = stringToBeReplaced[i].ToString();
                                op123 = stack.Pop();
                                op213 = stack.Pop();
                                
                                string TheCOd = "t"+ contor.ToString()+"="+op123 + operatie123 + op213;

                                string pushul = "t" + contor.ToString();
                                contor++;

                                codIntermediar.Add(TheCOd);
                                stack.Push(pushul);
                                contorCOd++;
                            }
                        }

                        return GeneratePushDown(list);
                    }


                    // If this is reached, something went wrong
                    // End recursion with current list
                    labelResultStatus.ForeColor = System.Drawing.Color.Red;
                    labelResultStatus.Text = "Fail";
                    return list;
                }
            }

            // If the last element is not a number
            catch(Exception e)
            {
                string penultimulElement = list.ElementAt(list.Count - 2);

                int line   = Convert.ToInt32(penultimulElement);
                int column = UsefulStuff.GetElemetIndexInArray(lastElement, terminals);

                // Salt
                if (column < 0)
                {
                    column = UsefulStuff.GetElemetIndexInArray(lastElement, terminaleSalt);

                    if (salt[line, column] != "NULL")
                    {
                        list.Add( salt[line, column] );
                        return GeneratePushDown(list);
                    }

                    // If this is reached, something went wrong
                    // End recursion with current list
                    labelResultStatus.ForeColor = System.Drawing.Color.Red;
                    labelResultStatus.Text = "Fail";
                    return list;
                }

                // Action
                else
                {
                    string action = actions[line, column];

                    // The generated string is accepted
                    if (action == "acc")
                    {
                        labelResultStatus.ForeColor = System.Drawing.Color.Green;
                        labelResultStatus.Text = "Success";
                        return list;
                    }

                    // Deplasare
                    if (action[0] == 'd')
                    {
                        list.Add( entryStack.Pop() );
                        string remaining = "";
                        for (int index = 1; index < action.Length; index++)
                        {
                            remaining += action[index];
                        }
                        list.Add(remaining);

                        return GeneratePushDown(list);
                    }

                    // Reducere
                    if (action[0] == 'r')
                    {
                        string remaining = "";
                        for (int index = 1; index < action.Length; index++)
                        {
                            remaining += action[index];
                        }
                        int productionNumber = Convert.ToInt32( remaining );

                        string stringToBeReplaced   = productions[productionNumber, 0];
                        int stringToBeReplacedIndex = UsefulStuff.GetElementIndexInList(stringToBeReplaced, list);

                        // Remove all elements starting from the string's index
                        list.RemoveRange(stringToBeReplacedIndex, list.Count - stringToBeReplacedIndex);

                        // Add the replacement string
                        list.Add(productions[productionNumber, 1]);

                        return GeneratePushDown(list);
                    }

                    // If this is reached, something went wrong
                    // End recursion with current list
                    labelResultStatus.ForeColor = System.Drawing.Color.Red;
                    labelResultStatus.Text = "Fail";
                    return list;
                }
            }
        }


        
    }
}
