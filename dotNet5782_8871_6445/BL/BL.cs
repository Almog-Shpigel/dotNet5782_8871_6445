
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;

namespace BL
{
    public class BL: IBL.IBL
    {
        public BL()
        {
            IDal Data = new DalObject.DalObject();
            Double[] BatteryUsed = Data.GetBatteryUsed();

        }
    }
}
