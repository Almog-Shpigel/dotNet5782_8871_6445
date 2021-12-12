using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;

namespace DAL
{
    public static class DalFactory
    {
        public static IDal GetDal(string DalType)
        {
           switch(DalType)
            {
                case "DalObject":
                    return null;        //Needs to return DalObject type
                    break;
                case "DalXml ":
                    return null;        //Needs to return DalXml type
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
