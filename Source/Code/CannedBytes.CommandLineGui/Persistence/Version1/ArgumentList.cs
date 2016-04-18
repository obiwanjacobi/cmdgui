using System.Collections.Generic;

namespace CannedBytes.CommandLineGui.Persistence.Version1
{
    public class ArgumentList : List<Argument>
    {
        public ArgumentList()
        { }

        public ArgumentList(IEnumerable<Argument> arguments)
            : base(arguments)
        { }
    }
}