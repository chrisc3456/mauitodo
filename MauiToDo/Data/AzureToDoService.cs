using Newtonsoft.Json;
using System.Net.Http.Json;

namespace MauiToDo.Data
{
    public class AzureToDoService : IToDoService
    {
        private static string _baseAddress = "https://exampletodolist.azurewebsites.net/api";
        private static HttpClient _httpClient;

        // TODO: Look into whether this should be refactored - not sure we should create a new client for every request, investigate performance implications
        // Also should we check response.EnsureSuccessStatusCode to check for errors?
        private static HttpClient GetClient()
        {
            if (_httpClient != null)
                return _httpClient;

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return _httpClient;
        }

        /**
         * GET /todo/
         * Retrieves a list of all existing to-do items
         */
        public async Task<List<ToDoItem>> GetToDoList()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return new List<ToDoItem>();

            HttpClient client = GetClient();
            string result = await client.GetStringAsync($"{_baseAddress}/todo");
            return JsonConvert.DeserializeObject<List<ToDoItem>>(result);
        }

        /**
         * GET /todo/{id}
         * Retrieves a specific to-do item matching the ID provided
         */
        public async Task<ToDoItem> GetToDoItem(string id)
        {
            throw new NotImplementedException();
        }

        /**
         * POST /todo/
         * Stores a new to-do item
         */
        public async Task<ToDoItem> CreateToDoItem(ToDoCreateItem toDoCreateItem)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            var msg = new HttpRequestMessage(HttpMethod.Post, $"{_baseAddress}/todo");
            msg.Content = JsonContent.Create(toDoCreateItem);

            HttpClient client = GetClient();

            var result = await client.SendAsync(msg);
            result.EnsureSuccessStatusCode();

            var returnedData = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ToDoItem>(returnedData);
        }

        /**
         * DELETE /todo/{id}
         * Deletes an existing to-do item
         */
        public async Task<ToDoItem> DeleteToDoItem(string id)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            var msg = new HttpRequestMessage(HttpMethod.Delete, $"{_baseAddress}/todo/{id}");

            HttpClient client = GetClient();

            var result = await client.SendAsync(msg);
            result.EnsureSuccessStatusCode();

            var returnedData = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ToDoItem>(returnedData);
        }

        /**
         * PUT /todo/{id}
         * Updates the description and/or completed status of an existing to-do item
         */
        public async Task<ToDoItem> UpdateToDoItem(string id, ToDoUpdateItem toDoUpdateItem)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            var msg = new HttpRequestMessage(HttpMethod.Put, $"{_baseAddress}/todo/{id}");
            msg.Content = JsonContent.Create(toDoUpdateItem);

            HttpClient client = GetClient();

            var result = await client.SendAsync(msg);
            result.EnsureSuccessStatusCode();

            var returnedData = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ToDoItem>(returnedData);
        }
    }
}
