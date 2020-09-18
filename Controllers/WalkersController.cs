using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace DogGo.Controllers
{
    //If I was to change the public class name of walker and use intelisense....it would change the occurences and then by changing the file name and index.cshtml....I could chage the url link name
    //
    public class WalkersController : Controller
    {
        private readonly IWalkerRepository _walkerRepo;

        private readonly IDogRepository _dogRepo;

        private readonly IWalkRepository _walksRepo;


        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        public WalkersController(
            IWalkerRepository walkerRepository,
            IDogRepository dogRepository,
            IWalkRepository walksRepository)
        {
            _walkerRepo = walkerRepository;
            _dogRepo = dogRepository;
            _walksRepo = walksRepository;
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
            List<Walker> walkers = _walkerRepo.GetAllWalkers();
            //Walk walks = _walkerRepo.GetAll();
            //Had to add to idogRepo...
            //List<Dog> dogs = _dogRepo.GetDogsByWalkerId(walker.Id);
            //
            List<Walk> walk = _walksRepo.GetALLWalksandIds(walker.Id);


            //We used the items declared above.....to pair our new lists/paramenters with the requested Id 
            //and then shoved it into a profileVIEW, then we returned it. (Had to change details panel)
            WalkerDetailsViewModel vm = new WalkerDetailsViewModel()
            {
                Walkers = walkers,
               // Dogs = dogs,
                Walks = walk
            };

            return View(vm);
        }


    }
}
