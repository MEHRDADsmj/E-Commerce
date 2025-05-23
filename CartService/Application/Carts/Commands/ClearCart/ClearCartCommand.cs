﻿using MediatR;
using Shared.Data;

namespace CartService.Application.Carts.Commands.ClearCart;

public record ClearCartCommand(Guid UserId) : IRequest<Result<bool>>;