using System;

namespace CannedBytes.CommandLineGui
{
    class ArgumentNotFoundException : Exception
    {
        public ArgumentNotFoundException(string bindingName, string argumentName)
            : base(BuildExceptionMessage(bindingName, argumentName))
        {
            BindingName = bindingName;
            ArgumentName = argumentName;
        }

        public string BindingName { get; private set; }

        public string ArgumentName { get; private set; }

        private static string BuildExceptionMessage(string bindingName, string argumentName)
        {
            return String.Format("Binding '{0}' refers to an argument '{1}' that can not be found.", bindingName, argumentName);
        }
    }
}