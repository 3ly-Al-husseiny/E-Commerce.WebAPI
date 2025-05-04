using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class DeliveryMethodNotFoundException(int id) : Exception($"The DeliveryMethod With Id: {id} is Not Found")
    {
    }
}
