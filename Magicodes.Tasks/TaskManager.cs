using Magicodes.Logger;
using Magicodes.Notify;
using Magicodes.Tasks.Help;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Magicodes.Tasks
{
    public class TaskManager
    {
        /// <summary>
        ///     日志
        /// </summary>
        public LoggerBase TaskManagerLogger { get; set; }

        /// <summary>
        ///     通知器
        /// </summary>
        public INotifier Notifier { get; set; }

        /// <summary>
        ///     任务日志记录器
        /// </summary>
        public LoggerBase TaskLogger { get; set; }

        /// <summary>
        ///     任务类型集合
        /// </summary>

        protected ConcurrentDictionary<string, Type> TaskTypes =
           new ConcurrentDictionary<string, Type>();

        /// <summary>
        /// 任务执行
        /// </summary>
        public Action<TaskBase> TaskRunAction = (instance) => Task.Run(() => instance.Run());

        /// <summary>
        /// 初始化执行
        /// </summary>
        public Action<TaskManager> InitAction = (taskManager) => Task.Run(() => {
            foreach (
                    var currentassembly in
                    AppDomain.CurrentDomain.GetAssemblies().Where(p => p.FullName.StartsWith("Magicodes")))
                try
                {
                    var baseType = typeof(TaskBase);
                    currentassembly.GetTypes()
                        .Where(p => p.IsClass && p.IsSubclassOf(baseType))
                        .Each(t =>
                        {
                            var instance = (TaskBase)Activator.CreateInstance(t);
                            if (instance == null)
                            {
                                taskManager.TaskManagerLogger.LogFormat(LoggerLevels.Error,
                                    "CreateInstance 失败！ AssemblyFullName:{0}\tFullName:{1}", t.Assembly.FullName,
                                    t.FullName);
                            }
                            else
                            {
                                var key = string.IsNullOrEmpty(instance.Keyword) ? t.Name : instance.Keyword;
                                taskManager.TaskTypes.TryAdd(key, t);
                                taskManager.TaskManagerLogger.LogFormat(LoggerLevels.Debug,
                                    "TaskTypes Add Key:{0}\tFullName:{1}", key,
                                    t.FullName);
                            }
                        });
                }
                catch (ReflectionTypeLoadException ex)
                {
                    taskManager.TaskManagerLogger.Log(LoggerLevels.Error, ex);
                    if (ex.LoaderExceptions.Length <= 0) return;
                    foreach (var loaderEx in ex.LoaderExceptions)
                        taskManager.TaskManagerLogger.Log(LoggerLevels.Error, loaderEx);
                }
                catch (Exception ex)
                {
                    taskManager.TaskManagerLogger.Log(LoggerLevels.Error, ex);
                }
        });

        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="keyword">任务Key</param>
        /// <param name="parameter">参数</param>
        /// <param name="notifyInfo">通知参数</param>
        public virtual void Start(string keyword, object parameter = null, INotifyInfo notifyInfo = null)
        {
            if (TaskTypes.ContainsKey(keyword))
            {
                var type = TaskTypes[keyword];
                var instance = (TaskBase)Activator.CreateInstance(type);
                instance.Logger = TaskLogger;
                instance.Notifier = Notifier;
                instance.NotifyInfo = notifyInfo;
                //当通知标题没有赋值时，使用Task Title
                if (string.IsNullOrEmpty(instance.NotifyInfo.Title))
                    instance.NotifyInfo.Title = instance.Title;

                instance.Parameter = parameter;
                //执行
                TaskRunAction.Invoke(instance);
            }
            throw new KeyNotFoundException(keyword + " 不存在！");
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init()
        {
            InitAction.Invoke(this);
        }
    }
}
