using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController()]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name ="GetById")]
        public IActionResult GetById(int id)
        {
            var currentObject = _context.CelestialObjects.FirstOrDefault(e => e.Id == id);
            if (currentObject == null)
            {
                return NotFound();
            }
            else
            {
                currentObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == id).ToList();   
                return Ok(currentObject);
            }

        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var currentObjects = _context.CelestialObjects.Where(e => e.Name == name);
            if (currentObjects.Count() == 0)
            {
                return NotFound();
            }
            else
            {
                foreach (var item in currentObjects)
                {
                    item.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == item.Id).ToList();
                }

                return Ok(currentObjects);
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var objects = _context.CelestialObjects;
            foreach (var item in objects)
            {
                item.Satellites = objects.Where(e => e.OrbitedObjectId == item.Id).ToList();
            }
            return Ok(objects);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject newObject)
        {
            _context.CelestialObjects.Add(newObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = newObject.Id }, newObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject currentObject)
        {
            var dbObject = _context.CelestialObjects.FirstOrDefault(e => e.Id == id);
            if (dbObject == null)
            {
                return NotFound();
            }
            else
            {
                dbObject.Name = currentObject.Name;
                dbObject.OrbitalPeriod = currentObject.OrbitalPeriod;
                dbObject.OrbitedObjectId = currentObject.OrbitedObjectId;
                _context.CelestialObjects.Update(dbObject);
                _context.SaveChanges();
                return NoContent();
            }
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var currentObject = _context.CelestialObjects.FirstOrDefault(e => e.Id == id);
            if (currentObject == null)
            {
                return NotFound();
            }
            else
            {
                currentObject.Name = name;
                _context.CelestialObjects.Update(currentObject);
                _context.SaveChanges();
                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var allMatches = _context.CelestialObjects.Where(e => e.Id == id || e.OrbitedObjectId == id);
            if (allMatches.Count() == 0)
            {
                return NotFound();
            }
            else
            {
                _context.CelestialObjects.RemoveRange(allMatches);
                _context.SaveChanges();
                return NoContent();
            }

        }
    }
}
