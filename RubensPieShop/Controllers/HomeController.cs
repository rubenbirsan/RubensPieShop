using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RubensPieShop.Models;
using RubensPieShop.ViewModels;

namespace RubensPieShop.Controllers
{
    public class HomeController : Controller
    {

        private readonly IPieRepository _pieRepository;

        public HomeController(IPieRepository pieRepository)
        {
            _pieRepository = pieRepository;

        }

        public IActionResult Index()
        {
            var pies = _pieRepository.GetAllPies().OrderBy(p => p.Name);

            var homeViewModel = new HomeViewModel()
            {
                Pies = pies.ToList(),
                Title = "Welcome to Ruben's Shop"
            };

            foreach (var pie in homeViewModel.Pies)
            {
                pie.ImageThumbnailUrl = pie.ImageThumbnailUrl.Replace("server","");
            }

            return View(homeViewModel);
        }

        public IActionResult Details(int id)
        {
            var pie = _pieRepository.GetPieById(id);
            if (pie == null)
                return NotFound();

            pie.ImageUrl = pie.ImageUrl.Replace("server", "");

            return View(pie);
        }
    }
}