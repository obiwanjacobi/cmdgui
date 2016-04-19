using CannedBytes.CommandLineGui.Schema.Version1;
using CannedBytes.CommandLineGui.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CannedBytes.CommandLineGui.Model.Factory;

namespace CannedBytes.CommandLineGui.UnitTest
{
    /// <summary>
    ///This is a test class for BindingModelFactoryForFileVersion1Test and is intended
    ///to contain all BindingModelFactoryForFileVersion1Test Unit Tests
    ///</summary>
    [TestClass()]
    public class BindingModelFactoryForSchemaVersion1Test
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

        #endregion Additional test attributes

        [TestMethod()]
        [DeploymentItem(@"TestFiles\TestFile1.xml")]
        public void CreateBindingModelTest()
        {
            var toolConfig = XmlSerializer<CommandLineGuiConfig>.Deserialize("TestFile1.xml");

            Assert.IsNotNull(toolConfig, "Deserialize returned null.");
            Assert.IsTrue(toolConfig.Executables.Count > 0, "No executable configuration found.");

            var factory = new BindingModelFactoryForSchemaVersion1();
            var toolBindingModel = factory.Create(toolConfig.Executables[0]);

            Assert.IsNotNull(toolBindingModel, "Factory returned null.");
        }

        [TestMethod()]
        [DeploymentItem(@"TestFiles\svcutil.exe.gui")]
        public void CreateBindingModel_SvcUtilTest()
        {
            var toolConfig = XmlSerializer<CommandLineGuiConfig>.Deserialize("svcutil.exe.gui");

            Assert.IsNotNull(toolConfig, "Deserialize returned null.");
            Assert.IsTrue(toolConfig.Executables.Count > 0, "No executable configuration found.");

            var factory = new BindingModelFactoryForSchemaVersion1();
            var toolBindingModel = factory.Create(toolConfig.Executables[0]);

            Assert.IsNotNull(toolBindingModel, "Factory returned null.");
        }
    }
}