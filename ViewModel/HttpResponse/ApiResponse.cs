using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.HttpResponse
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Data = data,
                Errors = null
            };
        }

        public static ApiResponse<T> FailResponse(List<string> errors, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Data = default,
                Errors = errors
            };
        }

        public static ApiResponse<T> FailResponse(string error, int statusCode = 400)
        {
            return FailResponse(new List<string> { error }, statusCode);
        }
    }

}
