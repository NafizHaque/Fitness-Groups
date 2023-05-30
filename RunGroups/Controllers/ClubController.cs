using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroups.Data;
using RunGroups.DTOs.ClubDTOs;
using RunGroups.Interfaces;
using RunGroups.Models;
using System.Diagnostics.Eventing.Reader;

namespace RunGroups.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubService _clubService;
        private readonly IPhotoService _photoService;
        public ClubController(IClubService clubService, IPhotoService photoService)
        {
            _clubService = clubService;
            _photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubService.GetAll();
            return View(clubs);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Club club = await _clubService.GetByIdAsync(id);
            return View(club);
        }

        public IActionResult Create() { return View(); }

        [HttpPost]
        public async Task<IActionResult> Create(ClubDto clubDto)
        {
            if (ModelState.IsValid)
            {
                ImageUploadResult result = await _photoService.AddPhotoAsync(clubDto.Image);
                Club club = new Club
                {
                    Title = clubDto.Title,
                    Description = clubDto.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        AddressLine = clubDto.Address.AddressLine,
                        City = clubDto.Address.City,
                        Country = clubDto.Address.Country,
                        Postcode = clubDto.Address.Postcode,

                    }
                };
                _clubService.Add(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }
            return View(clubDto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Club club = await _clubService.GetByIdAsync(id);
            if (club == null) return View("Error");
            EditClubDto clubDto = new EditClubDto
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                URL = club.Image,
                ClubCategory = club.ClubCategory
            };
            return View(clubDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubDto clubDto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "failed to edit club");
                return View("Edit", clubDto);
            }
            Club userClub = await _clubService.GetByIdAsyncNoTracking(id);
            if (userClub != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userClub.Image);

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(clubDto);
                }
                ImageUploadResult photoResult = await _photoService.AddPhotoAsync(clubDto.Image);

                Club club = new Club
                {
                    Id = id,
                    Title = clubDto.Title,
                    Description = clubDto.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = clubDto.AddressId,
                    Address = clubDto.Address,
                };

                _clubService.Update(club);
                return RedirectToAction("Index");
            }
            else
            {
                return View(clubDto);
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            Club club = await _clubService.GetByIdAsync(id);
            if (club == null) { return View("Error"); }
            return View(club);

        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            Club clubDeletion = await _clubService.GetByIdAsync(id);
            if (clubDeletion == null) { return View("Error"); }
            _clubService.Delete(clubDeletion);
            return RedirectToAction("Index");

        }
    }
}
