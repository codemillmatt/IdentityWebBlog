using System.Threading.Tasks;
using System.Collections.Generic;

namespace TodoListApp 
{
    public interface IForecastService    
    {
        Task<string> GetForecast(string relativeEndpoint = "", string[] requiredScopes = null);        
    }
}