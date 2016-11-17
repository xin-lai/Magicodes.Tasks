using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Magicodes.Logger.DebugLogger;
using Magicodes.Tasks.Builder;

namespace Magicodes.Tasks.Test
{
    [TestClass]
    public class TaskUnitTest
    {
        [TestMethod]
        public void Start_Test()
        {
            var taskManager =
                TaskBuilder.Create()
                //设置日志记录
                .WithLoggers(new DebugLogger("TaskManager"), new DebugLogger("Task"))
                //设置通知器
                .WithNotifier(new TestNotifier())
                .Build();

            taskManager.Start(nameof(TeskTask));
        }
    }
}
