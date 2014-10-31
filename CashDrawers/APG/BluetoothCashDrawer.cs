using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using System.Diagnostics;
using Windows.Devices.Enumeration;

namespace CashDrawers.APG
{
    public class BluetoothCashDrawer : CashDrawer
    {
        public static async Task<IEnumerable<CashDrawer>> FindAllAsync()
        {
            List<CashDrawer> drawers = new List<CashDrawer>();
            var devices = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(RfcommDeviceService.GetDeviceSelector(RfcommServiceId.SerialPort));
            foreach (var device in devices.Where(x => x.Name == "APG CashDrawers"))
            {
                drawers.Add(new BluetoothCashDrawer(device));
            }

            return drawers;
        }

        public BluetoothCashDrawer(DeviceInformation device)
        {
            if (null == device)
            {
                throw new ArgumentNullException("device");
            }

            this.device = device;
        }

        private DeviceInformation device;
        private StreamSocket socket;

        protected DataReader Reader { get; private set; }
        protected DataWriter Writer { get; private set; }
        public override async Task InitAsync()
        {
            try
            {
                RfcommDeviceService rfcomm = null;
                await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
                {
                    rfcomm = RfcommDeviceService.FromIdAsync(this.device.Id).GetResults();
                });

                if (rfcomm == null)
                {
                    throw new Exception("Permission not granted");
                }

                this.socket = new StreamSocket();
                await socket.ConnectAsync(rfcomm.ConnectionHostName, rfcomm.ConnectionServiceName);
                this.Writer = new DataWriter(socket.OutputStream);
                this.Reader = new DataReader(socket.InputStream);

            }
            catch (Exception)
            {
                throw;
            }

        }

        public override async Task OpenAsync()
        {
            if (null == this.socket)
            {
                await this.InitAsync();
            }

            //writer.WriteBytes(Encoding.UTF8.GetBytes( Commands.Device));
            //await writer.FlushAsync();

            //var s = reader.ReadString(2);
            //
            Writer.WriteBytes(Encoding.UTF8.GetBytes(Commands.Open));
            //writer.WriteBytes(BinaryCommands.Open);
            await Writer.FlushAsync();


        }

        public async override Task<CashDrawerStatus> GetStatusAsync()
        {
            return CashDrawerStatus.Unknown;
        }
    }
}
