using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CannedBytes.CommandLineGui.Persistence;
using CannedBytes.CommandLineGui.Schema.Version1;

namespace CannedBytes.CommandLineGui.Schema
{
    class GuiSchemaManager
    {
        public GuiSchemaManager()
        {
            GuiSchemas = new List<GuiSchema>();
        }

        public List<GuiSchema> GuiSchemas { get; private set; }

        public void AddSchemas(string basePath)
        {
            var files = Directory.GetFiles(basePath, "*.gui", SearchOption.AllDirectories);

            foreach (var file in files)
            {
#if DEBUG
                if (Debugger.IsAttached)
                {
                    AddFromFile(file);
                    continue;
                }
#endif
                try
                {
                    AddFromFile(file);
                }
                catch (Exception e)
                {
                    // this code executes at app startup
                    // missing some schema files is no big deal.

                    Trace.WriteLine(String.Format("The file '{0}' could not be loaded.\nAn error occurred during loading: {1}", file, e.ToString()), "GuiDefinition");
                }
            }
        }

        public GuiSchema AddFromFile(string schemaFilePath)
        {
            // check if the schema already exists.
            var existingSchema = (from schema in this.GuiSchemas
                                  where schema.SchemaFilePath == schemaFilePath
                                  select schema).FirstOrDefault();

            if (existingSchema != null)
            {
                return existingSchema;
            }

            CommandLineGuiConfig toolConfig = ReadToolSchema(schemaFilePath);

            //var existingSchema = (from schema in GuiSchemas
            //                      where schema.SchemaFilePath.ToUpper() == schemaFilePath.ToUpper()
            //                      select schema).FirstOrDefault();

            //if (existingSchema != null)
            //{
            //    var newExecs = toolConfig.Executables.Except(existingSchema.ToolConfig.Executables);
            //    var existingExecs = toolConfig.Executables.Intersect(existingSchema.ToolConfig.Executables);

            //    foreach (var exec in existingExecs)
            //    {
            //        var existingExec = existingSchema.FindExecutable(exec.Name);
            //        var newGroups = exec.Gui.BindingGroupsAndBindings.Except(existingExec.Gui.BindingGroupsAndBindings);

            //        var bindingList = new List<object>(existingExec.Gui.BindingGroupsAndBindings);
            //        bindingList.AddRange(newGroups);

            //        existingExec.Gui.BindingGroupsAndBindings = bindingList;
            //    }

            //    existingSchema.ToolConfig.Executables.AddRange(newExecs);

            //    return existingSchema;
            //}
            //else
            {
                var guiSchema = new GuiSchema();
                guiSchema.SchemaFilePath = schemaFilePath;
                guiSchema.ToolConfig = toolConfig;

                GuiSchemas.Add(guiSchema);

                return guiSchema;
            }
        }

        private static CommandLineGuiConfig ReadToolSchema(string schemaFilePath)
        {
            var serializer = XmlSerializer<CommandLineGuiConfig>.CreateFromResource(
                SchemaNames.CommandLineGuiSchemaV1);

            CommandLineGuiConfig toolConfig = serializer.Read(schemaFilePath);

            return toolConfig;
        }

        public GuiSchema FindByExecutableName(string toolName)
        {
            return (from schema in GuiSchemas
                    from exec in schema.ToolConfig.Executables
                    where exec.Name == toolName
                    select schema).FirstOrDefault();
        }

        public IEnumerable<Executable> AllExecutables
        {
            get
            {
                return (from schema in GuiSchemas
                        from exec in schema.ToolConfig.Executables
                        select exec);
            }
        }
    }
}