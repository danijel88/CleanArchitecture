using System.Threading.Tasks;
using CleanArchitecture.Application.Core;
using CleanArchitecture.Application.Models.Requests;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Service
{
    public class TodoService : BaseService<Todo>, ITodoService
    {
        private readonly IBaseServiceProvider<Todo> _baseServiceProvider;

        public TodoService(IBaseServiceProvider<Todo> baseServiceProvider) : base(baseServiceProvider)
        {
            _baseServiceProvider = baseServiceProvider;
        }

        public async Task<bool> CreateTodo(TodoCreateRequest todo)
        {
            Todo entity = new Todo();
            entity.Name = todo.Name;
            await Create(entity);
            return true;
        }
    }
}