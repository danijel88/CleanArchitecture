using System.Threading.Tasks;
using CleanArchitecture.Application.Models.Requests;
using CleanArchitecture.Application.Service;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class TodosController : WebApiControllerBase
    {
        private readonly ITodoService _service;

        public TodosController(ITodoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(TodoCreateRequest todo)
        {
            await _service.CreateTodo(todo);
            return Ok();
        }
    }
}