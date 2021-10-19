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
            if (currentObjects == null)
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
    }
}
