using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Models;
namespace Service
{
    public class TodoClient
    {
        readonly HttpClient _client;
        readonly string BaseUrl = "https://jsonplaceholder.typicode.com/todos";

        public TodoClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<Todo>> GetTodosAsync()
        {
            try
            {
                var response = await _client.GetAsync($"{BaseUrl}");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<Todo>>(responseBody);
            }
            catch (HttpRequestException)
            {

                return null;
            }
        }

        public async Task<Todo> GetTodoAsync(int id)
        {
            try
            {
                var response = await _client.GetAsync($"{BaseUrl}/{id}");
                response.EnsureSuccessStatusCode();
                var responsebody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Todo>(responsebody);
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<Todo> PostTodoAsync(Todo todo)
        {
            try
            {
                var todoJson = JsonConvert.SerializeObject(todo);
                var content = new StringContent(todoJson.ToString(), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync($"{BaseUrl}", content);
                response.EnsureSuccessStatusCode();
                var responsebody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Todo>(responsebody);
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<Todo> PutTodoAsync(int id, Todo todo)
        {
            try
            {
                var todoJson = JsonConvert.SerializeObject(todo);
                var content = new StringContent(todoJson.ToString(), Encoding.UTF8, "application/json");
                var response = await _client.PutAsync($"{BaseUrl}/{id}", content);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Todo>(responseBody);
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<bool> DeleteTodoAsync(int id)
        {
            try
            {
                var response = await _client.DeleteAsync($"{BaseUrl}/{id}");
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
    }
}
