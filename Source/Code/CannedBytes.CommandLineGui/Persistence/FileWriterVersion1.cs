using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using CannedBytes.CommandLineGui.Model;
using CannedBytes.CommandLineGui.Persistence.Version1;
using CannedBytes.CommandLineGui.Schema.Version1;

namespace CannedBytes.CommandLineGui.Persistence
{
    class FileWriterVersion1
    {
        public FileWriterVersion1(Stream output)
        {
            OutputStream = output;
        }

        public Stream OutputStream { get; private set; }

        public void Write(GuiDocument guiDocument)
        {
            var argFinder = new BindingModelArgumentFinder();
            argFinder.Navigate(guiDocument.ToolBindingModel);

            var fileDocument = CreateFileDocument(guiDocument, argFinder.Arguments);

            if (guiDocument.EmbedGuiSchema)
            {
                fileDocument.ToolDefinition.GuiSchema = CreateGuiSchema(guiDocument.ToolInfo.Tool);
            }

            XmlSerializer<GuiFileDocument>.Serialize(fileDocument, OutputStream);
        }

        private XmlElement CreateGuiSchema(Schema.Version1.Executable executable)
        {
            using (var memStream = new MemoryStream())
            {
                XmlSerializer<Executable>.Serialize(executable, memStream);

                memStream.Position = 0;

                var xmlDoc = new XmlDocument();
                xmlDoc.Load(memStream);

                return xmlDoc.DocumentElement;
            }
        }

        private GuiFileDocument CreateFileDocument(GuiDocument document, IEnumerable<Persistence.Version1.Argument> arguments)
        {
            var doc = new GuiFileDocument()
            {
                Arguments = new Persistence.Version1.ArgumentList(arguments),
                ToolDefinition = new Persistence.Version1.ToolDefinition()
                {
                    Location = document.ToolInfo.ToolExecutablePath,
                    Name = document.ToolInfo.Tool.Name,
                    GuiSchemaRef = document.GuiSchema.SchemaFilePath
                },
            };

            return doc;
        }

        private class BindingModelArgumentFinder : BindingModelNavigator
        {
            public BindingModelArgumentFinder()
            {
                Arguments = new List<Persistence.Version1.Argument>();
            }

            public List<Persistence.Version1.Argument> Arguments { get; private set; }

            protected override void OnNavigateNonValueBindingModel(ValueBindingModel valueBindingModel)
            {
                var arg = new Persistence.Version1.Argument();

                arg.Name = valueBindingModel.Argument.Name;

                Arguments.Add(arg);
            }

            protected override void OnNavigateSingleValueBindingModel(ValueBindingModel valueBindingModel, ValueEntity valueEntity)
            {
                var arg = new Persistence.Version1.Argument();

                arg.Name = valueBindingModel.Argument.Name;
                arg.Values.Add(valueEntity.ToString());

                Arguments.Add(arg);
            }

            protected override void OnNavigateMultiValueBindingModel(ValueBindingModel valueBindingModel, IEnumerable<ValueEntity> values)
            {
                var arg = new Persistence.Version1.Argument();

                arg.Name = valueBindingModel.Argument.Name;
                arg.Values.AddRange(from value in values
                                    select value.ToString());

                Arguments.Add(arg);
            }
        }
    }
}