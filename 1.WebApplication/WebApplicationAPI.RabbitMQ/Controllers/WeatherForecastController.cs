using Application.Layer.DataAccess.ChangeDataLog;
using Extensions.Caching.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI.Controllers.Bases;

namespace WebApplicationAPI.RabbitMQ.Controllers;
public class RabbitMQController : BaseController
{
    public RabbitMQController(DatabaseContext context) : base(context)
    {
    }

    [HttpPost]
    public ActionResult Create()
    {
        return Ok();
    }

    [HttpPut]
    public ActionResult Update()
    {
        return Ok();
    }

    [HttpDelete]
    public ActionResult Delete()
    {
        return Ok();
    }
    [HttpGet]
    public ActionResult Get()
    {
        return Ok();
    }
}
