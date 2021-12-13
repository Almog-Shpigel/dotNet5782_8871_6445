using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BO.EnumsBL;

    namespace BO
    {
        public class ParcelToList
        {
            public int ID { get; set; }
            public string SenderName { get; set; }
            public string TargetName { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public ParcelStatus Status { get; set; }

            public ParcelToList(int iD = 0, string senderName = "", string targetName = "", WeightCategories weight = WeightCategories.Light, Priorities priority = Priorities.Regular, ParcelStatus status = ParcelStatus.Delivered)
            {
                ID = iD;
                SenderName = senderName;
                TargetName = targetName;
                Weight = weight;
                Priority = priority;
                Status = status;
            }
            public override string ToString()
            {
                return $"Parcel #{ID}:\n" +
                    $"Properties: {Weight}, {Priority}, {Status}. " +
                    $"Sent by {SenderName} to {TargetName}";
            }
        }
    }

