﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Exceptions
{
    public class UserNotFoundException(string email):NotFoundException($"User with {email} Not Found")
    {
    }
}
