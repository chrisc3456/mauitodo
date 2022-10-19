namespace MauiToDo.Data
{
    [Serializable]
    public class ToDoUpdateItem
    {
        public string TaskDescription { get; set; }
        public bool IsCompleted { get; set; }
    }
}
