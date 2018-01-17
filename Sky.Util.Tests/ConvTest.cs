using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sky.Util.Tests
{
    /// <summary>
    /// 类型转换公共操作类测试
    /// </summary>
    [TestClass]
    public class ConvTest
    {
        /// <summary>
        /// 测试转换为整形
        /// </summary>
        [TestMethod]
        public void TestToInt()
        {
            Assert.AreEqual(1,Util.Conv.ToInt("1"));
        }
    }
}
