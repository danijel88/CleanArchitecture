using System;
using CleanArchitecture.Application.Core;

namespace WebApi.Models
{
    public class CurrentUser : ICurrentUser
    {
        public bool IsAuthenticated { get; set; }
        public Guid UserId { get; set; }
    }
}