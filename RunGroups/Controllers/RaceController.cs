using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroups.Data;
using RunGroups.DTOs.RaceDTOs;
using RunGroups.Interfaces;
using RunGroups.Models;
using RunGroups.Service;

namespace RunGroups.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceService _raceService;
        private readonly IPhotoService _photoService;
        public RaceController(IRaceService raceService, IPhotoService photoService) 
            
        {
            _raceService = raceService;
            _photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races = await _raceService.GetAll();
            return View(races);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Race race = await _raceService.GetByIdAsync(id);
            return View(race);
        }

        public IActionResult Create() { return View(); }

        [HttpPost]
        public async Task<IActionResult> Create(RaceDto raceDto)
        {
            if (ModelState.IsValid)
            {
                ImageUploadResult result = await _photoService.AddPhotoAsync(raceDto.Image);
                Race race = new Race
                {
                    Title = raceDto.Title,
                    Description = raceDto.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        AddressLine = raceDto.Address.AddressLine,
                        City = raceDto.Address.City,
                        Country = raceDto.Address.Country,
                        Postcode = raceDto.Address.Postcode,

                    }
                };
                _raceService.Add(race);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }
            return View(raceDto);

        }
    }
}
