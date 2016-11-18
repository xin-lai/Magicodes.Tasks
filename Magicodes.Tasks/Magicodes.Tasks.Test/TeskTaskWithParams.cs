using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magicodes.Tasks.Test
{
    public class TeskTaskWithParams : TaskBase
    {
        public override void Run()
        {
            Logger.Log(Magicodes.Logger.LoggerLevels.Debug, "Test,Param:" + this.Parameter);
        }
    }
}
