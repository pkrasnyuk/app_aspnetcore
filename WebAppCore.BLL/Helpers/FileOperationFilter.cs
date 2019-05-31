using System.Linq;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebAppCore.BLL.Helpers
{
    public class FileOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ParameterDescriptions.Any(
                x => x.ModelMetadata.ContainerType == typeof(IFormFile)))
                if (operation.Parameters != null)
                {
                    //operation.Parameters.Clear();

                    if (operation.Parameters.Any())
                    {
                        var parametersForRemove = operation.Parameters.Where(parameter => !parameter.Name.Equals("id"))
                            .ToList();
                        if (parametersForRemove.Any())
                            foreach (var parameter in parametersForRemove)
                                operation.Parameters.Remove(parameter);
                    }

                    operation.Parameters.Add(new NonBodyParameter
                    {
                        Name = "filePayload",
                        In = "formData",
                        Description = "Upload file",
                        Required = true,
                        Type = "file"
                    });

                    operation.Consumes.Add("application/form-data");
                }
        }
    }
}