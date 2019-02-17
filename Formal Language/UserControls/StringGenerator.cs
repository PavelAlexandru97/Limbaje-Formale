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
    public partial class StringGenerator : UserControl
    {
        List<string> terminalList = new List<string>();
        List<string> startList = new List<string>();
        List<List<string>> productionList = new List<List<string>>();

        Random randomNumberGenerator = new Random();

        int characterLimit = 5;

        public StringGenerator()
        {
            InitializeComponent();
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            string generatedString = "";
            bool isGenerationFinished = false;

            // Choose one of the start values
            int startIndex = randomNumberGenerator.Next(0, startList.Count);
            generatedString += startList[startIndex];


            // Keep replacing nonterminals
            while (!isGenerationFinished)
            {
                if ( generatedString == Replace(generatedString, productionList, characterLimit))
                {
                    isGenerationFinished = true;
                }
                else
                {
                    generatedString = Replace(generatedString, productionList, characterLimit);
                }
            }

            textBoxResult.Text = "Size: " + generatedString.Length + Environment.NewLine + generatedString;
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            textBoxDataRead.Text = "Selecting file...";

            terminalList = new List<string>();
            startList = new List<string>();
            productionList = new List<List<string>>();

            buttonGenerate.Enabled = false;

            // Read from file and store read data into appropiate lists
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxDataRead.Text = "";

                System.IO.StreamReader streamReader = new System.IO.StreamReader(openFileDialog.FileName);
                string line;
                int lineCounter = 0;
                while ( (line = streamReader.ReadLine() ) != null )
                {
                    // First line should contain the list of terminals
                    if (lineCounter == 0)
                    {
                        string[] terminali = line.Split();
                        foreach (string terminal in terminali)
                        {
                            terminalList.Add(terminal);
                        }
                    }

                    // Second line should contain the list of start values
                    if (lineCounter == 1)
                    {
                        string[] starturi = line.Split();
                        foreach (string start in starturi)
                        {
                            startList.Add(start);
                        }
                    }

                    // Every other line should contain a production
                    if (lineCounter > 1)
                    {
                        string[] productii = line.Split();
                        List<string> lista = new List<string>();
                        foreach (string productie in productii)
                        {
                            lista.Add(productie);
                        }
                        productionList.Add(lista);
                    }

                    lineCounter++;
                }

                // Output terminal list
                textBoxDataRead.Text += "Terminals: ";
                foreach (string terminalValue in terminalList)
                {
                    textBoxDataRead.Text += terminalValue + " ";
                }

                // Output start list
                textBoxDataRead.Text += Environment.NewLine + "Start: ";
                foreach (string startValue in startList)
                {
                    textBoxDataRead.Text += startValue + " ";
                }

                // Output production list
                textBoxDataRead.Text += Environment.NewLine + "Productions:" + Environment.NewLine;
                foreach (List<string> production in productionList)
                {
                    textBoxDataRead.Text += "    ";
                    foreach (string productionValue in production)
                    {
                        textBoxDataRead.Text += productionValue + " ";
                    }
                    textBoxDataRead.Text += Environment.NewLine;
                }

                // Output nonterminals (first value from each production is a nonterminal)
                textBoxDataRead.Text += Environment.NewLine + "Nonterminals: ";
                foreach (List<string> lista in productionList)
                {
                    textBoxDataRead.Text += lista[0] + " ";
                }
                streamReader.Close();

                // If everything went alright, make the Generate button available
                buttonGenerate.Enabled = true;
            }

            else
            {
                textBoxDataRead.Text = "Data couldn't be read... Make sure you selected a file.";
            }
        }

        /// <summary>
        /// Finds the first nonterminal, and replaces it with one of the values found in its production.
        /// </summary>
        /// <param name="String">The string that will be analyzed.</param>
        /// <param name="productionList">The list of productions.</param>
        /// <param name="characterLimit">The maximum number of characters the final string should have. Default is 60.</param>
        /// <returns>The string after a succesful replace, or the same string if there are no more nonterminals or
        /// if it's above the limit.</returns>
        string Replace(string String, List<List<string>> productionList, int characterLimit = 60)
        {
            foreach (char character in String)
            {
                foreach (List<string> production in productionList)
                {
                    // If the character is found in a production
                    if (character == production[0][0] && String.Length < characterLimit)
                    {
                        // Get a random value from the production
                        int indexProductionValue = randomNumberGenerator.Next(1, production.Count);
                        int indexCharacterToReplace = UsefulStuff.GetCharacterIndex(String, character);

                        // Replace character
                        String = String.Remove(indexCharacterToReplace, 1);
                        String = String.Insert(indexCharacterToReplace, production[indexProductionValue]);

                        // Return after replace
                        return String;
                    }
                }
            }

            // In this phase, the string should be over the limit or it wouldn't contain any nonterminals
            return String;
        }

        private void numericUpDownCharacterLimit_ValueChanged(object sender, EventArgs e)
        {
            characterLimit = (int)numericUpDownCharacterLimit.Value;
        }
    }
}
