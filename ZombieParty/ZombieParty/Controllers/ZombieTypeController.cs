using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZombieParty.Models;
using ZombieParty.Models.Data;
using ZombieParty.ViewModels;

namespace ZombieParty.Controllers
{
    public class ZombieTypeController : Controller
    {
        private ZombiePartyDbContext _baseDonnees { get; set; }

        public ZombieTypeController(ZombiePartyDbContext baseDonnees)
        {
            _baseDonnees = baseDonnees;
        }

        public IActionResult Index()
        {
            List<ZombieType> zombieTypesList = _baseDonnees.ZombieTypes.ToList();

            return View(zombieTypesList);
        }

        public IActionResult Details(int id)
        {
            var zombies = _baseDonnees.Zombies.Where(z => z.ZombieTypeId == id);

            ZombieTypeVM zombieTypeVM = new()
            {
                ZombieType = new(),
                ZombiesList = zombies.ToList(),
                ZombiesCount = zombies.Count(),
                PointsAverage = zombies.Average(p => p.Point)
            };

            zombieTypeVM.ZombieType = _baseDonnees.ZombieTypes.FirstOrDefault(zt => zt.Id == id);
            return View(zombieTypeVM);
        }


        //GET CREATE
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        public IActionResult Create(Models.ZombieType zombieType)
        {
            if (ModelState.IsValid)
            {
                // Ajouter à la BD
                _baseDonnees.ZombieTypes.Add(zombieType);
                _baseDonnees.SaveChanges();
                TempData["Success"] = $"{zombieType.TypeName} zombie type added";
                return this.RedirectToAction("Index");
            }

            return this.View(zombieType);
        }

        public IActionResult Edit(int id)
        {
            ZombieTypeVM zombieTypeVM = new ZombieTypeVM();
            zombieTypeVM.ZombieType = _baseDonnees.ZombieTypes.Find(id);

            return View(zombieTypeVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ZombieTypeVM zombieTypeVM)
        {
            //Si le modèle est valide le zombieType est modifié et nous sommes redirigé vers index.
            if (ModelState.IsValid)
            {
                _baseDonnees.ZombieTypes.Update(zombieTypeVM.ZombieType);
                _baseDonnees.SaveChanges();
                TempData["Success"] = $"ZombieType {zombieTypeVM.ZombieType.TypeName} has been modified";
                return this.RedirectToAction("Index");
            }

            return View(zombieTypeVM);
        }

        public IActionResult Delete(int id)
        {
            ZombieTypeVM zombieTypeVM = new ZombieTypeVM();
            zombieTypeVM.ZombieType = _baseDonnees.ZombieTypes.Find(id);

            if (zombieTypeVM == null)
            {
                return NotFound();
            }

            return View(zombieTypeVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            ZombieType? zombieTypeVM = _baseDonnees.ZombieTypes.Find(id);
            if (zombieTypeVM == null)
            {
                return NotFound();
            }

            _baseDonnees.ZombieTypes.Remove(zombieTypeVM);
            _baseDonnees.SaveChanges();
            TempData["Success"] = $"ZombieType {zombieTypeVM.TypeName} terminated";
            return RedirectToAction("Index");
        }

    }
}
