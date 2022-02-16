using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Linq;

namespace ECom.API.Utilities
{
    public class GroupControllerByVersion : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var controllerNamespace = controller.ControllerType.Namespace; //ECom.API.Controllers.OData.v1
            var apiVersion = controllerNamespace?.Split('.').Last().ToLower();
            controller.ApiExplorer.GroupName = apiVersion;
        }
    }
}
