using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formal_Language.Classes
{
    class UsefulStuff
    {
        /// <summary>
        /// Returns the index of the first character found.
        /// </summary>
        /// <param name="String">String where the character will be sought.</param>
        /// <param name="character">Character which will be sought.</param>
        /// <returns>Index of the character if it was found, or -1 if it wasn't.</returns>
        public static int GetCharacterIndex(string String, char character)
        {
            for (int index = 0; index < String.Length; index++)
            {
                if (String[index] == character)
                {
                    return index;
                }
            }

            return -1;
        }


        /// <summary>
        /// Gets the index of the element in the provided array (if it exists), 
        /// or -1 if it doesn't exist.
        /// </summary>
        /// <param name="element">The searched element.</param>
        /// <param name="array">Array which will be searched.</param>
        /// <returns>Index of the element (if it's found) or -1 (if it's not found).</returns>
        public static int GetElemetIndexInArray(string element, string[] array)
        {
            for (int index = 0; index < array.Length; index++)
            {
                if (array[index] == element)
                {
                    return index;
                }
            }

            return -1;
        }


        /// <summary>
        /// Gets the index of the element in the provided list (if it exists), 
        /// or 1000000 if it doesn't exist. 
        /// </summary>
        /// <param name="element">The searched element.</param>
        /// <param name="list">List which will be searched.</param>
        /// <returns></returns>
        public static int GetElementIndexInList(string element, List<string> list)
        {
            int minIndex = 1000000;

            for (int index = 0; index < element.Length; index++)
            {
                for (int listIndex = 0; listIndex < list.Count; listIndex++)
                {
                    string listElement = list.ElementAt(listIndex);

                    foreach (char character in listElement)
                    {
                        if (character == element[index])
                        {
                            minIndex = (listIndex < minIndex) ? listIndex : minIndex;
                        }

                    }
                }
            }

            return minIndex;
        }
    }
}
