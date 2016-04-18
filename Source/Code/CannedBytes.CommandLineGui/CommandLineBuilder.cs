using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CannedBytes.CommandLineGui.Model;
using CannedBytes.CommandLineGui.Schema.Version1;

namespace CannedBytes.CommandLineGui
{
    /// <summary>
    /// Build the command line based on the entered values by the user.
    /// </summary>
    /// <remarks>The command line does not include the location of the executable.</remarks>
    class CommandLineBuilder : BindingModelNavigator
    {
        private const string Separator = " ";

        private Executable _executable;
        private CommandLineContext _ctx;

        /// <summary>
        /// Constructs a new instance for the specified <paramref name="executable"/>.
        /// </summary>
        /// <param name="executable">Must not be null.</param>
        public CommandLineBuilder(Executable executable)
        {
            _executable = executable;
        }

        /// <summary>
        /// Builds the command line.
        /// </summary>
        /// <param name="bindingModel">Must not be null.</param>
        /// <returns></returns>
        public string Build(GroupBindingModel bindingModel)
        {
            _ctx = new CommandLineContext();

            NavigateGroup(bindingModel);

            var commandLine = Compile(_ctx);

            _ctx = null;

            return commandLine;
        }

        /// <summary>
        /// Compiles the <paramref name="ctx"/> into a command line.
        /// </summary>
        /// <param name="ctx">Must not be null.</param>
        /// <returns>Never returns null.</returns>
        private string Compile(CommandLineContext ctx)
        {
            var sorted = ctx.AsSorted();
            var builder = new StringBuilder();

            foreach (var argValue in sorted)
            {
                if (builder.Length > 0)
                {
                    builder.Append(Separator);
                }

                switch (argValue.ArgumentEntity.ValueCount)
                {
                    case 0:
                        builder.Append(argValue.Argument.Format);
                        break;
                    case 1:
                        builder.AppendFormat(argValue.Argument.Format, argValue.Value.Value);
                        break;
                    default:
                        var values = (from val in argValue.Values
                                      select val.Value).ToArray();
                        builder.AppendFormat(argValue.Argument.Format, values);
                        break;
                }
            }

            return builder.ToString();
        }

        protected override void OnNavigateNonValueBindingModel(ValueBindingModel valueBindingModel)
        {
            var arg = FindArgument(valueBindingModel.Argument.Name);

            _ctx.Add(arg, valueBindingModel.Argument);
        }

        protected override void OnNavigateSingleValueBindingModel(ValueBindingModel valueBindingModel, ValueEntity valueEntity)
        {
            var arg = FindArgument(valueBindingModel.Argument.Name);

            _ctx.Add(arg, valueBindingModel.Argument, valueEntity);
        }

        protected override void OnNavigateMultiValueBindingModel(ValueBindingModel valueBindingModel, IEnumerable<ValueEntity> values)
        {
            var arg = FindArgument(valueBindingModel.Argument.Name);

            _ctx.Add(arg, valueBindingModel.Argument, values);
        }

        private void FillGroup(GroupBindingModel bindingModel, CommandLineContext ctx)
        {
            var optionsGroupBindingModel = bindingModel as OptionsGroupBindingModel;

            if (optionsGroupBindingModel != null)
            {
                Fill(optionsGroupBindingModel.SelectedOption, ctx);
            }
            else
            {
                foreach (var binding in bindingModel.Bindings)
                {
                    Fill(binding, ctx);
                }
            }
        }

        private void FillValue(ValueBindingModel bindingModel, CommandLineContext ctx)
        {
            var singleValueBindingModel = bindingModel as SingleValueBindingModel;
            var multiValueBindingModel = bindingModel as MultiValueBindingModel;
            var optionsValueBindingModel = bindingModel as OptionsValueBindingModel;
            var repeaterBindingModel = bindingModel as RepeatingValueBindingModel;

#if DEBUG
            if (singleValueBindingModel == null &&
                multiValueBindingModel == null &&
                optionsValueBindingModel == null &&
                repeaterBindingModel == null)
            {
                Trace.WriteLine("Warning: Skipping binding model: " + bindingModel.Name);
            }
#endif

            var arg = FindArgument(bindingModel.Argument.Name);

            switch (bindingModel.Argument.ValueCount)
            {
                case 0:
                    {
                        if (singleValueBindingModel != null &&
                            singleValueBindingModel.Value.HasValue)
                        {
                            ctx.Add(arg, bindingModel.Argument);
                        }
                    }
                    break;
                case 1:
                    {
                        if (singleValueBindingModel != null &&
                            singleValueBindingModel.Value.HasValue)
                        {
                            ctx.Add(arg, bindingModel.Argument, singleValueBindingModel.Value);
                        }

                        if (optionsValueBindingModel != null &&
                            optionsValueBindingModel.SelectedOption != null &&
                            optionsValueBindingModel.SelectedOption.HasValue)
                        {
                            ctx.Add(arg, bindingModel.Argument, optionsValueBindingModel.SelectedOption);
                        }
                    }
                    break;
                default:
                    {
                        if (multiValueBindingModel != null)
                        {
                            var values = from binding in multiValueBindingModel.Bindings
                                         where binding.Value.HasValue == true
                                         select binding.Value;

                            ctx.Add(arg, bindingModel.Argument, values);
                        }
                    }
                    break;
            }

            if (repeaterBindingModel != null)
            {
                foreach (var vbm in repeaterBindingModel.Bindings)
                {
                    FillValue(vbm, ctx);
                }
            }
        }

        private void Fill(BindingModel bindingModel, CommandLineContext ctx)
        {
            var groupBindingModel = bindingModel as GroupBindingModel;
            var valueBindingModel = bindingModel as ValueBindingModel;

            if (groupBindingModel != null)
            {
                FillGroup(groupBindingModel, ctx);
            }

            if (valueBindingModel != null)
            {
                FillValue(valueBindingModel, ctx);
            }
        }

        private Argument FindArgument(string argumentName)
        {
            return (from arg in _executable.Arguments
                    where arg.Name == argumentName
                    select arg).FirstOrDefault();
        }

        //---------------------------------------------------------------------

        private class CommandLineContext
        {
            public CommandLineContext()
            {
                ArgumentValues = new List<ArgumentValue>();
            }

            public List<ArgumentValue> ArgumentValues { get; private set; }

            public ArgumentValue Add(Argument argument, ArgumentEntity argumentEntity)
            {
                var argValue = new ArgumentValue(argument, argumentEntity);

                ArgumentValues.Add(argValue);

                return argValue;
            }

            public ArgumentValue Add(Argument argument, ArgumentEntity argumentEntity, ValueEntity value)
            {
                if (argument == null || argumentEntity == null || value == null || !value.HasValue) return null;

                var argValue = new ArgumentValue(argument, argumentEntity);
                argValue.Value = value;

                ArgumentValues.Add(argValue);

                return argValue;
            }

            public ArgumentValue Add(Argument argument, ArgumentEntity argumentEntity, IEnumerable<ValueEntity> values)
            {
                if (argument == null || argumentEntity == null || values == null || !values.Any()) return null;

                var argValue = new ArgumentValue(argument, argumentEntity);
                var valueList = new List<ValueEntity>(values);

                while (valueList.Count < argumentEntity.ValueCount)
                {
                    valueList.Add(ValueEntity.Empty);
                }

                argValue.Values = valueList;

                ArgumentValues.Add(argValue);

                return argValue;
            }

            public IEnumerable<ArgumentValue> AsSorted()
            {
                var priority = from argValue in ArgumentValues
                               where argValue.Argument.OrdinalSpecified
                               orderby argValue.Argument.Ordnial ascending
                               select argValue;

                // NOTE: ordinals not specified are placed last in collection
                var unspecified = from argValue in ArgumentValues
                                  where !argValue.Argument.OrdinalSpecified
                                  select argValue;

                return priority.Concat(unspecified);
            }
        }

        private class ArgumentValue
        {
            public ArgumentValue(Argument argument, ArgumentEntity argumentEntity)
            {
                Argument = argument;
                ArgumentEntity = argumentEntity;
            }

            public Argument Argument;

            public ArgumentEntity ArgumentEntity;

            public ValueEntity Value;

            public List<ValueEntity> Values;
        }
    }
}