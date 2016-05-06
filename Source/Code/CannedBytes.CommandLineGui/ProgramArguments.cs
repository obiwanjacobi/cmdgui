using System;
using System.Linq;

namespace CannedBytes.CommandLineGui
{
    /// <summary>
    /// Command line arguments for this application.
    /// </summary>
    class ProgramArguments
    {
        /// <summary>
        /// ctor
        /// </summary>
        public ProgramArguments()
        {
            var args = Environment.GetCommandLineArgs();
            Parse(args, true);
        }

        /// <summary>
        /// ctor with args
        /// </summary>
        /// <param name="args">From Main.</param>
        public ProgramArguments(string[] args)
        {
            Parse(args, false);
        }

        private void Parse(string[] args, bool containsSelf)
        {
            if (args == null)
            {
                IsEmpty = true;
                return;
            }

            var cmdArgs = containsSelf ? args.Skip(1) : args;

            if (cmdArgs.Any())
            {
                FileName = cmdArgs.First();
            }
            else
            {
                IsEmpty = true;
            }
        }

        /// <summary>
        /// A file name was specified to load.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// If true, no args were found
        /// </summary>
        public bool IsEmpty { get; private set; }
    }
}
