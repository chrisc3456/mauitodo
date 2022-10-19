namespace MauiToDo.Data
{
    [Serializable]
    public class ToDoItem
    {
        public string Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string TaskDescription { get; set; }
        public bool IsCompleted { get; set; }
    }
}
