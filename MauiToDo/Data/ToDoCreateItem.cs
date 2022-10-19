namespace MauiToDo.Data
{
    [Serializable]
    public class ToDoCreateItem
    {
        public string TaskDescription { get; set; }

        public ToDoCreateItem(string taskDescription)
        {
            TaskDescription = taskDescription;
        }
    }
}
