﻿using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SoftwarepartDetector.Attributes;
using SoftwarepartDetector.DataModel;

namespace SoftwarepartDetector.Detectors;
public class ControllersAndActionDetector
{
    private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

    public ControllersAndActionDetector(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
    {
        _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
    }

    public Task<List<SoftwarePartController>> Detect()
    {
        return Task.Run(() =>
        {
            var result = new List<SoftwarePartController>();

            var controllerActionDescriptors = _actionDescriptorCollectionProvider.ActionDescriptors.Items.OfType<ControllerActionDescriptor>().ToList();

            foreach (var item in controllerActionDescriptors)
            {
                var attribute = item.ControllerTypeInfo.GetCustomAttributes(typeof(SoftwarePartControllerOptionAttribute), false).FirstOrDefault() as SoftwarePartControllerOptionAttribute;
                result.Add(new SoftwarePartController
                {
                    Module = attribute?.Module,
                    Service = attribute?.Service,
                    Name = item.ControllerName,
                    ApplicationPartType = SoftwarePartType.Controller,
                });
            }

            result = result.DistinctBy(c => c.Name).GroupJoin(controllerActionDescriptors, c => c.Name, a => a.ControllerName, (c, a) => new SoftwarePartController
            {
                Module = c.Module,
                Service = c.Service,
                Name = c.Name,
                ApplicationPartType = SoftwarePartType.Controller,
                Actions = a.Select(b => new SoftwarePartAction
                {
                    Name = b.ActionName,
                    ApplicationPartType = SoftwarePartType.Action
                }).DistinctBy(c => c.Name).ToList()
            }).ToList();

            return result;
        });
    }
}
