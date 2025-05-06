using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Constants
{
    public static class ErrorCodes
    {
        public const int BadRequest = StatusCodes.Status400BadRequest;
        public const int Ok = StatusCodes.Status200OK;
    }
}
