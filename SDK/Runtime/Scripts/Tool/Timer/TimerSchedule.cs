using FfalconXR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RayNeo
{
    public delegate void TimerTask();//任务委托

    /// <summary>
    /// 计时器模块
    /// </summary>
    public class TimerSchedule
    {

        private static TimerSchedule instance;
        public static TimerSchedule Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TimerSchedule();
                }
                return instance;
            }
        }

        private Thread TimeThread;//计时器线程

        private Dictionary<int, TimerInfo> IDTimerDict = new Dictionary<int, TimerInfo>();  //任务ID与计时器任务字典，待执行任务字典
        private List<int> RemoveTimerList = new List<int>();                                  //任务ID移除列表，待移除任务ID

        private int Index = 0;

        public TimerSchedule()
        {
            TimeThread = new Thread(TimerStart);//开启计时器线程
            TimeThread.Start();
        }

        private void TimerStart()
        {
            while (true)
            {
                Thread.Sleep(100);//每100毫秒执行一次任务，及时将待移除任务销毁
                CallBack();
            }
        }

        //执行任务回调
        private void CallBack()
        {
            lock (RemoveTimerList)//线程锁，防止数据竞争
            {
                lock (IDTimerDict)
                {
                    for (int i = 0; i < RemoveTimerList.Count; i++)
                    {
                        IDTimerDict.Remove(RemoveTimerList[i]);
                    }
                    RemoveTimerList.Clear();

                    long m_EndTime = DateTime.Now.Ticks;                                 //获取现行时间
                    List<int> TimerDictKeys = new List<int>(IDTimerDict.Keys.ToList());  //将待执行任务ID创建列表

                    for (int i = 0; i < TimerDictKeys.Count; i++)
                    {
                        if (IDTimerDict[TimerDictKeys[i]].Time <= m_EndTime)             //待执行的时间大于等于当前时间
                        {
                            RemoveTimerList.Add(IDTimerDict[TimerDictKeys[i]].TimerID);  //将此任务添加至移除列表
                            try
                            {
                                IDTimerDict[TimerDictKeys[i]].Run();
                            }
                            catch (Exception e)
                            {
                                Log.Error("TimerSchedule", e);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 添加一个计时器任务
        /// </summary>
        /// <param name="Task">待执行的任务</param>
        /// <param name="Time">任务执行时间精度为毫秒</param>
        /// <returns></returns>
        public int AddSchedule(TimerTask Task, long Time)
        {
            lock (IDTimerDict)
            {
                Index++;                              //任务ID

                long NowTime = DateTime.Now.Ticks;    //根据现行时间添加任务时间
                NowTime += Time * 10000;

                TimerInfo Model = new TimerInfo(Index, NowTime, Task);//创建一个新的时间任务
                IDTimerDict.Add(Index, Model);                        //将此任务添加至任务字典
                return Index;
            }
        }

        /// <summary>
        /// 根据任务ID移除任务
        /// </summary>
        /// <param name="TaskID">待移除任务ID</param>
        /// <returns></returns>
        public bool RemoveSchedule(int TaskID)
        {
            //如果该任务已存在待移除列表，则直接返回成功
            if (RemoveTimerList.Contains(TaskID))
            {
                return true;
            }
            //如果该任务位于待执行任务字典，则将该任务添加至移除列表，返回成功
            if (IDTimerDict.ContainsKey(TaskID))
            {
                RemoveTimerList.Add(TaskID);
                return true;
            }
            return false;
        }


        /// <summary>
        /// 清除所有任务
        /// </summary>
        /// <returns></returns>
        public bool RemoveAllSchedule()
        {
            //如果该任务位于待执行任务字典，则将该任务添加至移除列表，返回成功
            if (IDTimerDict.Count > 0)
            {
                RemoveTimerList.AddRange(IDTimerDict.Keys.ToList());
                return true;
            }
            return false;
        }
    }
}
