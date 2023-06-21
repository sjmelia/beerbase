using Microsoft.AspNetCore.Mvc;

namespace Beerbase.Tests
{
    internal static class ActionResultExtensions
    {
        public static T AssertAndGetOkValue<T>(this ActionResult<T> actionResult) where T : class
        {
            var okResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okResult);
            var value = okResult.Value as T;
            Assert.NotNull(value);
            return value;
        }
    }
}
