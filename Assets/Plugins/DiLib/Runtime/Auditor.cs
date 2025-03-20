using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Feverfew.DiLib
{
    public static class Auditor
    {
        public static void EnableLogging(bool enable)
        {
            DiContextAudit.Logging = enable;
        }
    }
}
