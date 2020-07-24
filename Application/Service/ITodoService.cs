using System.Threading.Tasks;
using CleanArchitecture.Application.Models.Requests;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Service
{
    public interface ITodoService
    {
        Task<bool> CreateTodo(TodoCreateRequest todo);
    }
}