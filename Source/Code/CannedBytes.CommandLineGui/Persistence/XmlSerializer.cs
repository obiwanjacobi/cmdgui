using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;

namespace CannedBytes.CommandLineGui.Persistence
{
    class XmlSerializer<T> where T : class
    {
        private XmlSchemaSet _schemas = new XmlSchemaSet();

        public static XmlSerializer<T> CreateFromResource(string schemaResourcePath)
        {
            var serializer = new XmlSerializer<T>();

            var assmebly = Assembly.GetExecutingAssembly();
            var stream = assmebly.GetManifestResourceStream(schemaResourcePath);

            serializer.InitializeSchema(stream);

            return serializer;
        }

        private void InitializeSchema(Stream stream)
        {
            var schema = XmlSchema.Read(stream, new ValidationEventHandler(ValidateXmlSchema));

            _schemas.Add(schema);
            _schemas.Compile();
        }

        private void ValidateXmlSchema(object sender, ValidationEventArgs e)
        {
            if (e.Exception != null)
            {
                throw e.Exception;
            }

            if (e.Severity == XmlSeverityType.Error)
            {
                throw new Exception(e.Message);
            }
        }

        private XmlReader CreateValidatingReader(Stream input)
        {
            var settings = new XmlReaderSettings()
            {
                Schemas = _schemas,
                ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings,
                ValidationType = ValidationType.Schema
            };

            return XmlReader.Create(input, settings);
        }

        public T Read(string filePath)
        {
            T obj = null;

            ErrorHandler(filePath, () =>
                {
                    using (var stream = File.OpenRead(filePath))
                    {
                        obj = Read(stream);
                    }
                });

            return obj;
        }

        public T Read(Stream input)
        {
            var reader = CreateValidatingReader(input);

            return Deserialize(reader);
        }

        //---------------------------------------------------------------------
        // static interface
        //---------------------------------------------------------------------

        private static readonly System.Xml.Serialization.XmlSerializer _serializer =
            new System.Xml.Serialization.XmlSerializer(typeof(T));

        private static void ErrorHandler(string filePath, Action action)
        {
            try
            {
                action();
            }
            catch (XmlException xe)
            {
                throw new InvalidDataException(
                    string.Format("The file '{0}' was not in the correct format.", filePath), xe);
            }
            catch (InvalidDataException ide)
            {
                throw new InvalidDataException(
                    string.Format("The file '{0}' was not in the correct format.", filePath), ide);
            }
        }

        public static T Deserialize(string filePath)
        {
            T obj = null;

            ErrorHandler(filePath, () =>
                {
                    using (FileStream stream = File.Open(
                        filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        obj = Deserialize(stream);
                    }
                });

            return obj;
        }

        public static T Deserialize(Stream stream)
        {
            XmlReader reader = System.Xml.XmlReader.Create(stream);

            return Deserialize(reader);
        }

        public static T Deserialize(XmlReader reader)
        {
            if (!_serializer.CanDeserialize(reader))
            {
                throw new InvalidDataException("Failed to read Xml.");
            }

            return (T)_serializer.Deserialize(reader);
        }

        public static void Serialize(T root, string filePath)
        {
            using (FileStream stream = File.Open(
                filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Serialize(root, stream);
            }
        }

        public static void Serialize(T root, Stream stream)
        {
            _serializer.Serialize(stream, root);
        }
    }
}