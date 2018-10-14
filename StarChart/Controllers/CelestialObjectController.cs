using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var obj = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            obj.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == id).ToList();
            return Ok(obj);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var objs = _context.CelestialObjects.Where(x => x.Name == name).ToList();
            if (objs.Any() == false)
            {
                return NotFound();
            }C:\Users\anguzy\Documents\GitHub\AspNetCore-StarChartAPI\StarChart\Controllers\CelestialObjectController.cs

            foreach (var obj in objs)
            {
                obj.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == obj.Id).ToList();

            }
            return Ok(objs);
        }

        [HttpGet()]
        public IActionResult GetAll()
        {
            var list = _context.CelestialObjects.ToList();
            foreach (var item in list)
            {
                item.Satellites = list.Where(x => x.OrbitedObjectId == item.Id).ToList();
            }
            return Ok(list);
        }
    }
}
