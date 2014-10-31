using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.HumanInterfaceDevice;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;

namespace CashDrawers.APG
{
    public class UsbCashDrawer : CashDrawer
    {
        HidDevice hid;
        CashDrawerStatus status;

        public UsbCashDrawer()
        {

        }

        private async Task<HidDevice> FindCashDrawer()
        {
            //usage 241
            //page 240
            var query = HidDevice.GetDeviceSelector(240, 241, 1989, 1280);
            var devices = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(query);
            var device = devices.FirstOrDefault();
            if (null == device)
            {
                return null;
            }

            var hid = await HidDevice.FromIdAsync(device.Id, Windows.Storage.FileAccessMode.ReadWrite);
            if (null == hid)
            {
                return null;
            }

            Debug.WriteLine("usage " + hid.UsageId);
            Debug.WriteLine("page" + hid.UsagePage);

            hid.InputReportReceived += hid_InputReportReceived;
            
            return hid;
        }

        public override async Task InitAsync()
        {
            this.hid = await this.FindCashDrawer();
        }

        void hid_InputReportReceived(HidDevice sender, HidInputReportReceivedEventArgs args)
        {
            Debug.WriteLine("Hid Input report received");
            var data = args.Report.Data.ToArray();
            var oldStatus = this.status;
            this.status = (CashDrawerStatus)data[1];
            Debug.WriteLine("Status = {0}", this.status);

            if (this.status != oldStatus)
            {
                this.OnStatusChanged(new CashDrawerStatusEventArgs(this.status));
            }
        }

        public async override Task OpenAsync()
        {
            if (null == this.hid)
            {
                this.hid = await this.FindCashDrawer();
            }

            if (null == this.hid)
            {
                throw new Exception("CashDrawer not found");
            }

            var output = hid.CreateOutputReport();
            var stream = output.Data.AsStream();

            await stream.WriteAsync(new byte[] { 0, 1, 1 }, 0, 3);
            var resp = await hid.SendOutputReportAsync(output);
        }

        public override async Task<CashDrawerStatus> GetStatusAsync()
        {
            if (null == this.hid)
            {
                this.hid = await this.FindCashDrawer();
            }

            if (null == this.hid)
            {
                return CashDrawerStatus.Unknown;
            }

            var output = hid.CreateOutputReport();
            var stream = output.Data.AsStream();

            await stream.WriteAsync(new byte[] { 0, 0, 0 }, 0, 3);
            var resp = await hid.SendOutputReportAsync(output);
            var input = await hid.GetInputReportAsync();
            var statusBytes = input.Data.ToArray(0, 3);

            return (CashDrawerStatus) statusBytes[1];
        }
    }
}
