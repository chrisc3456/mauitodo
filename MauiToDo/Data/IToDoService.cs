namespace MauiToDo.Data
{
    public interface IToDoService
    {
        public Task<List<ToDoItem>> GetToDoList();
        public Task<ToDoItem> GetToDoItem(string id);
        public Task<ToDoItem> DeleteToDoItem(string id);
        public Task<ToDoItem> CreateToDoItem(ToDoCreateItem toDoCreateItem);
        public Task<ToDoItem> UpdateToDoItem(string id, ToDoUpdateItem toDoUpdateItem);
    }
}
