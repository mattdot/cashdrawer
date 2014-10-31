using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashDrawers
{
    public class Commands
    {
        public const string Open = "0x77A";
        public const string Enable = "0x66A";
        public const string Status = "0x55A";
        public const string Disable = "0xFFA";
        public const string Device = "0xBBA"; // Instructs BluePro Adapter to return the device information back to the host application
        public const string StatusHigh = "0xCCA";
        public const string StatusLow = "0xDDA";
        public const string RequestBoxType = "0x00A";
    }

    public class BinaryCommands
    {
        public static readonly byte[] Open = new byte[] { 0x07, 0x7A };
        public const string Enable = "0x66A";
        public const string Status = "0x55A";
        public const string Disable = "0xFFA";
        public static readonly byte[] Device = new byte []{ 0x0B, 0xBA }; // Instructs BluePro Adapter to return the device information back to the host application
        public const string StatusHigh = "0xCCA";
        public const string StatusLow = "0xDDA";
        public const string RequestBoxType = "0x00A";
    }
}
