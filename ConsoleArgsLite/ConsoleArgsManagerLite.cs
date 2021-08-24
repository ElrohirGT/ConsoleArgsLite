using System;
using System.Collections.Generic;

namespace ConsoleArgsLite
{
    public class ConsoleArgsManagerLite
    {
        private Dictionary<string, ConsoleArg<object>> _arguments = new Dictionary<string, ConsoleArg<object>>();

        /// <summary>
        /// Adds a string argument.
        /// </summary>
        /// <param name="name">The name of the argument, this value must be unique.</param>
        /// <param name="required">Whether or not the argument is required.</param>
        /// <param name="consoleName">The name of the argument in the console (do not include --).</param>
        public void AddArgument(string name, bool required = true, string consoleName = null) => AddArgument(name, required, (s) => s, consoleName);

        /// <summary>
        /// Adds an argument of type <typeparamref name="T"/>.
        /// It is recommended that you use nullable types for value types like integers.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="name">The name of the argument, this value must be unique.</param>
        /// <param name="required">Whether or not the value is required.</param>
        /// <param name="parser">The converter from the string value from the cmd to a type T.</param>
        /// <param name="consoleName">The name of the argument in the console (do not include --).</param>
        public void AddArgument<T>(string name, bool required, Func<string, T> parser, string consoleName = null)
        {
            Func<string, object> converter = (s) => parser(s);
            _arguments.Add(name, new ConsoleArg<object>(name, converter, required, consoleName));
        }

        /// <summary>
        /// Attemps the parsed value of an argument.
        /// It is recommended that you use nullable types for value types like integers.
        /// </summary>
        /// <typeparam name="T">The type of the value to get.</typeparam>
        /// <param name="name">The name of the argument to get.</param>
        /// <returns>The parsed value or null if it has not been parsed yet.</returns>
        public T GetValueFromArgument<T>(string name) => (T) _arguments[name].Value;

        public void ParseArguments(string[] args)
        {
            int currentArgsIndex = 0;
            foreach (var argument in _arguments.Values)
            {
                if (currentArgsIndex < args.Length)
                    ParseArgument(argument, args, ref currentArgsIndex);
                else if (argument.Required)
                    throw new ArgumentNullException(argument.Name);
            }
        }

        private static void ParseArgument(ConsoleArg<object> argument, string[] args, ref int currentIndex)
        {
            if (string.IsNullOrEmpty(argument.ConsoleName))
            {
                if (TryParsingArgument(args[currentIndex], argument))
                    ++currentIndex;
            }
            else if ($"--{argument.ConsoleName}" == args[currentIndex])
            {
                int valueIndex = currentIndex + 1;
                if (valueIndex >= args.Length && argument.Required)
                    throw new ArgumentNullException(argument.Name);

                if (TryParsingArgument(args[valueIndex], argument))
                    currentIndex = valueIndex + 1;
            }
        }

        /// <summary>
        /// Tries to parse an argument from the supplied consoleArgument.
        /// </summary>
        /// <param name="consoleArgument">The string supplied in the commandline.</param>
        /// <param name="argument">The argument object.</param>
        /// <returns>True if it succesfully parsed the object, false otherwise.</returns>
        private static bool TryParsingArgument(string consoleArgument, ConsoleArg<object> argument)
        {
            try
            {
                bool consoleArgIsAFlag = consoleArgument.StartsWith("--");
                if (consoleArgIsAFlag)
                {
                    if (argument.Required)
                        throw new ArgumentNullException(argument.Name);
                    else
                        return false;
                }
                argument.Parse(consoleArgument);
                return true;
            }
            catch (ArgumentNullException) { throw; }
            catch (Exception e)
            {
                throw new ArgumentException($"Couldn't parse the {argument.Name} argument", argument.Name, e);
            }
        }
    }
}