﻿using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Products.Commands.AddProduct;
using ProductService.Application.Products.Commands.DeleteProduct;
using ProductService.Application.Products.Commands.UpdateProduct;
using ProductService.Application.Products.Queries.GetBulk;
using ProductService.Application.Products.Queries.GetProductById;
using ProductService.Application.Products.Queries.GetProductsPaginated;
using ProductService.Domain.Entities;
using ProductService.Presentation.DTOs;

namespace ProductService.Presentation.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet("health")]
    public IActionResult GetHealth()
    {
        return Ok("Products Healthy");
    }

    [HttpPut("add")]
    public async Task<ActionResult<AddProductResponseDto>> AddProduct([FromBody] AddProductRequestDto dto)
    {
        var command = new AddProductCommand(dto.Name, dto.UnitPrice, dto.Description);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            var resp = new AddProductResponseDto(result.Value!.Id, result.Value.Name, result.Value.UnitPrice);
            return CreatedAtAction("AddProduct", resp);
        }
        return BadRequest(result.ErrorMessage);
    }

    [HttpPatch("update")]
    public async Task<ActionResult<UpdateProductResponseDto>> UpdateProduct([FromBody] UpdateProductRequestDto dto)
    {
        var productDto = new ProductDto(dto.Id, dto.Name, dto.UnitPrice, dto.Description);
        var command = new UpdateProductCommand(productDto);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            var resp = new UpdateProductResponseDto(result.Value!.Id, result.Value.Name, result.Value.UnitPrice,
                                                    result.Value.Description);
            return Ok(resp);
        }
        return BadRequest(result.ErrorMessage);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteProduct([FromBody] DeleteProductRequestDto dto)
    {
        var command = new DeleteProductCommand(dto.Id);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return NoContent();
        }
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet()]
    public async Task<IActionResult> GetAllProducts([FromQuery, Range(1, int.MaxValue)] int page = 1, 
                                                    [FromQuery(Name = "page_size"), Range(1, int.MaxValue)] int pageSize = 10)
    {
        var query = new GetProductsPaginatedQuery(page, pageSize);
        var result = await _mediator.Send(query);

        if (result.IsSuccess)
        {
            var resp = new GetProductsPaginatedResponseDto(result.Value);
            return Ok(resp);
        }
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetProductsPaginatedResponseDto>> GetProductById([FromRoute] Guid id)
    {
        var query = new GetProductByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result.IsSuccess)
        {
            var resp = new GetProductByIdResponseDto(result.Value!);
            return Ok(resp);
        }
        return BadRequest(result.ErrorMessage);
    }

    [HttpPost("bulk")]
    public async Task<ActionResult<GetProductsBulkResponseDto>> GetProductsBulk([FromBody] GetProductsBulkRequestDto dto)
    {
        var query = new GetProductsBulkQuery(dto.ProductIds);
        var result = await _mediator.Send(query);

        if (result.IsSuccess)
        {
            var resp = new GetProductsBulkResponseDto(result.Value!);
            return Ok(resp);
        }
        return BadRequest(result.ErrorMessage);
    }
}