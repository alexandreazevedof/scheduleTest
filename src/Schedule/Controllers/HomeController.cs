using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Reflection;
using EF6.Data;
using System.Data.Entity;

namespace Schedule.Controllers
{
    public class HomeController : Controller
    {
        private ScheduleContext _context;
        public IActionResult Index()
        {
            return View();
        }
        public List<DefaultSchedule> GetData()
        {


            List<DefaultSchedule> datas =  _context.DefaultSchedule
                .Include(c => c.Customers)
                .Include(c => c.Equipments)
                .Include(c =>c.Resources)
                .Include(c => c.Employees)
                .ToList();
            return datas;
        }

        public List<CategorizeSettings> GetCatogories()
        {
            List<CategorizeSettings> data = _context.CategorizeSettings.ToList();
            return data;
        }

        public List<Employee> GetEmployees()
        {
            List<Employee> data = _context.Employees.ToList();
            return data;
        }

        public List<Resource> GetResources()
        {
            List<Resource> data = _context.Resources.ToList();
            return data;
        }
        
       
        public List<Customer> GetCustomers(){
            List<Customer> data = _context.Customers.ToList();
            return data;
        }

        public JsonResult GetEquipments()
        {
            var data = _context.Equipments;

            return Json(data);
        }

        //Filter data based on desired view: Start Date, Due Date or Issue Date
        public List<DefaultSchedule> filterData([FromBody] Filter request)
        {
            List<DefaultSchedule> result;

            if (request.FilteredCategoryList == null) // none of the categories is selected
            {
                result = _context.DefaultSchedule.Where(item => item.Id == 0).ToList(); // empty result
            }
            else
            {
                List<DefaultSchedule> items = _context.DefaultSchedule.ToList();
                //filter based on categories selected
                result = items.Where(item => request.FilteredCategoryList.Any(c => c.Id.Equals(Convert.ToInt32(item.Category)))).ToList();

            }
            //customer filter
            if (request.FilteredCustomerList != null)
            {
                result = result.Where(item => request.FilteredCustomerList.Any(c => c.Id.Equals(Convert.ToInt32(item.CustomerString)))).ToList();
            }
            //resource filter
            if(request.FilteredResourceList != null)
            {
                result = result.Where(item => request.FilteredResourceList.Any(c => c.Id.Equals(Convert.ToInt32(item.ResourceString)))).ToList();
            }
            
            //employee filter
            if(request.FilteredEmployeeList != null)
            {
                result = result.Where(item => request.FilteredEmployeeList.Any(c => c.Id.Equals(Convert.ToInt32(item.EmployeeString)))).ToList();
            }
            

            //filter based on DateView selected
            for (var i = 0; i < result.Count; i++)
            {
                switch (request.Date)
                {
                    case "Start Date":
                        result[i].StartTime = result[i].StartDate;
                        result[i].EndTime = result[i].StartDate;
                        break;
                    case "Due Date":
                        result[i].StartTime = result[i].DueDate;
                        result[i].EndTime = result[i].DueDate;
                        break;
                    case "Issue Date":
                        result[i].StartTime = result[i].IssueDate;
                        result[i].EndTime = result[i].IssueDate;
                        break;
                    default: //scheduleFor
                        result[i].StartTime = result[i].ScheduleFor;
                        result[i].EndTime = result[i].ScheduleFor;
                        break;
                }
            }
    
            return result;
        }

        //public List<DefaultSchedule> filterDataByCategory([FromBody] Filter request)
        //{
            
        //    if (request.FilteredCategoryList == null)
        //    {
        //        return _context.DefaultSchedule.Where(item => item.Id == 0).ToList();
        //    }
        //    List<DefaultSchedule> items = _context.DefaultSchedule.ToList();

        //    List<DefaultSchedule> result = items.Where(item => request.FilteredCategoryList.Any(category => category.Id.Equals(Convert.ToInt32(item.Category)))).ToList();
        //    return result;

        //}

        public class Filter
        {
            public string Date { get; set; }
            public string Category { get; set; }
            public bool isChecked { get; set; }

            public CategorizeSettings[] FilteredCategoryList { get; set; }
            public Customer[] FilteredCustomerList { get; set; }
            public Resource[] FilteredResourceList { get; set; }
            public Employee[] FilteredEmployeeList { get; set; }

        }
        public List<DefaultSchedule> Add([FromBody] EditParams param)
        {
            DefaultSchedule appoint = new DefaultSchedule();
            
            
            object result;
            //object startDate =null;
            var value = param.value;
            foreach (var fieldName in value.GetType().GetProperties())
            {
                var newName = fieldName.ToString().Split(null);

               
                if (newName[1] == "Id")
                {
                    //database automatic generate ids
                    continue;
                }else if(newName[1] == "Equipments" || newName[1] == "Customers" || newName[1] == "Resources" || newName[1] == "Employees")
                {
                    //Those list will be set based on their Strings.
                    continue;
                }
                else if (newName[1] == "ScheduleFor" || newName[1] == "StartDate" || newName[1] == "DueDate" || newName[1] == "IssueDate")
                {
                    
                    result = Convert.ToDateTime(fieldName.GetValue(value));
                }
                else
                {
                    result = fieldName.GetValue(value);
                }
                fieldName.SetValue(appoint, result);

                if (newName[1] == "EquipmentString")
                {
                    string v = fieldName.GetValue(value).ToString();

                    for (int i = 0; i < v.Length; i = i + 2)
                    {
                        int id = Convert.ToInt32(v[i].ToString());
                        var filterData = _context.Equipments.Where(c => c.Id == id);
                        if (filterData.Count() > 0)
                        {
                            var equi = _context.Equipments.Single(A => A.Id == id);
                            appoint.Equipments.Add(equi);
                        }


                    }
                    continue;
                }else if(newName[1] == "CustomerString")
                {
                    int id = Convert.ToInt32(fieldName.GetValue(value).ToString());
                    var customer = _context.Customers.Single(c => c.Id == id);
                    appoint.Customers.Add(customer);
                }


            }
            _context.DefaultSchedule.Add(appoint);
            _context.SaveChanges();
            List<DefaultSchedule> datas = _context.DefaultSchedule.Take(100).ToList();
            return datas;
        }
        public List<DefaultSchedule> Update([FromBody] EditParams param)
        {
            var value = param.action == "update" ? param.value : param.changed[0];
            var filterData = _context.DefaultSchedule.Where(c => c.Id == value.Id);
            if (filterData.Count() > 0)
            {
                DefaultSchedule appoint = _context.DefaultSchedule.Single(A => A.Id == value.Id);
                appoint.Number = value.Number;
                appoint.Job = value.Job;
                appoint.ScheduleFor = Convert.ToDateTime(value.ScheduleFor);
                appoint.StartDate = Convert.ToDateTime(value.StartDate);
                appoint.DueDate = Convert.ToDateTime(value.DueDate);
                appoint.IssueDate = Convert.ToDateTime(value.IssueDate);
                appoint.Category = value.Category;
                appoint.Resources = value.Resources;
                appoint.Employees = value.Employees;
                appoint.Customers = value.Customers;
                appoint.EmployeeString = value.EmployeeString;
                appoint.ResourceString = value.ResourceString;
                appoint.CustomerString = value.CustomerString;
                //appoint.Equipment = value.Equipment;
                appoint.StartTime = Convert.ToDateTime(value.StartTime);
                appoint.EndTime = Convert.ToDateTime(value.EndTime);
                appoint.Subject = value.Subject;
                appoint.Description = value.Description;
                appoint.Recurrence = value.Recurrence;
                appoint.AllDay = value.AllDay;
                appoint.RecurrenceRule = value.RecurrenceRule;
            }
            _context.SaveChanges();
            List<DefaultSchedule> datas = _context.DefaultSchedule.Take(500).ToList();
            return datas;
        }
        public List<DefaultSchedule> Remove([FromBody] EditParams param)
        {
            DefaultSchedule app = _context.DefaultSchedule.Where(c => c.Id == Convert.ToInt32(param.key)).FirstOrDefault();
            if (app != null) _context.DefaultSchedule.Remove(app);
            _context.SaveChanges();
            List<DefaultSchedule> datas = _context.DefaultSchedule.Take(500).ToList();
            return datas;
        }
        public List<DefaultSchedule> Batch([FromBody] EditParams param)
        {
            if (param.action == "insert" || (param.action == "batch" && (param.added.Count > 0))) // this block of code will execute while inserting the appointments
            {
                DefaultSchedule appoint = new DefaultSchedule();
                object result;
                if (param.action == "insert")
                {
                    var value = param.value;
                    foreach (var fieldName in value.GetType().GetProperties())
                    {
                        var newName = fieldName.ToString().Split(null);
                        if (newName[1] == "Id")
                        {
                            //database automatic generate ids
                            continue;
                        }
                        else if (newName[1] == "ScheduleFor" || newName[1] == "StartDate" ||
                            newName[1] == "DueDate" || newName[1] == "IssueDate" || newName[1] == "StartTime" ||
                            newName[1] == "EndTime")
                        {
                            result = Convert.ToDateTime(fieldName.GetValue(value));
                        }
                        else
                        {
                            result = fieldName.GetValue(value);
                            
                        }
                        fieldName.SetValue(appoint, result);

                    }
                    _context.DefaultSchedule.Add(appoint);
                }
                else
                {
                    foreach (var item in param.added.Select((x, i) => new { Value = x, Index = i }))
                    {
                        var value = item.Value;
                        foreach (var fieldName in value.GetType().GetProperties())
                        {
                            var newName = fieldName.ToString().Split(null);
                            if (newName[1] == "Id")
                            {
                                //database automatic generate ids
                                continue;
                            }
                            else if (newName[1] == "StartTime" || newName[1] == "EndTime")
                            {
                                result = Convert.ToDateTime(fieldName.GetValue(value));
                            }
                            else
                            {
                                result = fieldName.GetValue(value);
                            }
                            fieldName.SetValue(appoint, result);
                        }
                        _context.DefaultSchedule.Add(appoint);
                    }
                }
                _context.SaveChanges();
            }
            if ((param.action == "remove") || (param.action == "batch" && (param.deleted.Count > 0))) // this block of code will execute while removing the appointment
            {
                if (param.action == "remove")
                {
                    DefaultSchedule app = _context.DefaultSchedule.Where(c => c.Id == Convert.ToInt32(param.key)).FirstOrDefault();
                    if (app != null) _context.DefaultSchedule.Remove(app);
                }
                else
                {
                    foreach (var a in param.deleted)
                    {
                        var app = _context.DefaultSchedule.ToList().Where(c => c.Id == Convert.ToInt32(a.Id)).FirstOrDefault();
                        if (app != null) _context.DefaultSchedule.Remove(app);
                    }
                }
                _context.SaveChanges();
            }
            if (param.action == "update" || (param.action == "batch" && (param.changed.Count > 0))) // this block of code will execute while updating the appointment
            {
                var value = param.action == "update" ? param.value : param.changed[0];
                var filterData = _context.DefaultSchedule.Where(c => c.Id == value.Id);
                if (filterData.Count() > 0)
                {
                    DefaultSchedule appoint = _context.DefaultSchedule.Single(A => A.Id == value.Id);
                    appoint.Number = value.Number;
                    appoint.Job = value.Job;
                    appoint.ScheduleFor = Convert.ToDateTime(value.ScheduleFor);
                    appoint.StartDate = Convert.ToDateTime(value.StartDate);
                    appoint.DueDate = Convert.ToDateTime(value.DueDate);
                    appoint.IssueDate = Convert.ToDateTime(value.IssueDate);
                    appoint.StartTime = Convert.ToDateTime(value.StartTime);
                    appoint.Category = value.Category;
                    appoint.Resources = value.Resources;
                    appoint.Employees = value.Employees;
                    appoint.Customers = value.Customers;
                    appoint.EmployeeString = value.EmployeeString;
                    appoint.ResourceString = value.ResourceString;
                    appoint.CustomerString = value.CustomerString;
                    //appoint.Equipment = value.Equipment;
                    appoint.EndTime = Convert.ToDateTime(value.EndTime);
                    appoint.Subject = value.Subject;
                    appoint.Recurrence = value.Recurrence;
                    appoint.AllDay = value.AllDay;
                    appoint.RecurrenceRule = value.RecurrenceRule;
                }
                _context.SaveChanges();
            }
            List<DefaultSchedule> datas = _context.DefaultSchedule.Take(500).ToList();
            return datas;
        }

        public HomeController(ScheduleContext context)
        {
            _context = context;
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
