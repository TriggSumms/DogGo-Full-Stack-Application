
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogGo.Controllers
{
   // [Authorize]
    //Stuck Authorize at the top because I would like all of the methods to need a login
    public class DogsController : Controller

    {
        private readonly IDogRepository _dogRepo;
        private readonly IOwnerRepository _ownerRepo;

        // ASP.NET will give us an instance of our Owner Repository. This is called "Dependency Injection"
        public DogsController(IDogRepository dogRepository, IOwnerRepository ownerRepository)
        {
            _dogRepo = dogRepository;
            _ownerRepo = ownerRepository;
        }



        // GET: DogController
        //public IActionResult Index()
        //{
        //    List<Dog> dogs = _dogRepo.GetAllDogs();
        //    return View(dogs);
        //}


        //New call sorting with login method, replaced the GETcontroller
        
        public ActionResult Index()
        {
            int ownerId = GetCurrentUserId();

            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(ownerId);

            return View(dogs);
        }
        //End


        
        // GET: DogController/Details/5
        public ActionResult Details(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);

            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // GET: DogController/Create
        // LOOK AT THIS
        public ActionResult Create()
        {
            // We use a view model because we need the list of Owners in the Create view
            DogFormViewModel vm = new DogFormViewModel()
            {
                Dog = new Dog(),
                Owners = _ownerRepo.GetAllOwners(),
            };

            return View(vm);
        }


        // POST: DogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dog dog)
        {
            try
            {
                dog.OwnerId = GetCurrentUserId();
                _dogRepo.AddDog(dog);
                return RedirectToAction("Index");
            }
            catch
            {
                // LOOK AT THIS
                //  When something goes wrong we return to the view
                //  BUT our view expects a DogFormViewModel object...so we'd better give it one
                DogFormViewModel vm = new DogFormViewModel()
                {
                    Dog = dog,
                };

                return View(vm);
            }
        }

            // GET: DogController/Edit/5
            public ActionResult Edit(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);

            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }
        
        // POST: DogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Dog dog)
        {
            try
            {
                dog.OwnerId = GetCurrentUserId();
                _dogRepo.UpdateDog(dog);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }

       // [Authorize]
        // GET: DogController/Delete/5
        public ActionResult Delete(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);

            return View(dog);
        }

       // [Authorize]
        // POST: DogController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Dog dog)
        {
            try
            {
                dog.OwnerId = GetCurrentUserId();
                _dogRepo.DeleteDog(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }


        //Helper Method, for authenticating the current logged in users management privelages
        private int GetCurrentUserId()
        {
            //User is built in to MVC, it is a property given to us....
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
        //END login helper management 
    }
}
