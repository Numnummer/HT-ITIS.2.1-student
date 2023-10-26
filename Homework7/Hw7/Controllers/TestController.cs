﻿using Hw7.Models.ForTests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hw7.Controllers
{
    public class TestController : Controller
    {
        // GET: TestController1
        public ActionResult TestModel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TestModel(TestModel testModel)
        {
            return View(testModel);
        }

        // GET: TestController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TestController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TestController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TestController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TestController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TestController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TestController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
