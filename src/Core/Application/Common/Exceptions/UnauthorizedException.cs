using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions
{
    public class UnauthorizedException : ApiException
    {
        public UnauthorizedException() : base("Usuario no autenticado.", (int)HttpStatusCode.Unauthorized) { }

        public UnauthorizedException(string message) : base(message, (int)HttpStatusCode.Unauthorized) { }

        public UnauthorizedException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args), (int)HttpStatusCode.Unauthorized) { }
    }
}
