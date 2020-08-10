using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TodoListApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IForecastService _forecastService;

        public IndexModel(ILogger<IndexModel> logger, IForecastService forecastService)
        {
            _logger = logger;

            _forecastService = forecastService;
        }

        public async Task OnGet()
        {
            ViewData["ApiResult"] = await _forecastService.GetForecast();
        }
    }
}
