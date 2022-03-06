using Microsoft.AspNetCore.Http;
using ReactShope.RequestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReactShope.Extension
{
    public static class HttpExtension { 
    
        public static void AddPaginationHeader( this HttpResponse response, MetaData metaData )
        {
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            response.Headers.Add("Pagination", JsonSerializer.Serialize(metaData,options));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");

        }
    }
}
