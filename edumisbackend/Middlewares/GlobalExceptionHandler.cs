using edumis.DataAccess.IRepositories;
using edumis.Models.Global;
using edumisbackend.Common;
using System.Net;
using System.Text.Json;

namespace edumisbackend.Middlewares
{
    public class GlobalExceptionHandler : IMiddleware
    {
        private readonly IHostEnvironment environment;
        private readonly ILogger Logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;

        public GlobalExceptionHandler(IHostEnvironment environment, ILogger<GlobalExceptionHandler> logger, IUnitOfWork UnitOfWork, IConfiguration configuration)
        {
            this.environment = environment;
            Logger = logger;
            unitOfWork = UnitOfWork;
            this.configuration = configuration;
        }       

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
               
                string logFilePath = Path.Combine(environment.ContentRootPath, configuration["LogFilePath"]);

                edumis.Common.Utilities.LogExcetionDetailsToTextFile(ex, logFilePath);

                //ProblemDetails problemDetails = new()
                //{
                //    Detail = (environment.IsProduction() ? "An internal server error has occured." : ex.Message +
                //                (ex.InnerException != null ? ("; Inner Exception: " + ex.InnerException.Message) : "")),
                //    Type = "Server Error",
                //    Title = "Internal Server Error",
                //    Status = (int)HttpStatusCode.InternalServerError
                //};
                //var responseMsg = JsonSerializer.Serialize(problemDetails);
                //context.Response.ContentType = "application/json";
                //await context.Response.WriteAsync(responseMsg);

                //ResponseModel model = new ResponseModel()
                //{
                //    Message = "Some Error Occured!",
                //    ReturnCode = ((int) HttpStatusCode.BadRequest).ToString(),
                //    Success = false,
                //    ReturnId = string.Empty
                //};

                //Write Log to DB
                ExceptionLogs exceptionLogs = new ExceptionLogs()
                {
                    Origin = ex.Source,
                    ErrorMessage = ex.Message,
                    InnerMessage = ex.InnerException != null ? ex.InnerException.Message : string.Empty,
                    StackTrace = ex.StackTrace,
                    CreatedDate = DateTime.UtcNow
                };
                await unitOfWork.ExceptionHandler.Add(exceptionLogs);
                await unitOfWork.Save();

                var returnResponseModel = ResponseModel<string>.ServerError("Some Error Occured!");

                var responseMsg = JsonSerializer.Serialize(returnResponseModel);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(responseMsg);               
            }
        }
    }
}
