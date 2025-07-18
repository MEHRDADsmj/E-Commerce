﻿using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id);
    Task<User> GetByEmailAsync(string email);
    Task AddAsync(User user);
}