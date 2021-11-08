
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;

namespace IBL
{
    public class BL: IBL
    {
        public BL()
        {
            IDal Data = new DalObject.DalObject();
            Double[] BatteryUsed = Data.GetBatteryUsed();

        }
    }
}
