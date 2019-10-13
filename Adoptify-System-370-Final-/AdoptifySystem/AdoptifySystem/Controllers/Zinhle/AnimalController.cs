using AdoptifySystem.Models;
using AdoptifySystem.Models.nickeymodel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AdoptifySystem.Controllers.Zinhle
{
    public class AnimalController : Controller
    {
        // GET: Animal

        Wollies_ShelterEntities db = new Wollies_ShelterEntities();
        static public Innovation innovation = new Innovation();
        static public Flexible flex = new Flexible();
        [HttpGet]
        public JsonResult animalbreed(int categoryID)
        {
            //List<Animal_Breed> breeds = new List<Animal_Breed>();

            try
            {
                int id = Convert.ToInt32(categoryID);
                var sbreeds = db.Animal_Breed.Where(a => a.Animal_Type_ID == id).OrderBy(a => a.Animal_Breed_Name).ToList();
                var breeds = db.Animal_Breed.Select(x => new
                {
                    Animal_Breed_ID = x.Animal_Breed_ID,
                    Animal_Breed_Name = x.Animal_Breed_Name,
                    Animal_Type_ID = x.Animal_Type_ID,
                }).ToList();
                breeds = breeds.Where(z => z.Animal_Type_ID == id).ToList();
                ViewBag.breed = breeds;
                return Json(breeds, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw new Exception("Something Went Wrong!");
            }

        }
        public ActionResult AddTemporaryAnimal()
        {
            try
            {
                innovation.animalTypes = db.Animal_Type.ToList();
                return View(innovation);
            }
            catch (Exception e)
            {
                TempData["EditMessage"] = e.Message;
                throw new Exception("Something Went Wrong!");

            }

        }

        public ContentResult test(string Cross_Breed, Animal animal, string[] breeds, Microchip micro, HttpPostedFileBase animalPicture, FormCollection form)
        {
            try
            {
                if (animalPicture != null)
                {
                    //this is where we convert the contract to add to the database
                    byte[] bytes;
                    using (BinaryReader br = new BinaryReader(animalPicture.InputStream))
                    {

                        bytes = br.ReadBytes(animalPicture.ContentLength);
                    }
                    animal.Animal_Image_Name = Path.GetFileName(animalPicture.FileName);
                    animal.Animal_Image_Type = animalPicture.ContentType;
                    animal.Animal_Image = bytes;


                }

                if (animal != null)
                {
                    Animal_Status status = db.Animal_Status.Where(zz => zz.Animal_Status_Name == "Available").FirstOrDefault();
                    if (status == null)
                    {
                        TempData["EditMessage"] = "there are no status available.";
                        throw new Exception("Something Went Wrong!");
                    }
                    animal.Animal_Status_ID = status.Animal_Status_ID;
                    db.Animals.Add(animal);
                    db.SaveChanges();
                    flex.CreateAuditTrail(Convert.ToInt32(Session["ID"].ToString()), "Animal");
                }
                if (Cross_Breed == "True")
                {
                    // Split authors separated by a comma followed by space  
                    string[] breed = breeds[0].Split(',');
                    foreach (var item in breed)
                    {
                        CrossBreed cross = new CrossBreed();
                        cross.Animal_ID = animal.Animal_ID;
                        cross.Animal_Breed_ID = Convert.ToInt32(item);
                        db.CrossBreeds.Add(cross);
                        db.SaveChanges();
                    }
                }
                else
                {
                    CrossBreed cross = new CrossBreed();
                    cross.Animal_ID = animal.Animal_ID;
                    cross.Animal_Breed_ID = Convert.ToInt32(breeds);
                    db.CrossBreeds.Add(cross);
                    db.SaveChanges();

                }
                if (micro.Animal_Microchip_Code != null || micro.Implanters_PIN_Number != null || micro.Owner_Name != null || micro.Owner_Address != null || micro.Owner_Contact_Number != null)
                {

                    micro.Animal_ID = animal.Animal_ID;

                    db.Microchips.Add(micro);
                    db.SaveChanges();
                }
                TempData["SuccessMessage"] = "The animal is Stored";
                return Content("SearchAnimal");
            }
            catch (Exception e)
            {
                TempData["EditMessage"] = e.Message;
                throw new Exception("Something Went Wrong!");

            }
        }
        //[HttpPost]
        //public ActionResult AddTemporaryAnimal(bool Cross_Breed, Animal animal, int[] breed, Microchip micro,HttpPostedFileBase animalPicture)
        //{
        //    try
        //    {
        //        if(animalPicture != null)
        //        {
        //            //this is where we convert the contract to add to the database
        //            byte[] bytes;
        //            using (BinaryReader br = new BinaryReader(animalPicture.InputStream))
        //            {

        //                bytes = br.ReadBytes(animalPicture.ContentLength);
        //            }
        //            animal.Animal_Image_Name = Path.GetFileName(animalPicture.FileName);
        //            animal.Animal_Image_Type = animalPicture.ContentType;
        //            animal.Animal_Image = bytes;

        //        }

        //        if (animal !=null)
        //        {
        //            Animal_Status status = db.Animal_Status.Where(zz => zz.Animal_Status_Name == "Available").FirstOrDefault();
        //            if (status == null)
        //            {
        //                TempData["EditMessage"] = "there are no status available.";
        //                return RedirectToAction("AddTemporaryAnimal");
        //            }
        //            animal.Animal_Status_ID = status.Animal_Status_ID;
        //            db.Animals.Add(animal);
        //            db.SaveChanges();
        //        }
        //        if (Cross_Breed)
        //        {
        //            foreach(var item in breed)
        //            {
        //                CrossBreed cross = new CrossBreed();
        //                cross.Animal_ID = animal.Animal_ID;
        //                cross.Animal_Breed_ID = item;
        //            }
        //        }
        //        if (micro != null)
        //        {
        //            Animal animalid = db.Animals.Where(zz => zz.Animal_Name == animal.Animal_Name && zz.Animal_Size == animal.Animal_Size && zz.Animal_Age == animal.Animal_Age && zz.Animal_Entry_Date == animal.Animal_Entry_Date).FirstOrDefault();
        //            micro.Animal_ID = animalid.Animal_ID;
        //            db.Microchips.Add(micro);
        //            db.SaveChanges();
        //        }
        //        TempData["SuccessMessage"] = "The animal is Stored";
        //        return RedirectToAction("SearchAnimal");
        //    }
        //    catch (Exception e)
        //    {
        //        TempData["EditMessage"] = e.Message;
        //        return RedirectToAction("SearchAnimal");

        //    }            
        //}

        public ActionResult SearchAnimal()
        {
            List<Animal> animals = new List<Animal>();
            try
            {
                db.Database.CommandTimeout = 300;
                animals = db.Animals.ToList();
            }
            catch (Exception e)
            {
                TempData["EditMessage"] = e.Message;
                throw new Exception("Something Went Wrong!");
            }


            return View(animals);
        }
        [HttpPost]
        public ActionResult SearchAnimal(string search)
        {

            if (!(search == ""))
            {
                db.Database.CommandTimeout = 300;
                var animallist = db.Animals.Where(z => z.Animal_Name.StartsWith(search)).ToList();
                if (animallist == null)
                {
                    return RedirectToAction("SearchAnimal");
                }
                List<Animal> animals = new List<Animal>();
                animals = animallist;

                return View("SearchAnimal", animals);

            }
            TempData["SuccessMessage"] = "Enter Valid Details";
            return View();
        }

        public ActionResult MaintainAnimal(int? id)
        {
            try
            {
                db.Database.CommandTimeout = 150;
                Animal animals = db.Animals.Find(id);

                if (animals == null)
                {
                    TempData["EditMessage"] = "Animal not Found";
                    return View("SearchAnimal");
                }
                var micro = db.Microchips.Where(z => z.Animal_ID == animals.Animal_ID).FirstOrDefault();
                if (micro != null)
                {
                    innovation.micro = micro;
                }

                innovation.animal = animals;
                var emp = db.Animal_Type.ToList();
                var breed = db.Animal_Breed.ToList();
                innovation.animalTypes = emp;
                innovation.breedTypes = breed;
            }
            catch (Exception e)
            {
                TempData["EditMessage"] = e.Message;
                return View("SearchAnimal");
            }
            return View(innovation);
        }

        [HttpPost]
        public ContentResult MaintainAnimal(Animal animal, Microchip micro, HttpPostedFileBase animalPicture)
        {
            try
            {
                List<Animal> animalist = new List<Animal>();
                animalist = db.Animals.ToList();
                if (animalist != null)
                {
                    foreach (var item in animalist)
                    {
                        if (animal.Animal_Name == item.Animal_Name &&
                        animal.Animal_Coat == item.Animal_Coat &&
                        animal.Animal_Age == item.Animal_Age &&
                        animal.Animal_Description == item.Animal_Description &&
                        animal.Animal_Sterilization == item.Animal_Sterilization &&
                        animal.Animal_Castration == item.Animal_Castration &&
                        animal.Animal_Size == item.Animal_Size &&
                        animal.Animal_Gender == item.Animal_Gender)
                        {
                            if (item.Animal_ID != animal.Animal_ID)
                            {
                                return Content("");
                            }
                            
                        }
                    }
                }
                Animal searchanimal = db.Animals.Find(animal.Animal_ID);

                searchanimal.Animal_Name = animal.Animal_Name;
                searchanimal.Animal_Coat = animal.Animal_Coat;
                searchanimal.Animal_Age = searchanimal.Animal_Age;
                searchanimal.Animal_Description = animal.Animal_Description;
                searchanimal.Animal_Sterilization = animal.Animal_Sterilization;
                searchanimal.Animal_Castration = animal.Animal_Castration;
                searchanimal.Animal_Size = animal.Animal_Size;
                searchanimal.Animal_Gender = animal.Animal_Gender;
                if (animalPicture != null)
                {
                    //this is where we convert the contract to add to the database
                    byte[] bytes;
                    using (BinaryReader br = new BinaryReader(animalPicture.InputStream))
                    {

                        bytes = br.ReadBytes(animalPicture.ContentLength);
                    }
                    searchanimal.Animal_Image_Name = Path.GetFileName(animalPicture.FileName);
                    searchanimal.Animal_Image_Type = animalPicture.ContentType;
                    searchanimal.Animal_Image = bytes;
                }



                db.SaveChanges();
                flex.UpdateAuditTrail(Convert.ToInt32(Session["ID"].ToString()), "Animal");
                if (micro != null)
                {
                    if (searchanimal.Microchips.Count != 0 || searchanimal.Microchips != null)
                    {

                    }
                    else
                    {
                        foreach (var item in searchanimal.Microchips)
                        {
                            item.Animal_Microchip_Code = micro.Animal_Microchip_Code;
                            item.Implanters_PIN_Number = micro.Implanters_PIN_Number;
                            item.Owner_Name = micro.Owner_Name;
                            item.Owner_Contact_Number = micro.Owner_Contact_Number;
                            item.Owner_Address = micro.Owner_Address;
                            item.Date_of_Implant = micro.Date_of_Implant;

                        }
                    }

                }
                db.SaveChanges();
                TempData["SuccessMessage"] = "Successfully";
            }
            catch (Exception e)
            {
                ViewBag.err = e.Message;
                throw new Exception("Something Went Wrong!");
            }


            return Content("");
        }

        public ActionResult AddAnimalType()
        {

            return View();
        }
        [HttpPost]
        public ContentResult AddAnimalType(string price, string Animal_type_Name)
        {
            try
            {
                Animal_Type an = new Animal_Type();
                List<Animal_Type> Animal_Type = new List<Animal_Type>();
                Animal_Type = db.Animal_Type.ToList();
                if (Animal_Type.Count != 0)
                {
                    int count = 0;
                    foreach (var item in Animal_Type)
                    {
                        if (item.Animal_Type_Name == Animal_type_Name)
                        {
                            count++;
                            TempData["DeleteMessage"] = "There is a duplicate Animal Type already";
                            return Content("");
                        }

                    }
                    if (count == 0)
                    {

                        an.Animal_Type_Name = Animal_type_Name;
                        an.Price = Convert.ToInt32(price);
                        db.Animal_Type.Add(an);
                        db.SaveChanges();
                        flex.CreateAuditTrail(Convert.ToInt32(Session["ID"].ToString()), "Animal Type");
                        TempData["SuccessMessage"] = "Successfully Stored";
                    }
                }
                else
                {

                    db.Animal_Type.Add(an);
                    db.SaveChanges();
                    flex.CreateAuditTrail(Convert.ToInt32(Session["ID"].ToString()), "Animal Type");
                    TempData["SuccessMessage"] = "Successfully Stored";
                }
                return Content("");
            }
            catch (Exception e)
            {
                TempData["EditMessage"] = "There was an Error with network please try again: " + e.Message;
                return Content("");
            }

        }

        public ActionResult MainatainAnimalType(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("SearchAnimalType", "Animal");
            }
            Animal_Type animal_type = db.Animal_Type.Find(id);
            if (animal_type == null)
            {
                return RedirectToAction("SearchAnimalType", "Animal");
            }
            return View(animal_type);
        }
        [HttpPost]
        public ContentResult MainatainAnimalType(Animal_Type animal_Type)
        {

            try
            {
                Animal_Type searchaniaml = db.Animal_Type.Find(animal_Type.Animal_Type_ID);
                if (searchaniaml == null)
                {
                    return Content("");
                }
                else
                {
                    searchaniaml.Animal_Type_Name = animal_Type.Animal_Type_Name;
                    searchaniaml.Price = animal_Type.Price;
                    db.SaveChanges();
                    flex.UpdateAuditTrail(Convert.ToInt32(Session["ID"].ToString()), "Animal Type");
                }
            }
            catch (Exception e)
            {
                TempData["EditMessage"] = e.Message;
                return Content("");
            }

            return Content("");

        }

        public ActionResult SearchAnimalType()
        {

            List<Animal_Type> animal_Types = new List<Animal_Type>();
            try
            {
                animal_Types = db.Animal_Type.ToList();
                if (animal_Types.Count == 0)
                {

                }
                return View(animal_Types);
            }
            catch (Exception e)
            {
                TempData["EditMessage"] = "there was a network error: " + e.Message;
                return View();
            }
        }
        [HttpPost]
        public ActionResult SearchAnimalType(string search)
        {
            try
            {
                if (!(search == ""))
                {
                    db.Database.CommandTimeout = 300;
                    var animaltype = db.Animal_Type.Where(z => z.Animal_Type_Name.StartsWith(search)).ToList();
                    if (animaltype == null)
                    {
                        return RedirectToAction("SearchAnimalType");
                    }
                    List<Animal_Type> animals = new List<Animal_Type>();
                    animals = animaltype;

                    return View("SearchAnimalType", animals);

                }
                TempData["SuccessMessage"] = "Enter Valid Details";
                return RedirectToAction("SearchAnimalType");
            }
            catch (Exception)
            {
                throw new Exception("Something Wrong");
            }


        }
        public ActionResult AddBreedType()
        {
            try
            {
                innovation.animalTypes = db.Animal_Type.ToList();
                return View(innovation);
            }
            catch (Exception e)
            {
                TempData["EditMessage"] = e.Message + "";
                throw;
            }
        }
        [HttpPost]
        public ContentResult AddBreedType(Animal_Breed animal_breed)
        {

            try
            {

                List<Animal_Breed> Animal_Breeds = new List<Animal_Breed>();
                Animal_Breeds = db.Animal_Breed.ToList();
                if (Animal_Breeds.Count != 0)
                {
                    int count = 0;
                    foreach (var item in Animal_Breeds)
                    {
                        if (item.Animal_Breed_Name == animal_breed.Animal_Breed_Name)
                        {
                            count++;
                            TempData["EditMessage"] = "There is a duplicate Animal Breed Already";
                            return Content("");
                        }

                    }
                    if (count == 0)
                    {
                        db.Animal_Breed.Add(animal_breed);
                        db.SaveChanges();
                        flex.CreateAuditTrail(Convert.ToInt32(Session["ID"].ToString()), "Breed Type");
                    }
                }
                else
                {

                    db.Animal_Breed.Add(animal_breed);
                    db.SaveChanges();
                    flex.CreateAuditTrail(Convert.ToInt32(Session["ID"].ToString()), "Breed Type");

                }

            }
            catch (Exception e)
            {
                ViewBag.errorMessage = "There was an Error with network please try again: " + e.Message;
                return Content("");
            }
            return Content("");
        }

        public ActionResult MaintainBreedType(int? id)
        {
            try
            {
                if (id != null)
                {
                    innovation.animalTypes = db.Animal_Type.ToList();
                    var breed = db.Animal_Breed.Find(id);
                    if (breed == null)
                    {
                        return RedirectToAction("SearchBreedType", "Animal");
                    }
                    innovation.breed = breed;
                    return View(innovation);
                }
                return RedirectToAction("SearchBreedType", "Animal");
            }
            catch (Exception e)
            {
                ViewBag.err = e.Message;
                return RedirectToAction("SearchBreedType", "Animal");
            }
        }
        [HttpPost]
        public ContentResult MaintainBreedType(Animal_Breed animal_breed)
        {

            try
            {
                Animal_Breed searchbreed = db.Animal_Breed.Find(animal_breed.Animal_Breed_ID);
                if (searchbreed == null)
                {
                    return Content("");
                }
                else
                {
                    searchbreed.Animal_Breed_Name = animal_breed.Animal_Breed_Name;
                    searchbreed.Animal_Breed_Description = animal_breed.Animal_Breed_Description;
                    searchbreed.Animal_Type_ID = animal_breed.Animal_Type_ID;
                    db.SaveChanges();
                    flex.UpdateAuditTrail(Convert.ToInt32(Session["ID"].ToString()), "Breed Type");
                }
            }
            catch (Exception e)
            {
                ViewBag.err = e.Message;
                return Content("");
            }
            return Content("");
        }


        public ActionResult SearchBreedType()
        {
            List<Animal_Breed> breeds = new List<Animal_Breed>();
            try
            {
                breeds = db.Animal_Breed.ToList();
                if (breeds.Count == 0)
                {

                }
                return View(breeds);
            }
            catch (Exception e)
            {
                TempData["EditMessage"] = "there was a network error: " + e.Message;
                return View();
            }
        }
        public ActionResult DeleteBreedType(int? id)
        {

            if (id != null)
            {
                Animal_Breed breeds = db.Animal_Breed.Find(id);
                int ani = breeds.Animals.Count();
                int count = breeds.CrossBreeds.Count();
                if (count != 0 || ani != 0)
                {
                    //you cant delete becasue its referenced to another table
                    ViewBag.err = "You can not delete this";
                    return RedirectToAction("SearchBreedType");
                }
                else
                {
                    db.Animal_Breed.Remove(breeds);
                    db.SaveChanges();
                    flex.DeleteAuditTrail(Convert.ToInt32(Session["ID"].ToString()), "Breed Type");
                    return RedirectToAction("SearchBreedType");
                }
            }
            return RedirectToAction("SearchBreedType");

        }

        public ActionResult DeleteAnimal(int? id)
        {

            try
            {
                if (id != 0)
                {
                    Animal animal = db.Animals.Find(id);
                    int status = Convert.ToInt32(animal.Animal_Status_ID);

                    //kennel
                    //adoption
                    //status - Adoption
                    if (status == 3 || status == 5)
                    {
                        //you cant delete becasue its referenced to another table
                        ViewBag.err = "You can not delete this";
                        return RedirectToAction("SearchAnimal");
                    }
                    else
                    {
                        if (animal.Microchips.Count != 0)
                        {
                            foreach (var item in animal.Microchips)
                            {
                                db.Microchips.Remove(item);
                                db.SaveChanges();
                            }

                        }
                        if (animal.CrossBreeds.Count != 0)
                        {
                            foreach (var item in animal.CrossBreeds)
                            {
                                db.CrossBreeds.Remove(item);
                                db.SaveChanges();
                            }
                        }
                        db.Animals.Remove(animal);
                        db.SaveChanges();
                        flex.DeleteAuditTrail(Convert.ToInt32(Session["ID"].ToString()), "Animal");
                        return RedirectToAction("SearchAnimal");
                    }
                }
                return RedirectToAction("SearchAnimal");
            }
            catch (Exception e)
            {
                TempData["Success"] = e.Message;
                return RedirectToAction("SearchAnimal");
            }

        }
        public ActionResult DeleteAnimalType(int? id)
        {

            if (id != null)
            {
                Animal_Type animal_type = db.Animal_Type.Find(id);
                int count = animal_type.Animal_Breed.Count();
                if (count != 0)
                {
                    //you cant delete becasue its referenced to another table
                    ViewBag.err = "You can not delete this";
                    return RedirectToAction("SearchAnimalType");
                }
                else
                {
                    db.Animal_Type.Remove(animal_type);
                    db.SaveChanges();
                    flex.DeleteAuditTrail(Convert.ToInt32(Session["ID"].ToString()), "Animal Type");
                    return RedirectToAction("SearchAnimalType");
                }
            }
            return RedirectToAction("SearchAnimalType");

        }

        public ActionResult ClaimAnimal()
        {
            try
            {
                db.Database.CommandTimeout = 300;
                innovation.animals = db.Animals.Where(z => z.Animal_Status_ID == 1 || z.Animal_Status_ID == 4).ToList();
                return View(innovation);
            }
            catch (Exception e)
            {
                ViewBag.err = e.Message;
                return RedirectToAction("SearchAnimal");

            }

        }

        public ActionResult RemoveClaimAnimal(int id)
        {
            try
            {
                if (id != 0)
                {
                    Animal animal = db.Animals.Find(id);
                    int status = Convert.ToInt32(animal.Animal_Status_ID);

                    //kennel
                    //adoption
                    //status - Adoption
                    if (status == 3 || status == 5)
                    {
                        //you cant delete becasue its referenced to another table
                        ViewBag.err = "You can not delete this";
                        return RedirectToAction("SearchAnimal");
                    }
                    else
                    {
                        if (animal.Microchips.Count != 0)
                        {
                            foreach (var item in animal.Microchips)
                            {
                                db.Microchips.Remove(item);
                                db.SaveChanges();
                            }

                        }
                        if (animal.CrossBreeds.Count != 0)
                        {
                            foreach (var item in animal.CrossBreeds)
                            {
                                db.CrossBreeds.Remove(item);
                                db.SaveChanges();
                            }
                        }
                        db.Animals.Remove(animal);
                        db.SaveChanges();
                        flex.DeleteAuditTrail(Convert.ToInt32(Session["ID"].ToString()), "Animal");
                        return RedirectToAction("SearchAnimal");
                    }
                }
                return RedirectToAction("SearchAnimal");
            }
            catch (Exception e)
            {
                TempData["Success"] = e.Message;
                return RedirectToAction("SearchAnimal");
            }


            //return RedirectToAction("ClaimAnimal");
        }
    }
}