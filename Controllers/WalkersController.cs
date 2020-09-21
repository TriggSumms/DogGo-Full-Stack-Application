using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DogGo.Controllers
{
    //If I was to change the public class name of walker and use intelisense....it would change the occurences and then by changing the file name and index.cshtml....I could chage the url link name
    //
    public class WalkersController : Controller
    {
        private readonly IWalkerRepository _walkerRepo;

        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        public WalkersController(IWalkerRepository walkerRepository)
        {
            _walkerRepo = walkerRepository;
        }
        // GET: Walkers
        public IActionResult Index()
        {
            List<Walker> walkers = _walkerRepo.GetAllWalkers();

            return View(walkers);
        }
        // GET: Walkers/Details/5
        public ActionResult Details(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);

            if (walker == null)
            {
                return NotFound();
            }

            return View(walker);
        }


    }
}
