using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashDrawers
{
    public abstract class CashDrawer
    {
        public abstract Task InitAsync();
        public abstract Task OpenAsync();
        public abstract Task<CashDrawerStatus> GetStatusAsync();

        public event CashDrawerStatusEventHandler StatusChanged;

        protected void OnStatusChanged(CashDrawerStatusEventArgs args)
        {
            if(null != this.StatusChanged)
            {
                this.StatusChanged(this, args);
            }
        }
    }

    public sealed class CashDrawerStatusEventArgs : EventArgs
    {
        public CashDrawerStatusEventArgs(CashDrawerStatus status)
        {
            this.Status = status;
        }

        public CashDrawerStatus Status { get; private set; }
    }

    public delegate void CashDrawerStatusEventHandler(object sender, CashDrawerStatusEventArgs args);
}
