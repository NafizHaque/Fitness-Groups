using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroups.Data;
using RunGroups.DTOs.ClubDTOs;
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
        public async Task<IActionResult> Edit(int id)
        {
            Race race = await _raceService.GetByIdAsync(id);
            if (race == null) return View("Error");
            EditRaceDto raceDto = new EditRaceDto
            {
                Title = race.Title,
                Description = race.Description,
                AddressId = race.AddressId,
                Address = race.Address,
                URL = race.Image,
                RaceCategory = race.RaceCategory
            };
            return View(raceDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubDto raceDto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "failed to edit club");
                return View("Edit", raceDto);
            }
            Race userRace = await _raceService.GetByIdAsyncNoTracking(id);
            if (userRace != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userRace.Image);

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(raceDto);
                }
                ImageUploadResult photoResult = await _photoService.AddPhotoAsync(raceDto.Image);

                Race race = new Race
                {
                    Id = id,
                    Title = raceDto.Title,
                    Description = raceDto.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = raceDto.AddressId,
                    Address = raceDto.Address,
                };

                _raceService.Update(race);
                return RedirectToAction("Index");
            }
            else
            {
                return View(raceDto);
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            Race race = await _raceService.GetByIdAsync(id);
            if (race == null) { return View("Error"); }
            return View(race);

        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            Race raceDeletion = await _raceService.GetByIdAsync(id);
            if (raceDeletion == null) { return View("Error"); }
            _raceService.Delete(raceDeletion);
            return RedirectToAction("Index");

        }

    }
}
