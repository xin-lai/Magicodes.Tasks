using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Magicodes.Logger.DebugLogger;
using Magicodes.Tasks.Builder;

namespace Magicodes.Tasks.Test
{
    [TestClass]
    public class TaskUnitTest
    {
        public static int flag = 0;
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

        [TestMethod]
        public void Start_WithParams_Test()
        {
            var taskManager =
                TaskBuilder.Create()
                //设置日志记录
                .WithLoggers(new DebugLogger("TaskManager"), new DebugLogger("Task"))
                //设置通知器
                .WithNotifier(new TestNotifier())
                .Build();

            taskManager.Start(nameof(TeskTaskWithParams), 1);
        }

        [TestMethod]
        public void Start_WithTaskCompleteAction_Test()
        {
            var taskManager =
                TaskBuilder.Create()
                //设置日志记录
                .WithLoggers(new DebugLogger("TaskManager"), new DebugLogger("Task"))
                //设置通知器
                .WithNotifier(new TestNotifier())
                .WithTaskCompleteAction((task) =>
                {
                    flag = 1;
                    new DebugLogger("Debug").Log(Logger.LoggerLevels.Debug, "Test——WithTaskCompleteAction");
                })
                .Build();

            taskManager.Start(nameof(TeskTaskWithParams), 1);
            System.Threading.Thread.Sleep(1500);
            Assert.IsTrue(flag == 1);
            
        }

        [TestMethod]
        public void Start_WithException_Test()
        {
            var taskManager =
                TaskBuilder.Create()
                //设置日志记录
                .WithLoggers(new DebugLogger("TaskManager"), new DebugLogger("Task"))
                //设置通知器
                .WithNotifier(new TestNotifier())
                .WithTaskCompleteAction((task) =>
                {
                    flag = 1;
                    Assert.IsNotNull(task.Exception);
                    new DebugLogger("Debug").Log(Logger.LoggerLevels.Error, task.Exception.Message);
                })
                .Build();

            taskManager.Start(nameof(TeskTaskWithException));
            System.Threading.Thread.Sleep(1500);
            Assert.IsTrue(flag == 1);
        }
    }
}
