using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magicodes.Tasks.Test
{
    public class TeskTaskWithException : TaskBase
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
            throw new Exception("我是一个错误！");
        }
    }
}
