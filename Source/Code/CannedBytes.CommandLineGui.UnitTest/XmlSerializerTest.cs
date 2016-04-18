using System.IO;
using CannedBytes.CommandLineGui.Configuration.Version1;
using CannedBytes.CommandLineGui.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CannedBytes.CommandLineGui.UnitTest
{
    /// <summary>
    ///This is a test class for XmlSerializerTest and is intended
    ///to contain all XmlSerializerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class XmlSerializerTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes

        //
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion

        [TestMethod()]
        [DeploymentItem(@"TestFiles\TestFile1.xml")]
        public void DeserializeTest()
        {
            var toolConfig = XmlSerializer<CommandLineGuiConfig>.Deserialize("TestFile1.xml");

            Assert.IsNotNull(toolConfig, "Deserialize returned null.");

            Assert.IsTrue(toolConfig.Executables.Count > 0, "No exectuable configuration found.");
            var executable = toolConfig.Executables[0];

            Assert.AreEqual("fake.exe", executable.Name, "Executable name mismatch");
            Assert.AreEqual("/relative folder/fake.exe", executable.Location, "Executable location mismatch");
            Assert.AreEqual("/?", executable.HelpCmd, "Executable helpCmd mismatch");

            Assert.IsTrue(executable.Arguments.Count > 0, "No Argument configuration found.");
            var arg = executable.Arguments[0];

            Assert.IsNotNull(arg, "argument ref is null");
            Assert.AreEqual("arg1", arg.Name, "Argument name mismatch");
            Assert.AreEqual("/a1", arg.Format, "Argument format mismatch");
            Assert.AreEqual(Multiplicity.ExactlyOne, arg.Multiplicity, "Argument multiplicity mismatch");
        }

        [TestMethod]
        public void SerializeTest()
        {
            var toolConfig = new CommandLineGuiConfig()
            {
                Executables = new ExecutableList
                {
                    new Executable()
                    {
                        Name = "Exec1",
                        Location = "/relative/Exec1.exe",
                        Arguments = new ArgumentList()
                        {
                            new Argument()
                            {
                                Name = "Test1",
                                Format = "/t",
                                Multiplicity = Multiplicity.ExactlyOne
                            }
                        }
                    }
                }
            };

            using (var memStream = new MemoryStream())
            {
                XmlSerializer<CommandLineGuiConfig>.Serialize(toolConfig, memStream);

                memStream.Position = 0;
                var reader = new StreamReader(memStream);
                TestContext.WriteLine("{0}", reader.ReadToEnd());
            }
        }

        [TestMethod()]
        [DeploymentItem(@"TestFiles\svcutil.exe.gui")]
        public void DeserializeSvcUtilTest()
        {
            var toolConfig = XmlSerializer<CommandLineGuiConfig>.Deserialize("svcutil.exe.gui");

            Assert.IsNotNull(toolConfig, "Deserialize returned null.");
        }
    }
}