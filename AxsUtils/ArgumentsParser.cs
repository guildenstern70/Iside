using System;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace AxsUtils
{
    /// <summary>
    /// Arguments class.
    /// <example>
    ///  // Command line parsing
    ///  ArgumentsParser CommandLine=new ArgumentsParser(Args);
    ///
    ///   // Look for specific arguments values and display 
    ///   // them if they exist (return null if they don't)
    ///  if(CommandLine["param1"] != null) 
    ///    Console.WriteLine("Param1 value: " + 
    ///        CommandLine["param1"]);
    ///  else
    ///    Console.WriteLine("Param1 not defined !");
    ///
    ///  if(CommandLine["height"] != null) 
    ///    Console.WriteLine("Height value: " + 
    ///        CommandLine["height"]);
    ///  else 
    ///    Console.WriteLine("Height not defined !");
    ///
    ///  if(CommandLine["width"] != null) 
    ///    Console.WriteLine("Width value: " + 
    ///        CommandLine["width"]);
    ///  else 
    ///    Console.WriteLine("Width not defined !");
    ///
    ///  if(CommandLine["size"] != null) 
    ///    Console.WriteLine("Size value: " + 
    ///        CommandLine["size"]);
    ///  else 
    ///    Console.WriteLine("Size not defined !");
    ///
    ///  if(CommandLine["debug"] != null) 
    ///    Console.WriteLine("Debug value: " + 
    ///        CommandLine["debug"]);
    ///  else 
    ///    Console.WriteLine("Debug not defined !");
    /// </example>
    /// </summary>
    public class ArgumentsParser
    {
        // Variables
        private StringDictionary parameters;

        // Constructor
        public ArgumentsParser(string[] args)
        {
            parameters = new StringDictionary();
            Regex splitter = new Regex(@"^-{1,2}|^/|=|:",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

            Regex remover = new Regex(@"^['""]?(.*?)['""]?$",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

            string parameter = null;
            string[] parts;

            // Valid parameters forms:
            // {-,/,--}param{ ,=,:}((",')value(",'))
            // Examples: 
            // -param1 value1 --param2 /param3:"Test-:-work" 
            //   /param4=happy -param5 '--=nice=--'
            foreach (string txt in args)
            {
                // Look for new parameters (-,/ or --) and a
                // possible enclosed value (=,:)
                parts = splitter.Split(txt, 3);

                switch (parts.Length)
                {
                    // Found a value (for the last parameter 
                    // found (space separator))
                    case 1:
                        if (parameter != null)
                        {
                            if (!parameters.ContainsKey(parameter))
                            {
                                parts[0] =
                                    remover.Replace(parts[0], "$1");

                                parameters.Add(parameter, parts[0]);
                            }
                            parameter = null;
                        }
                        // else Error: no parameter waiting for a value (skipped)
                        break;

                    // Found just a parameter
                    case 2:
                        // The last parameter is still waiting. 
                        // With no value, set it to true.
                        if (parameter != null)
                        {
                            if (!parameters.ContainsKey(parameter))
                                parameters.Add(parameter, "true");
                        }
                        parameter = parts[1];
                        break;

                    // Parameter with enclosed value
                    case 3:
                        // The last parameter is still waiting. 
                        // With no value, set it to true.
                        if (parameter != null)
                        {
                            if (!parameters.ContainsKey(parameter))
                                parameters.Add(parameter, "true");
                        }

                        parameter = parts[1];

                        // Remove possible enclosing characters (",')
                        if (!parameters.ContainsKey(parameter))
                        {
                            parts[2] = remover.Replace(parts[2], "$1");
                            parameters.Add(parameter, parts[2]);
                        }

                        parameter = null;
                        break;
                }
            }
            // In case a parameter is still waiting
            if (parameter != null)
            {
                if (!parameters.ContainsKey(parameter))
                    parameters.Add(parameter, "true");
            }
        }

        // Retrieve a parameter value if it exists 
        // (overriding C# indexer property)
        public string this[string param]
        {
            get
            {
                return (parameters[param]);
            }
        }
    }
}
