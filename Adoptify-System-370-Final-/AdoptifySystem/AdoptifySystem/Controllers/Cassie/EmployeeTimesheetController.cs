using AdoptifySystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdoptifySystem.Controllers.Cassie
{
    //public class EmployeeTimesheetController : Controller
    //{
    //    [HttpGet]
    //    // GET: EmployeeTimesheet
    //    public ActionResult Index()
    //    {
    //        TimesheetVM vm = new TimesheetVM();

    //        //DropDownList
    //        vm.employee = GetEmployees(0);

    //        //Default values 
    //        vm.DateFrom = new DateTime(2019, 10, 1);
    //        vm.DateTo = new DateTime(2019, 10, 31);


    //        return View(vm);
    //    }
    //}
    //private SelectList GetEmployees(int selected)
    //{
    //    using (Wollies_ShelterEntities db = new Wollies_ShelterEntities())
    //    {
    //        db.Configuration.ProxyCreationEnabled = false;

    //        //SelectListItem for each Employee
    //        var employee = db.Employees.Select(x => new SelectListItem
    //        {
    //            Value = x.Emp_ID.ToString(),
    //            Text = x.Emp_Name
    //        }).ToList();

    //        if (selected == 0)
    //            return new SelectList(employee, "Value", "Text");
    //        else
    //            return new SelectList(employee, "Value", "Text", selected);
    //    }
    //}

}