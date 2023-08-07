﻿using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace CursoWebApi.Utilidades
{
    public class SwaggerAgrupaPorVersion : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var namespaceControlador = controller.ControllerType.Namespace; //V1
            var versionAPI = namespaceControlador .Split ('.').Last().ToLower (); //V1

            controller .ApiExplorer .GroupName = versionAPI;
        }
    }
}
