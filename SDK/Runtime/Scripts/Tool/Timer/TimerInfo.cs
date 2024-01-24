
namespace RayNeo
{
    public class TimerInfo
    {
        //计时器任务ID
        public int TimerID;

        //执行时间，单位为毫秒
        public long Time;

        //任务事件
        public TimerTask Task;

        //执行函数
        public void Run()
        {
            Task();
        }

        public TimerInfo(int ID, long Time, TimerTask Task)
        {
            this.TimerID = ID;
            this.Time = Time;
            this.Task = Task;
        }
    }
}
