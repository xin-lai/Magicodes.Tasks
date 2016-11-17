using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magicodes.Tasks.Test
{
    public class TeskTask : TaskBase
    {
        public override string Title
        {
            get
            {
                return "Test";
            }
        }

        public override void Run()
        {
            Logger.Log(Magicodes.Logger.LoggerLevels.Debug, "Test");
        }
    }
}
