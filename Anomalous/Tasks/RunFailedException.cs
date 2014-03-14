using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnomalousMSBuild.Tasks
{
    class RunFailedException : Exception
    {
        public RunFailedException(String message)
            :base(message)
        {

        }
    }
}
