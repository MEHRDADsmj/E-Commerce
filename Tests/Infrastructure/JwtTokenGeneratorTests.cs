﻿using UserService.Infrastructure;

namespace Tests.Infrastructure;

public class JwtTokenGeneratorTests
{
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public JwtTokenGeneratorTests()
    {
        _jwtTokenGenerator = new JwtTokenGenerator();
    }

    [Fact]
    public async Task GenerateToken_GeneratesValidToken()
    {
        var key = "alskfjlaskf9w8r8ewr987asldkfjauweoriuoweiru";
        var token = await _jwtTokenGenerator.GenerateToken(key, Guid.NewGuid());
        Assert.True(await _jwtTokenGenerator.ValidateToken(token, key));
    }
}