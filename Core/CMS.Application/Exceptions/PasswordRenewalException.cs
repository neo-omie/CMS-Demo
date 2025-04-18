using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Exceptions
{
    public class PasswordRenewalException : Exception
    {
        public PasswordRenewalException(string message):base(message) { }
    }
}
