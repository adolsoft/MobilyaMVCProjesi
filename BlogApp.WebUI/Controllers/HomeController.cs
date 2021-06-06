using BlogApp.Data.Abstract;
using BlogApp.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IBlogRepository blogRepository;

        public HomeController(IBlogRepository repository)
        {
            blogRepository = repository;
        }
        public IActionResult Index()
        {
            var model = new HomeBlogModel();

            model.HomeBlogs = blogRepository.GetAll().Where(i => i.isApproved && i.isHome).ToList();
            model.SliderBlogs = blogRepository.GetAll().Where(i => i.isApproved && i.isSlider).ToList();

            return View(model);//ikiside true olanları getirecek.
        }

        public IActionResult Details()
        {
            return View();
        }
        public IActionResult List()
        {
            return View();
        }
    }
}
