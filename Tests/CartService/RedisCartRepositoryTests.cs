﻿using System.Data;
using CartService.Domain.Entities;
using CartService.Infrastructure.Repositories;
using FluentAssertions;
using StackExchange.Redis;
using Testcontainers.Redis;

namespace Tests.CartService;

public class RedisCartRepositoryTests : IAsyncLifetime
{
    private RedisContainer _redisContainer;
    private IConnectionMultiplexer _connectionMultiplexer;
    private RedisCartRepository _repository;
    
    public async Task InitializeAsync()
    {
        _redisContainer = new RedisBuilder().WithImage("redis:6").WithCleanUp(true).Build();
        await _redisContainer.StartAsync();

        _connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(_redisContainer.GetConnectionString());
        _repository = new RedisCartRepository(_connectionMultiplexer);
    }

    [Fact]
    public async Task SaveAndGetAsync_ShouldPersistCart()
    {
        var userId = Guid.NewGuid();
        var cart = new Cart(userId, new List<CartItem>());
        cart.Items.Add(new CartItem(Guid.NewGuid(), 2));
        
        await _repository.SaveAsync(cart);
        var result = await _repository.GetAsync(userId);
        
        result.Should().NotBeNull();
        result.UserId.Should().Be(userId);
        result.Items.Should().BeEquivalentTo(cart.Items);
    }

    [Fact]
    public async Task RemoveItemAsync_ShouldRemoveSpecificItem()
    {
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var cart = new Cart(userId, new List<CartItem>());
        cart.Items.Add(new CartItem(productId, 1));
        cart.Items.Add(new CartItem(Guid.NewGuid(), 3));
        
        await _repository.SaveAsync(cart);
        await _repository.RemoveItemAsync(userId, productId);
        
        var result = await _repository.GetAsync(userId);
        result.Should().NotBeNull();
        result.UserId.Should().Be(userId);
        result.Items.Should().NotContain(item => item.ProductId == productId);
    }

    [Fact]
    public async Task ClearAsync_ShouldClearCart()
    {
        var userId = Guid.NewGuid();
        var cart = new Cart(userId, new List<CartItem>());
        cart.Items.Add(new CartItem(Guid.NewGuid(), 2));
        
        await _repository.SaveAsync(cart);
        await _repository.ClearCartAsync(userId);
        
        var result = await _repository.GetAsync(userId);
        result.Should().NotBeNull();
        result.UserId.Should().Be(userId);
        result.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateCartAsync_ShouldPersistCart()
    {
        var userId = Guid.NewGuid();
        var cart = await _repository.CreateCartAsync(userId);
        cart.Should().NotBeNull();
        cart.UserId.Should().Be(userId);
        cart.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateCartAsync_ShouldPersistCart()
    {
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var cart = new Cart(userId, new List<CartItem>());
        cart.Items.Add(new CartItem(productId, 2));
        
        await _repository.SaveAsync(cart);
        await _repository.UpdateItemQuantityAsync(userId, productId, 5);
        
        var result = await _repository.GetAsync(userId);
        result.Should().NotBeNull();
        result.UserId.Should().Be(userId);
        result.Items.Should().Contain(item => item.ProductId == productId && item.Quantity == 5);
    }

    [Fact]
    public async Task AddItemAsync_NewItem_ShouldPersistCart()
    {
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var cart = new Cart(userId, new List<CartItem>());

        await _repository.SaveAsync(cart);
        await _repository.AddItemAsync(userId, productId, 5);
        
        var result = await _repository.GetAsync(userId);
        result.Should().NotBeNull();
        result.UserId.Should().Be(userId);
        result.Items.Should().Contain(item => item.ProductId == productId && item.Quantity == 5);
    }

    [Fact]
    public async Task AddItemAsync_ExistingItem_ShouldPersistCart()
    {
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var cart = new Cart(userId, new List<CartItem>());
        cart.Items.Add(new CartItem(productId, 2));
        
        await _repository.SaveAsync(cart);
        try
        {
            await _repository.AddItemAsync(userId, productId, 5);
            var result = await _repository.GetAsync(userId);
            Assert.Fail("Did not throw exception");
        }
        catch (Exception ex)
        {
            ex.Should().BeOfType<DuplicateNameException>();
            cart.Items.Should().Contain(item => item.ProductId == productId && item.Quantity == 2);
        }
    }

    public Task DisposeAsync()
    {
        return _redisContainer.DisposeAsync().AsTask();
    }
}