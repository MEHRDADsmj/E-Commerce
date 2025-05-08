using UserService.Infrastructure;

namespace Tests.Infrastructure;

public class BcryptPasswordHasherTests
{
    private BCryptPasswordHasher _bcryptPasswordHasher;

    public BcryptPasswordHasherTests()
    {
        _bcryptPasswordHasher = new BCryptPasswordHasher();
    }

    [Fact]
    public async Task HashPassword_VerifyHashedPassword_ReturnsTrue()
    {
        var password = "password";
        var result = await _bcryptPasswordHasher.HashPassword(password);
        Assert.True(await _bcryptPasswordHasher.VerifyHash(result, password));
    }
}