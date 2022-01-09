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
                    return DalObject.DalObject.Instance;          //Needs to return DalObject type
                case "DalXml":
                    return DalXml.Instance;                             //Needs to return DalXml type
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
