using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NXKit.Util;

namespace NXKit.Tests
{

    [TestClass]
    public class NXDocumentHostTests
    {

        [TestMethod]
        public void Test_basic_load()
        {
            using (var uri = DynamicUriUtil.GetUriFor(@"<unknown />"))
            {
                var d1 = NXDocumentHost.Load(uri);
            }
        }

    }

}
