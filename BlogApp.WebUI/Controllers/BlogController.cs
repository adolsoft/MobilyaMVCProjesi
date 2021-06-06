using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.WebUI.Controllers
{
    public class BlogController : Controller
    {
        private IBlogRepository _blogRepository;
        private ICategoryRepository _categoryRepository;

        public BlogController(IBlogRepository blogRepo, ICategoryRepository categoryRepository)
        {
            _blogRepository = blogRepo;
            _categoryRepository = categoryRepository;
        }
        
        public IActionResult Index(int? id, string q)
        {

            var query = _blogRepository.GetAll()
                        .Where(i => i.isApproved);

            if (id != null)
            {
                query = query.Where(i => i.CategoryId == id);
               
            }

            if (!string.IsNullOrEmpty(q)) //! değil anlamındadir.
            {
                // query = query.Where(i => i.Title.Contains(q) || i.Description.Contains(q) || i.Body.Contains(q)); // başlıklarda q parametresini arar
               query = query.Where(i=>EF.Functions.Like(i.Title,"%"+q+"%") || EF.Functions.Like(i.Description, "%" +q + "%") || EF.Functions.Like(i.Body, "%" + q + "%"));
            }
           
                return View(query.OrderByDescending(i => i.Date));
            
        }

        public IActionResult Details(int id)
        {
            return View(_blogRepository.GetById(id));
        }

        public IActionResult ListDeneme()
        {
            return View();
        }
        public IActionResult List()
        {
            return View(_blogRepository.GetAll());//GetAll metu ile bütün blog listesi çağrılır.
           
        }
        //[HttpGet]
        //public IActionResult Create()
        //{
        //    ViewBag.Categories = new SelectList(_categoryRepository.GetAll(),"CategoryId","Name");
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Create(Blog entity)
        //{
        //    entity.Date = DateTime.Now;
        //    if (ModelState.IsValid)
        //    {
        //        _blogRepository.AddBlog(entity);
        //        return RedirectToAction("List");
        //    }

        //    return View(entity);
        //}
       // [HttpGet]
       // public IActionResult Edit(int id)
       // {
       //     ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
       //     return View(_blogRepository.GetById(id));

       // }

       //[HttpPost]
       //public IActionResult Edit(Blog entity)
       // {
       //     if (ModelState.IsValid)
       //     {
       //         _blogRepository.UpdateBlog(entity);
       //         TempData["message"] = $"{entity.Title} güncellendi.";
       //         return RedirectToAction("List");
       //     }
       //     ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
       //     return View(entity);
       // }

        //Yeni kayıt oluşturmak için kullanıyoruz.
        [HttpGet]
        public IActionResult Create()
        {
            
            
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
                      
            //yeni bir kayıt yap
            return View(new Blog());//boş bir nesne göndermemiz gerekiyor yeni bir kayıt oluştururken

        }
        [HttpPost]
        public IActionResult Create(Blog entity)
        {
            if (ModelState.IsValid)
            {
                entity.Date = DateTime.Now;
                _blogRepository.SaveBlog(entity);
                TempData["message"] = $"{entity.Title} kayıt edildi.";
                return RedirectToAction("List");
            }
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
            return View(entity);
        }

        //Edit oluşturmak için kullanıyoruz.
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");

            //yeni bir kayıt yap
           return View(_blogRepository.GetById(id));//boş bir nesne göndermemiz gerekiyor yeni bir kayıt oluştururken
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Blog entity, IFormFile file)  //çok resim olursa IEnumerable<IFormFile> olacak
        {
            if (ModelState.IsValid)
            {
                if ( file != null)
                {

                     var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);

                     using (var stream = new FileStream(path, FileMode.Create))
                     {
                         await file.CopyToAsync(stream); // dosya yukarıdaki img dosyasına kaydedilir.
              
                         entity.Image = file.FileName; //entity içersine gelen resmin ismini kaydediyoruz.
                     }

                 }

                entity.Date = DateTime.Now;
                _blogRepository.SaveBlog(entity);
                TempData["message"] = $"{entity.Title} güncellendi...";
                return RedirectToAction("List");
            }
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
            return View(entity);
        }




        // bu kısmı artık kullanmıyoruz. Burası hem yeni kayı hemde güncelleme için kullandık Artık yukarıdaki kismı kullanıyoruz.
        [HttpGet]
        public IActionResult AddOrUpdate(int? id)
        {
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");

            if (id==null)
            {
                //yeni bir kayıt yap
                return View(new Blog());//boş bir nesne göndermemiz gerekiyor yeni bir kayıt oluştururken
            }
            else
            {
                //güncelleme yapılacak
                return View(_blogRepository.GetById((int)id)); //burda id null da olabileceğinden int çevirdik.
            }
        }
        [HttpPost]
        public IActionResult AddOrUpdate(Blog entity)
        {
            if (ModelState.IsValid)
            {
                entity.Date = DateTime.Now;
                _blogRepository.SaveBlog(entity);
                TempData["message"] = $"{entity.Title} kayıt edildi.";
                return RedirectToAction("List");
            }
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
            return View(entity);
        }

      

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(_blogRepository.GetById(id));
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteOnay(int BlogId)
        {
            _blogRepository.DeleteBlog(BlogId);
            TempData["message"] = $"{BlogId} numaralı kayıt silindi.";
            return RedirectToAction("List");
        }

    }
}
