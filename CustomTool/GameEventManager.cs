using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

[AttributeUsage(AttributeTargets.Method)]
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
            var attribute = method.GetCustomAttribute<ListenerAttribute>();
            if (attribute != null)
            {
                if (method.GetParameters().Length != 0)
                {
                    throw new Exception($"事件 {attribute.EventName} 的處理函數 {method.Name} 不能有參數！");
                }

                // 檢查事件名稱是否已經註冊過
                if (!eventHandlers.ContainsKey(attribute.EventName))
                {
                    eventHandlers[attribute.EventName] = null;
                }

                // 檢查是否已有相同事件的處理器，避免重複註冊
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
        }
        else
        {
            Console.WriteLine($"[警告] 事件 '{eventName}' 沒有被監聽");
        }
    }
    public void ClearAllListeners()
    {
        eventHandlers.Clear();
        Console.WriteLine("[信息] 所有事件處理器已清空！");
    }

}