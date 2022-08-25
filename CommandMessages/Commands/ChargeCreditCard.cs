using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI
{
    public class ChargeCreditCard :ICommand
    {
        public string OrderId { get; set; }
        public string CreditCardNumber { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
