using FakeUserDataGenerator.Models;
using FakeUserDataGenerator.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Globalization;

namespace FakeUserDataGenerator.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakeDataController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(ParametersForFake parameters)
        {
            var users = GetUserData(parameters);

            return Ok(users);
        }

        [HttpGet("download")]
        public async Task<IActionResult> Download(ParametersForFake parameters, [FromServices] ICsvService service)
        {
            var users = GetUserDataForCsv(parameters);
            var report = await service.GenerateCsv(users);

            return File(report.Content, report.Type, report.Filename);
        }

        private IEnumerable<User> GetUserData(ParametersForFake parameters)
        {
            SetLocale(parameters);
            parameters.Errors = Math.Min(1000, parameters.Errors);
            var users = FakeDataService.GetFakeData(parameters);
            AddErrorsToData(parameters, users);

            return users;
        }

        private IEnumerable<User> GetUserDataForCsv(ParametersForFake parameters)
        {
            var data = new List<User>();
            var totalPages = parameters.Page;
            for (var i = 0; i <= totalPages; i++)
            {
                parameters.Page = i;
                data.AddRange(GetUserData(parameters));
            }

            return data;
        }

        private static void AddErrorsToData(ParametersForFake parameters, IEnumerable<User> users)
        {
            var errorsService = new ErrorsService(parameters.Seed, parameters.Locale);
            errorsService.CorruptData(users, parameters.Errors);
        }

        private static void SetLocale(ParametersForFake parameters)
        {
            var allowedLocales = new string[] { "en_US", "pl", "ru" };
            parameters.Locale = allowedLocales.Contains(parameters.Locale) ? parameters.Locale : "en_US";
        }
    }
}
