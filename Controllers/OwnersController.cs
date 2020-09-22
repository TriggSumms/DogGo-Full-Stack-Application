﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace DogGo.Controllers
{
    public class OwnersController : Controller
    {



        //METHODS added via Chp 5...ties in model VIEWS
        private readonly IOwnerRepository _ownerRepo;
        private readonly IDogRepository _dogRepo;
        private readonly IWalkerRepository _walkerRepo;
        private readonly INeighborhoodRepository _neighborhoodRepo;


        public OwnersController(
            IOwnerRepository ownerRepository,
            IDogRepository dogRepository,
            IWalkerRepository walkerRepository,
            INeighborhoodRepository neighborhoodRepository)
        {
            _ownerRepo = ownerRepository;
            _dogRepo = dogRepository;
            _walkerRepo = walkerRepository;
            _neighborhoodRepo = neighborhoodRepository;
        }
        //END



        // ASP.NET will give us an instance of our Owner Repository. This is called "Dependency Injection"
        // private readonly IOwnerRepository _ownerRepo;
        //public OwnersController(IOwnerRepository ownerRepository)
        //{
        //    _ownerRepo = ownerRepository;
        //}
        // GET: Owners babi
        public IActionResult Index()
        {
            List<Owner> owners = _ownerRepo.GetAllOwners();

            return View(owners);
        }
        //// GET: Owners/Details/5
        //public ActionResult Details(int id)
        //{
        //   Owner owner = _ownerRepo.GetOwnerById(id);

        //    if (owner == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(owner);
        //}

        //Chp 5 model view method
        // GET: Owners/Details/5
        public ActionResult Details(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);
            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(owner.Id);
            List<Walker> walkers = _walkerRepo.GetWalkersInNeighborhood(owner.NeighborhoodId);

            //We used the items declared above.....to pair our new lists/paramenters with the requested Id 
            //and then shoved it into a profileVIEW, then we returned it. (Had to change details panel)
            ProfileViewModel vm = new ProfileViewModel()
            {
                Owner = owner,
                Dogs = dogs,
                Walkers = walkers
            };

            return View(vm);
        }
        //END
        //public ActionResult Create()
        //{
        //    return View();
        //}


        //CHP 5
        // GET: Owners/Create
        public ActionResult Create()
        {
            List<Neighborhood> neighborhoods = _neighborhoodRepo.GetAll();

            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                Owner = new Owner(),
                Neighborhoods = neighborhoods
            };

            return View(vm);
        }
        //CHP 5 END

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Owner owner)
        {
            try
            {
                _ownerRepo.AddOwner(owner);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(owner);
            }
        }

        // GET: Owners/Delete/5
        public ActionResult Delete(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);

            return View(owner);
        }

        // POST: Owners/Delete/5
        //Delete and Post are needed in hand
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Owner owner)
        {
            try
            {
                _ownerRepo.DeleteOwner(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(owner);
            }
        }

        public ActionResult Edit(int id)
        {
            List<Neighborhood> neighborhoods = _neighborhoodRepo.GetAll();

            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
            Owner = _ownerRepo.GetOwnerById(id),
            Neighborhoods = neighborhoods
            };

            //if (owner == null)
            //{
            //    return NotFound();
            //}

            return View(vm);
        }


        //GET: Owners/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    Owner owner = _ownerRepo.GetOwnerById(id);

        //    if (owner == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(owner);
        //}
        // POST: Owners/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Owner owner)
        {
            try
            {
                _ownerRepo.UpdateOwner(owner);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(owner);
            }
        }


        //LOGIN START
        public ActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel viewModel)
        {
            Owner owner = _ownerRepo.GetOwnerByEmail(viewModel.Email);
            //Did we find an owner?
            if (owner == null)
            {
                return Unauthorized();
            }
            //Run the code or run the 401 ERROR/unauth'd
            var claims = new List<Claim>
    {
            new Claim(ClaimTypes.NameIdentifier, owner.Id.ToString()),
            new Claim(ClaimTypes.Email, owner.Email),
            new Claim(ClaimTypes.Role, "DogOwner"),
    };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Dogs");
        }
        //LOGIN END
        //Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        //Logout END

    }
}