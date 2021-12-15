using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;

namespace BL
{
    public static class BlFactory
    {
       public static IBL GetBl()
        {
            return BlApi.BL.GetBL();
        }
    }
}
