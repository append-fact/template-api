using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions
{
    public class NotFoundException : ApiException
    {
        public NotFoundException() : base("El recurso solicitado no fue encontrado.", (int)HttpStatusCode.NotFound) { }

        public NotFoundException(string message) : base(message, (int)HttpStatusCode.NotFound) { }

        public NotFoundException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args), (int)HttpStatusCode.NotFound) { }
    }
}
