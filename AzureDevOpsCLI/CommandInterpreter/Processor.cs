using System;
using CSharpTest.Net.Commands;

namespace VSTSTool.CommandInterpreter
{
    public static class Processor
    {
        public static int Process(string[] args, ICommands commands)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands));
            }

            var commandInterpreter =
                new CSharpTest.Net.Commands.CommandInterpreter(DefaultCommands.Help, commands)
                {
                    /* Sometimes ErrorLevel still has a value other than zero when testing.
                     Maybe because this is a static class and a static member?
                     Force it here.
                    */
                    ErrorLevel = 0
                };

            commandInterpreter.Run(args);

            return commandInterpreter.ErrorLevel;
        }
    }
}