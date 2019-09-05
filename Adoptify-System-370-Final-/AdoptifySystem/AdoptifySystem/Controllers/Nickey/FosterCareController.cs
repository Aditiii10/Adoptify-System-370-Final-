using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AdoptifySystem;
using AdoptifySystem.Models.nickeymodel;

namespace AdoptifySystem.Controllers
{
    public class FosterCareController : Controller
    {
        // GET: FosterCare
        Wollies_ShelterEntities db = new Wollies_ShelterEntities();
        static Flexible flex = new Flexible();
        // GET: FosterCare
        public ActionResult AddFosterCareParent()
        {
            List<Foster_Care_Parent> mylist = new List<Foster_Care_Parent>();
            mylist = db.Foster_Care_Parent.ToList();
            return View(mylist);
        }
        public JsonResult getProducts(int? categoryID)
        {
            List<Foster_Care_Parent> products = new List<Foster_Care_Parent>();
            using (Wollies_ShelterEntities dc = new Wollies_ShelterEntities())
            {
                products = dc.Foster_Care_Parent.Where(z => z.Foster_Parent_ID == categoryID).OrderBy(z => z.Foster_Parent_Name).ToList();
            }

            return new JsonResult { Data = products, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public ActionResult AddFosterCareParent(Foster_Care_Parent foster_Care_Parent, string button)
        {
            ViewBag.errorMessage = "";
            //Donation_Type asd = new Donation_Type();
            if (button == "Save")
            {
                try
                {

                    List<Foster_Care_Parent> foster_Care_Parents = new List<Foster_Care_Parent>();
                    foster_Care_Parents = db.Foster_Care_Parent.ToList();
                    if (foster_Care_Parents.Count != 0)
                    {
                        int count = 0;
                        foreach (var item in foster_Care_Parents)
                        {
                            if (item.Foster_Parent_Name == foster_Care_Parent.Foster_Parent_Name)
                            {
                                count++;
                                ViewBag.errorMessage = "There is a duplicate Donation Type Already";
                                return View();
                            }

                        }
                        if (count == 0)
                        {
                            db.Foster_Care_Parent.Add(foster_Care_Parent);
                            db.SaveChanges();
                        }
                    }
                    else
                    {

                        db.Foster_Care_Parent.Add(foster_Care_Parent);
                        db.SaveChanges();


                    }

                }
                catch (Exception e)
                {
                    ViewBag.errorMessage = "There was an Error with network please try again: " + e.Message;
                    return View();
                }

            }
            else if (button == "Cancel")
            {

                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");

        }
        [HttpPost]
        public ActionResult MaintainFosterCareParent(Foster_Care_Parent foster_Care_Parent, string button)
        {
            if (button == "Save")
            {
                try
                {
                    Foster_Care_Parent foster_Care_Parent1 = db.Foster_Care_Parent.Find(foster_Care_Parent.Foster_Parent_ID);
                    if (foster_Care_Parent1 == null)
                    {

                        return HttpNotFound();
                    }
                    else
                    {
                        db.Entry(foster_Care_Parent1).CurrentValues.SetValues(foster_Care_Parent);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewBag.err = e.Message;
                    return RedirectToAction("MaintainFosterCareParent", "Stock");
                }
            }
            else if (button == "Cancel")
            {

                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult MaintainFosterCareParent(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Foster_Care_Parent foster_Care_Parent = db.Foster_Care_Parent.Find(id);
                if (foster_Care_Parent == null)
                {
                    return HttpNotFound();
                }
                return View(foster_Care_Parent);

            }
            catch (Exception e)
            {
                ViewBag.err = e.Message;
                throw;
            }
            
        }
        public ActionResult SearchFosterCareParent()
        {
            ViewBag.errormessage = "";
            List<Foster_Care_Parent> foster_Care_Parents = new List<Foster_Care_Parent>();
            try
            {
                foster_Care_Parents = db.Foster_Care_Parent.ToList();
                if (foster_Care_Parents.Count == 0)
                {

                }
                return View(foster_Care_Parents);
            }
            catch (Exception e)
            {
                ViewBag.errormessage = "there was a network error: " + e.Message;
                return View();
            }
        }
        public ActionResult AddtoFosterCare()
        {
            try
            {
                flex.fostercareparent = db.Foster_Care_Parent.ToList();
                return View(flex);
            }
            catch (Exception e)
            {
                ViewBag.err = e.Message;
                return RedirectToAction("index", "home");
            }
            
        }
        public async Task<ActionResult> Test(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            flex.parent = await db.Foster_Care_Parent.Where(a => a.Foster_Parent_ID == id).FirstOrDefaultAsync();

            if (flex.parent == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("AddtoFosterCare", flex.parent);

        }
        public ActionResult RemovefromFosterCare()
        {
            return View();
        }
    }
}