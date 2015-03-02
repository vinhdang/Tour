using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.Base;

namespace Website.Base
{
    public class EntityController<TIEntityService, TEntityType> : Controller
        where TEntityType : class,new()
        where TIEntityService : IServiceBase<TEntityType>
    {
        private readonly TIEntityService m_Service;
        public EntityController(TIEntityService iService)
        {
            this.m_Service = iService;
        }
        public virtual ViewResult Index()
        {
            return View(m_Service.All);
        }

        public virtual ViewResult Details(int id)
        {
            return View(m_Service.Find(id));
        }

        public virtual ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult Create(TEntityType entity)
        {
            if (ModelState.IsValid)
            {
                m_Service.Insert(entity);
                m_Service.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }



        public virtual ActionResult Edit(int id)
        {
            return View(m_Service.Find(id));
        }
        [HttpPost]
        public virtual ActionResult Edit(TEntityType entity)
        {
            if (ModelState.IsValid)
            {
                m_Service.Update(entity);
                m_Service.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }



        public virtual ActionResult Delete(int id)
        {
            return View(m_Service.Find(id));
        }

        //
        // POST: /TEntityType/Delete/5

        [HttpPost, ActionName("Delete")]
        public virtual ActionResult DeleteConfirmed(int id)
        {
            m_Service.Delete(id);
            m_Service.Save();

            return RedirectToAction("Index");
        }

    }
}