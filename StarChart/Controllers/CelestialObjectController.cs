using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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
            }

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

        [HttpPost()]
        public IActionResult Create([FromBody]CelestialObject obj)
        {
            _context.CelestialObjects.Add(obj);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = obj.Id }, obj);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject obj)
        {
            var matched = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);
            if(matched == null)
            {
                return NotFound();
            }
            matched.Name = obj.Name;
            matched.OrbitalPeriod = obj.OrbitalPeriod;
            matched.OrbitedObjectId = obj.OrbitedObjectId;
            _context.CelestialObjects.Update(matched);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var matched = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);
            if (matched == null)
            {
                return NotFound();
            }
            matched.Name = name;
            _context.CelestialObjects.Update(matched);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var matched = _context.CelestialObjects.Where(x => x.Id == id || x.OrbitedObjectId == id).ToList();
            if(matched.Any() == false)
            {
                return NotFound();
            }
            _context.CelestialObjects.RemoveRange(matched);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
