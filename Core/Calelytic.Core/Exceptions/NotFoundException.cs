﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calelytic.Core.Exceptions
{
    [Serializable]
    internal class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string? message) : base(message)
        {
        }

        public NotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}

