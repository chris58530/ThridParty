using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
public class ListenerAttribute : Attribute
{
    public string EventName { get; }

    public ListenerAttribute(string eventName)
    {
        EventName = eventName;
    }
}

public class Listener
{
    private Dictionary<string, Action> eventHandlers = new Dictionary<string, Action>();

    public void RegisterListener(object listener)
    {


        var methods = listener.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        foreach (var method in methods)
        {
            var attributes = method.GetCustomAttributes<ListenerAttribute>(true);
            foreach (var attribute in attributes)
            {
                if (method.GetParameters().Length != 0)
                {
                    throw new Exception($"事件 {attribute.EventName} 的處理函數 {method.Name} 不能有參數！");
                }

                if (!eventHandlers.ContainsKey(attribute.EventName))
                {
                    eventHandlers[attribute.EventName] = null;
                }

                Action handler = () => method.Invoke(listener, null);
                if (eventHandlers[attribute.EventName] == null || !eventHandlers[attribute.EventName].GetInvocationList().Contains(handler))
                {
                    eventHandlers[attribute.EventName] += handler;
                }
                else
                {
                    Console.WriteLine($"[警告] 事件 '{attribute.EventName}' 的處理函數 {method.Name} 已經註冊過了！");
                }
            }

        }

    }

    public void BroadCast(string eventName)
    {

        if (eventHandlers.TryGetValue(eventName, out var handler) && handler != null)
        {
            handler.Invoke();
            LogService.Instance.Log($"Invoke {eventName}");

        }
        else
        {
            string str = ($"[警告] 事件 '{eventName}' 沒有被監聽");
            LogService.Instance.Log($" {str}");

        }
    }
    public void ClearAllListeners()
    {
        eventHandlers.Clear();
        Console.WriteLine("[信息] 所有事件處理器已清空！");
    }

}