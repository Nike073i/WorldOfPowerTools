using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace WorldOfPowerTools.API.Test.Infrastructure.Helpers.Web.Requests
{
    public static class RequestHelper
    {
        public static async Task NotAllowedRequest(Func<Task<IActionResult>> request)
        {
            var objectResult = await InvokeRequest(request);
            CheckAwaitingStatusCode(objectResult, StatusCodes.Status405MethodNotAllowed);
        }

        public static async Task NotFoundRequest(Func<Task<IActionResult>> request)
        {
            var objectResult = await InvokeRequest(request);
            CheckAwaitingStatusCode(objectResult, StatusCodes.Status404NotFound);
        }

        public static async Task<ObjectResult> OkRequest(Func<Task<IActionResult>> request)
        {
            var objectResult = await InvokeRequest(request);
            CheckAwaitingStatusCode(objectResult, StatusCodes.Status200OK);
            return objectResult;
        }

        private static void CheckAwaitingStatusCode(ObjectResult objectResult, int statusCode)
        {
            Assert.True(objectResult!.StatusCode == statusCode);
        }

        private static async Task<ObjectResult> InvokeRequest(Func<Task<IActionResult>> request)
        {
            var response = await request();
            Assert.IsNotNull(response);
            var objectResult = response as ObjectResult;
            Assert.IsNotNull(objectResult);
            return objectResult!;
        }
    }
}
