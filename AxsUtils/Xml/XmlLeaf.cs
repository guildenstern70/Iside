using System;
using System.Collections;
using System.Text;

namespace AxsUtils.Xml
{
    /// <summary>
    ///     XmlLeaf is basically an XML dictionary one to many.
    ///     The XML leaf
    ///     <code>
    /// 		<prova>
    ///             <valore1>10</valore1>
    ///             <valore2>20</valore2>
    ///         </prova>
    ///  </code>
    ///     is rendered as
    ///     XmlLeaf.category="prova"
    ///     XmlLeaf.values={ valore1=10, valore2=20 }
    ///     We call "Names" 'valore1' and 'valore2'
    ///     and "Values" '10' and '20'.
    ///     Since Values are stored in an HashTable based on Names key,
    ///     the Names in a single Leaf cannot be equals.
    /// </summary>
    public class XmlLeaf
    {
        private readonly string category;
        private int index;
        private readonly Random r;
        private readonly Hashtable values;

        /// <summary>
        ///     Summary description for XmlLeaf.
        /// </summary>
        public XmlLeaf(string cCategory)
        {
            this.r = new Random();
            this.category = cCategory;
            this.values = new Hashtable();
        }

        /// <summary>
        ///     Add name/value pair to current leaf.
        /// </summary>
        /// @param nome name of current item
        /// @param valore value of current item
        public void AddValue(string nome, string valore)
        {
            string name = nome;

            // equality check
            if (this.values.ContainsKey(name))
            {
                name += string.Format("{0}", ++this.index);
            }
            this.values.Add(name, valore);
        }

        /// <summary>
        ///     Get random value in current category.
        /// </summary>
        /// <returns></returns>
        public string GetRandomValue()
        {
            int valuesCount = this.values.Count;
            int rndPosition = this.r.Next(valuesCount);
            ICollection vals = this.values.Values;
            string[] strArray = new string[valuesCount];
            vals.CopyTo(strArray, 0);
            return strArray[rndPosition];
        }

        /// <summary>
        ///     Get value associated to a particular item (nome)
        /// </summary>
        /// @return value (string)
        public string GetValue(string nome)
        {
            return (string) this.values[nome];
        }

        /// <summary>
        ///     Get value associated to a particular item (nome)
        /// </summary>
        /// @return value (int)
        public int GetIntValue(string nome)
        {
            string valore = (string) this.values[nome];
            return int.Parse(valore);
        }

        /// <summary>
        ///     Get value associated to a particular item (nome)
        /// </summary>
        /// @return value (double)
        public double GetDoubleValue(string nome)
        {
            string valore = (string) this.values[nome];
            return double.Parse(valore);
        }

        /// <summary>
        ///     Get container of items for this leaf.
        /// </summary>
        /// @return items in this leaf (Set)
        public ICollection GetNames()
        {
            return this.values.Keys;
        }

        /// <summary>
        ///     Get category of this leaf.
        /// </summary>
        /// @return category name
        public string GetCategory()
        {
            return this.category;
        }

        /// <summary>
        ///     Long description of leaf (category and values)
        /// </summary>
        public override string ToString()
        {
            IDictionaryEnumerator i = this.values.GetEnumerator();
            StringBuilder sb = new StringBuilder(this.category);
            sb.Append(":\n");
            while (i.MoveNext())
            {
                sb.Append("\t");
                sb.Append(i.Key);
                sb.Append("=");
                sb.Append(i.Value);
                sb.Append("\n");
            }
            return sb.ToString();
        }
    }
}