using System.Collections.Generic;
using UnityEngine;
using System;
using FfalconXR;
namespace RayNeo
{

    //用于处理Native回调结果并非在主线程执行会引起的问题
    public class MainThreadQueue : MonoSingleton<MainThreadQueue>
    {
        public Queue<Action> ExecuteQueue = new Queue<Action>();
        private Action CurrentAction;

        private void Update()
        {
            try
            {
                while (ExecuteQueue.Count > 0)
                {
                    //主线程队列执行
                    CurrentAction = ExecuteQueue.Dequeue();
                    CurrentAction.Invoke();
                }
            }
            catch (Exception e)
            {
                Log.Error("[RayNeoX2]:MethodName:" , CurrentAction.Method.Name , "MainThreadQueue error:" , e);
            }
        }
    }
}
