﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Exceptions
{
    public class BadRequestWithErrorsException(IEnumerable<string> errors):Exception
    {
        public IEnumerable<string> Errors { get; set; } = errors;
    }
}
