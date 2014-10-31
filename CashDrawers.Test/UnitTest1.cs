using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using CashDrawers.APG;
using System.Threading.Tasks;

namespace CashDrawers.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var btcd = (await BluetoothCashDrawer.FindAllAsync()).FirstOrDefault();
            await btcd.InitAsync();
            await btcd.OpenAsync();
        }
    }
}
