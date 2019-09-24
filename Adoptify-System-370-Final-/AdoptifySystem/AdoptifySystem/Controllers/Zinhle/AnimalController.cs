﻿using AdoptifySystem.Models;
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
        [HttpGet]
        public JsonResult animalbreed(int categoryID)
        {
            //List<Animal_Breed> breeds = new List<Animal_Breed>();
            

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
                return RedirectToAction("Index","Employees");

            }

        }

        public ContentResult test(string Cross_Breed, Animal animal, string breeds, Microchip micro, HttpPostedFileBase animalPicture,FormCollection form)
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
                        return Content("AddTemporaryAnimal");
                    }
                    animal.Animal_Status_ID = status.Animal_Status_ID;
                    db.Animals.Add(animal);
                    db.SaveChanges();
                }
                if (Cross_Breed == "True")
                {
                    // Split authors separated by a comma followed by space  
                    string[] breed = breeds.Split(',');
                        foreach (var item in breed)
                    {
                        CrossBreed cross = new CrossBreed();
                        cross.Animal_ID = animal.Animal_ID;
                        cross.Animal_Breed_ID = Convert.ToInt32(item);
                    }
                }
                else {
                    CrossBreed cross = new CrossBreed();
                    cross.Animal_ID = animal.Animal_ID;
                    cross.Animal_Breed_ID = Convert.ToInt32(breeds);
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
                return Content("SearchAnimal");

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
                return RedirectToAction("AddTemporaryAnimal", "Animal");
            }


            return View(animals);
        }
        [HttpPost]
        public ActionResult SearchAnimal(string search)
        {
            
            if (!(search == ""))
            {
                var animallist = db.Animals.Where(z=>z.Animal_Name.Equals(search)).ToList();
                if (animallist == null)
                {
                    return RedirectToAction("SearchAnimal");
                }
                List<Animal> animals = new List<Animal>();
                animals = animallist;
                
                return View("SearchAnimal",animals);

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
                var micro = db.Microchips.Where(z=>z.Animal_ID ==animals.Animal_ID).FirstOrDefault();
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
                    return Content("MaintainDonationType", "Donations");
                }
            
            
            return Content("");
        }

        public ActionResult AddAnimalType()
        {

            return View();
        }
        [HttpPost]
        public ActionResult AddAnimalType(Animal_Type animal_type, string button)
        {
            if (button == "Save")
            {
                try
                {

                    List<Animal_Type> Animal_Type = new List<Animal_Type>();
                    Animal_Type = db.Animal_Type.ToList();
                    if (Animal_Type.Count != 0)
                    {
                        int count = 0;
                        foreach (var item in Animal_Type)
                        {
                            if (item.Animal_Type_Name == animal_type.Animal_Type_Name)
                            {
                                count++;
                                TempData["DeleteMessage"] = "There is a duplicate Animal Type already";
                                return View();
                            }

                        }
                        if (count == 0)
                        {
                            db.Animal_Type.Add(animal_type);
                            db.SaveChanges();
                            TempData["SuccessMessage"] = "Successfully Stored";
                        }
                    }
                    else
                    {

                        db.Animal_Type.Add(animal_type);
                        db.SaveChanges();

                        TempData["SuccessMessage"] = "Successfully Stored";
                    }

                }
                catch (Exception e)
                {
                    TempData["EditMessage"] = "There was an Error with network please try again: " + e.Message;
                    return View();
                }

            }
            else if (button == "Cancel")
            {
                
                return RedirectToAction("SearchAnimalType", "Animal");
            }
            return RedirectToAction("SearchAnimalType", "Animal");
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
        public ActionResult MainatainAnimalType(Animal_Type animal_Type, string button)
        {
            if (button == "Save")
            {
                try
                {
                    Animal_Type searchaniaml = db.Animal_Type.Find(animal_Type.Animal_Type_ID);
                    if (searchaniaml == null)
                    {
                        return HttpNotFound();
                    }
                    else
                    {
                        db.Entry(searchaniaml).CurrentValues.SetValues(animal_Type);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    TempData["EditMessage"] = e.Message;
                    return RedirectToAction("MaintainDonationType", "Donations");
                }
            }
            else if (button == "Cancel")
            {

                return RedirectToAction("SearchAnimalType", "Animal");
            }
            return RedirectToAction("SearchAnimalType", "Animal");

        }

        public ActionResult SearchAnimalType()
        {
           
            List<Animal_Type> animal_Types = new List<Animal_Type>();
            try
            {
                animal_Types = db.Animal_Type.ToList();
                if(animal_Types.Count == 0)
                {

                }
                return View(animal_Types);
            }
            catch (Exception e)
            {
                TempData["EditMessage"] = "there was a network error: "+ e.Message;
                return View();
            }
        }
        [HttpPost]
        public ActionResult SearchAnimalType(string search)
        {
            return View();
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
        public ActionResult AddBreedType(Animal_Breed animal_breed, string button)
        {
            ViewBag.errorMessage = "";
            if (button == "Save")
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
                                return View();
                            }

                        }
                        if (count == 0)
                        {
                            db.Animal_Breed.Add(animal_breed);
                            db.SaveChanges();
                        }
                    }
                    else
                    {

                        db.Animal_Breed.Add(animal_breed);
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

                return RedirectToAction("SearchBreedType", "Animal");
            }
            return RedirectToAction("SearchBreedType", "Animal");
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
        public ActionResult MaintainBreedType(Animal_Breed animal_breed, string button)
        {
            if (button == "Save")
            {
                try
                {
                    Animal_Breed searchbreed = db.Animal_Breed.Find(animal_breed.Animal_Breed_ID);
                    if (searchbreed == null)
                    {
                        return HttpNotFound();
                    }
                    else
                    {
                        db.Entry(searchbreed).CurrentValues.SetValues(animal_breed);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewBag.err = e.Message;
                    return RedirectToAction("SearchBreedType", "Animal");
                }
            }
            else if (button == "Cancel")
            {

                return RedirectToAction("SearchBreedType", "Animal");
            }
            return RedirectToAction("SearchBreedType", "Animal");
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
                int count = breeds.CrossBreeds.Count();
                if (count != 0)
                {
                    //you cant delete becasue its referenced to another table
                    ViewBag.err = "You can not delete this";
                    return RedirectToAction("SearchDonationType");
                }
                else
                {
                    db.Animal_Breed.Remove(breeds);
                    db.SaveChanges();
                    return RedirectToAction("SearchBreedType");
                }
            }
            return RedirectToAction("SearchBreedType");

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
                    return RedirectToAction("SearchAnimalType");
                }
            }
            return RedirectToAction("SearchAnimalType");

        }

        /*public ActionResult ClaimAnimal()
         {
             try
             {
                 innovation.animals = db.Animals.Select( x=> new Animal { Animal_Name = x.Animal_Name}).ToList();


                 return View(innovation);
             }
             catch (Exception e)
             {
                 ViewBag.err = e.Message;
                 return RedirectToAction("Index", "Employees");

             }

         }

         [HttpPost]
         public ActionResult ClaimAnimal()
         {



             Animal animal = new Animal();
             Animal_Status status = db.Animal_Status.Where(zz => zz.Animal_Status_Name == "Temporary").FirstOrDefault();
             animal.Animal_Status_ID = status.Animal_Status_ID;
             db.Animals.Add(animal);
             db.SaveChanges();


         }*/
    }
}