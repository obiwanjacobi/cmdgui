using CannedBytes.CommandLineGui.Model.Factory;
using CannedBytes.CommandLineGui.Persistence;
using CannedBytes.CommandLineGui.Properties;
using CannedBytes.CommandLineGui.Schema;
using CannedBytes.CommandLineGui.Schema.Version1;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CannedBytes.CommandLineGui
{
    /// <summary>
    /// Manages a collection of <see cref="GuiDocument"/> instances.
    /// </summary>
    class GuiDocumentManager : ObservableObject
    {
        private readonly GuiSchemaManager _schemaMgr = new GuiSchemaManager();

        /// <summary>
        /// Default ctor.
        /// </summary>
        public GuiDocumentManager()
        {
            Documents = new ObservableCollection<GuiDocument>();

            InitializeSchemas();
        }

        private void InitializeSchemas()
        {
            if (Settings.Default.UserGuiDefinitionPaths != null &&
                Settings.Default.UserGuiDefinitionPaths.Count > 0)
            {
                foreach (var path in Settings.Default.UserGuiDefinitionPaths)
                {
                    _schemaMgr.AddSchemas(path);
                }
            }
            else
            {
                var entry = Assembly.GetEntryAssembly();
                var path = Path.GetDirectoryName(entry.Location);

                path = Path.Combine(path, "GuiDefinitions");

                if (Directory.Exists(path))
                {
                    _schemaMgr.AddSchemas(path);
                }
                else
                {
                    Trace.WriteLine("No .gui schema files were loaded.");
                }
            }
        }

        public GuiSchemaManager SchemaManager
        {
            get { return _schemaMgr; }
        }

        private GuiDocument _activeDocument;

        /// <summary>
        /// Can be null.
        /// </summary>
        public GuiDocument ActiveDocument
        {
            get { return _activeDocument; }
            set
            {
                _activeDocument = value;
                OnPropertyChanged("ActiveDocument");
            }
        }

        /// <summary>
        /// A collection of loaded tool gui documents.
        /// </summary>
        public ObservableCollection<GuiDocument> Documents { get; private set; }

        public GuiDocument FindByExecutableName(string toolName)
        {
            return (from doc in Documents
                    where doc.ToolInfo.Tool.Name == toolName
                    select doc).FirstOrDefault();
        }

        public int NewAll(string schemaFilePath)
        {
            var guiSchema = _schemaMgr.AddFromFile(schemaFilePath);

            return NewAll(guiSchema);
        }

        /// <summary>
        /// Opens all executable definitions inside the tool config file.
        /// </summary>
        /// <param name="guiSchema">Must not be null.</param>
        /// <returns>Returns the index of the first newly added document in the <see cref="Documents"/> collection.</returns>
        public int NewAll(GuiSchema guiSchema)
        {
            int index = Documents.Count;

            foreach (var exec in guiSchema.ToolConfig.Executables)
            {
                AddDocument(guiSchema, exec);
            }

            return index;
        }

        public GuiDocument Open(string filePath)
        {
            var reader = new FileReaderVersion1();
            reader.Read(filePath);

            GuiDocument guiDocument = null;

            var embededSchema = reader.GetEmbededToolDefinition();

            if (embededSchema != null)
            {
                guiDocument = new GuiDocument();
                guiDocument.ToolInfo = new ToolInfo(embededSchema);
            }
            else
            {
                if (!File.Exists(reader.GuiFileDocument.ToolDefinition.GuiSchemaRef))
                {
                    throw new FileNotFoundException("Cannot find Gui Definition file '" +
                        reader.GuiFileDocument.ToolDefinition.GuiSchemaRef + "'.",
                        reader.GuiFileDocument.ToolDefinition.GuiSchemaRef);
                }

                int index = NewAll(reader.GuiFileDocument.ToolDefinition.GuiSchemaRef);
                guiDocument = FindByExecutableName(reader.GuiFileDocument.ToolDefinition.Name);

                if (guiDocument == null)
                {
                    throw new InvalidDataException(
                        String.Format("The Gui Definition file '{0}' does not contain a definition for executable '{1}'.",
                            reader.GuiFileDocument.ToolDefinition.GuiSchemaRef,
                            reader.GuiFileDocument.ToolDefinition.Name));
                }
            }

            if (guiDocument != null)
            {
                if (!String.IsNullOrEmpty(reader.GuiFileDocument.ToolDefinition.Location))
                {
                    // set customized tool location
                    guiDocument.ToolInfo.ToolExecutablePath = reader.GuiFileDocument.ToolDefinition.Location;
                }

                guiDocument.DocumentFilePath = filePath;

                reader.ApplyTo(guiDocument);

                guiDocument.IsChanged = false;
            }

            return guiDocument;
        }

        public void SaveActive()
        {
            if (ActiveDocument != null && ActiveDocument.IsChanged)
            {
                Save(ActiveDocument);
            }
        }

        public void SaveAll()
        {
            var changedDocs = from doc in Documents
                              where doc.IsChanged == true
                              select doc;

            foreach (var doc in changedDocs)
            {
                Save(doc);
            }
        }

        public static void Save(GuiDocument guiDocument)
        {
            if (guiDocument == null) return;
            if (String.IsNullOrEmpty(guiDocument.DocumentFilePath)) return;

            using (var fileStream = File.OpenWrite(guiDocument.DocumentFilePath))
            {
                var writer = new FileWriterVersion1(fileStream);

                writer.Write(guiDocument);
            }

            guiDocument.IsChanged = false;
        }

        public static void SaveAs(GuiDocument guiDocument, string filePath, IFileTypeProvider provider)
        {
            using (var fileStream = File.OpenWrite(filePath))
            {
                provider.Serialize(guiDocument, fileStream);
            }
        }

        /// <summary>
        /// Creates and adds a new <see cref="GuiDocument"/> to the <see cref="Documents"/> collection.
        /// </summary>
        /// <param name="guiSchema">Can be null.</param>
        /// <param name="tool">The tool executable definition.</param>
        /// <returns>Never returns null.</returns>
        public GuiDocument AddDocument(GuiSchema guiSchema, Executable tool)
        {
            var factory = new BindingModelFactoryForSchemaVersion1();
            var doc = new GuiDocument();

            doc.GuiSchema = guiSchema;
            doc.ToolInfo = new ToolInfo(guiSchema, tool);
            doc.ToolBindingModel = factory.Create(tool);

            Documents.Add(doc);

            doc.IsChanged = false;

            return doc;
        }
    }
}