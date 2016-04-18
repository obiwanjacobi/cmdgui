using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace CannedBytes.CommandLineGui.UnitTest
{
    [TestClass]
    public class RegexTest
    {
        [TestMethod]
        public void ParseArgumentValueFormatTest()
        {
            var regex = new Regex(@"{[0-9]");

            var test0 = "NoHits";
            var test1 = "/o:{0}";
            var test2 = "/n:{0},{1}";

            var matches = regex.Matches(test0);
            Assert.AreEqual(0, matches.Count);
            matches = regex.Matches(test1);
            Assert.AreEqual(1, matches.Count);
            matches = regex.Matches(test2);
            Assert.AreEqual(2, matches.Count);
        }
    }
}
