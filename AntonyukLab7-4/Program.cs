using System;
using System.Collections.Generic;

public class TaskScheduler<TTask, TPriority>
{
    private class TaskItem
    {
        public TTask Task { get; }
        public TPriority Priority { get; }

        public TaskItem(TTask task, TPriority priority)
        {
            Task = task;
            Priority = priority;
        }
    }

    private readonly PriorityQueue<TaskItem> taskQueue = new PriorityQueue<TaskItem>();

    public delegate void TaskExecution(TTask task);

    // Метод для додавання завдання до планувальника з вказаним пріоритетом
    public void AddTask(TTask task, TPriority priority)
    {
        TaskItem taskItem = new TaskItem(task, priority);
        taskQueue.Enqueue(taskItem, priority);
    }

    // Метод для виконання завдання з найвищим пріоритетом
    public void ExecuteNext(TaskExecution executeTask)
    {
        if (taskQueue.Count > 0)
        {
            TaskItem nextTask = taskQueue.Dequeue();
            executeTask(nextTask.Task);
        }
        else
        {
            Console.WriteLine("No tasks in the scheduler.");
        }
    }

    // Приклад методу для ініціалізації об'єкта
    public TTask InitializeTask()
    {
        // Логіка для ініціалізації нового завдання
        Console.WriteLine("Enter task description:");
        string description = Console.ReadLine();
        return (TTask)Convert.ChangeType(description, typeof(TTask));
    }

    // Приклад методу для скидання об'єкта до пулу
    public void ResetTask(TTask task)
    {
        // Логіка для скидання завдання (якщо необхідно)
        Console.WriteLine($"Task '{task}' has been reset.");
    }
}

// Простий клас для черги з пріоритетами
public class PriorityQueue<T>
{
    private SortedDictionary<object, Queue<T>> priorityQueue = new SortedDictionary<object, Queue<T>>();

    public int Count { get; private set; }

    public void Enqueue(T item, object priority)
    {
        if (!priorityQueue.TryGetValue(priority, out var queue))
        {
            queue = new Queue<T>();
            priorityQueue[priority] = queue;
        }

        queue.Enqueue(item);
        Count++;
    }

    public T Dequeue()
    {
        if (Count == 0)
        {
            throw new InvalidOperationException("Queue is empty.");
        }

        var queue = priorityQueue.First();
        var item = queue.Value.Dequeue();
        if (queue.Value.Count == 0)
        {
            priorityQueue.Remove(queue.Key);
        }
        Count--;
        return item;
    }
}

class Program
{
    static void Main()
    {
        // Приклад використання дженеричного планувальника завдань
        TaskScheduler<string, int> scheduler = new TaskScheduler<string, int>();

        while (true)
        {
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. Execute Next Task");
            Console.WriteLine("3. Exit");
            Console.Write("Choose an option: ");

            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    string taskDescription = scheduler.InitializeTask();
                    Console.Write("Enter priority for the task: ");
                    int priority = int.Parse(Console.ReadLine());
                    scheduler.AddTask(taskDescription, priority);
                    break;

                case "2":
                    scheduler.ExecuteNext(task => Console.WriteLine($"Executing task: {task}"));
                    break;

                case "3":
                    return;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
}
