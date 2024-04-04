using API_Foreignkey.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using Test2.Data;
using Test2.Models;

namespace API_Foreignkey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles ="Admin")]

    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("index")]
        public IActionResult Index()
        {
            var dep = _context.departments.ToList();
            return Ok(dep);
        }

        [HttpGet]
        [Route("skill")]
        public IActionResult skill()
        {
            var skl = _context.skills.ToList();
            return Ok(skl);
        }
        //[HttpGet]
        //[Route("{id}")]
        //public IActionResult skils(int id)
        //{
        //    var dep = _context.skills.Where(x => x.Sklid == id).FirstOrDefault();
        //    return Ok(dep);
        //}
        [HttpGet]
        [Route("{id}")]
        public IActionResult Employeees(int id)
        {
            var result = (from e in _context.employees
                          join d in _context.departments on e.DepId equals d.DepId
                          join s in _context.empSkills on e.EmpId equals s.EmpId
                          join sk in _context.skills on s.Sklid equals sk.Sklid
                          select new Responsemployee
                          {
                              id = e.EmpId,
                              Name = e.Name,
                              Salary = e.Salary,
                              DepName = d.DepName,
                              DepId = e.DepId,
                              Sklid = s.Sklid,
                              Sklname = sk.Sklname
                          }).Where(x => x.id == id);
            return Ok(result);
        }

        [HttpGet]
        [Route("Employee")]
        public IActionResult Employee()
        {
            var result = (from e in _context.employees
                          join d in _context.departments on e.DepId equals d.DepId
                          join s in _context.empSkills on e.EmpId equals s.EmpId
                          join sk in _context.skills on s.Sklid equals sk.Sklid
                          select new Responsemployee
                          {
                              id = e.EmpId,
                              Name = e.Name,
                              Salary = e.Salary,
                              DepName = d.DepName,
                              DepId = e.DepId,
                              Sklid = s.Sklid,
                              Sklname = sk.Sklname
                          }).ToList();

            //for(int i = 0; i < result.Count; i++)
            //{
            //    if (result[i].Sklid == 5)
            //    {
            //        result[i].Sklname = "c";
            //    }
            //    else if (result[i].Sklid == 6)
            //    {
            //        result[i].Sklname = "c#";
            //    }
            //    else if (result[i].Sklid == 7) 
            //    {
            //        result[i].Sklname = "java";
            //    }
            //    else if (result[i].Sklid == 9)
            //    {
            //        result[i].Sklname = "python";
            //    }
            //}
            // result = result.OrderBy(employee => employee.id).ToList();

            List<EmpReturn> empReturn = new List<EmpReturn>();
            int EmpID = 0;
            for (int j = 0; j < result.Count; j++)
            {

                if (result[j].id != EmpID)
                {
                    EmpReturn empReturn2 = new EmpReturn();
                    //ID = result[j].id;
                    EmpID = result[j].id;

                    empReturn2.Id = EmpID; //result.Select(x => x.id = EmpID).FirstOrDefault();
                    empReturn2.EmpId = EmpID;
                    empReturn2.Name = result.Where(employee => employee.id == EmpID).Select(employee => employee.Name).ToList()[0];
                    empReturn2.Salary = result.Where(employee => employee.id == EmpID).Select(employee => employee.Salary).ToList()[0];
                    empReturn2.DepName = result.Where(employee => employee.id == EmpID).Select(employee => employee.DepName).ToList()[0];
                    empReturn2.DepId = result.Where(employee => employee.id == EmpID).Select(employee => employee.DepId).ToList()[0];
                    var skills = result.Where(employee => employee.id == EmpID).Select(employee => employee.Sklname).ToList();

                    string skillName = "";
                    for (int i = 0; i < skills.Count; i++)
                    {
                        if (skillName == "")
                        {
                            skillName = skills[i].ToString();
                        }
                        else
                        {
                            skillName += "," + skills[i].ToString();
                        }
                    }
                    empReturn2.Sklname = skillName;
                    empReturn.Add(empReturn2);
                }
            }
            int EmpId = 0;
            if (result == null || result.Count == 0)
            {
                return BadRequest("No data found");
            }
            return Ok(empReturn);
        }
        [HttpPost]
        [Route("Create")]
        public IActionResult Create(EmployeeWithSkills employeeWithSkills)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {

                if (employeeWithSkills.Employee.Name == "" || employeeWithSkills.Employee.Name.ToLower() == "string" || employeeWithSkills.Employee.Salary == 0)
                {
                    serviceResponse.issuccess = false;
                    serviceResponse.message = "User not Found!";
                }
                else
                {
                    var dep = _context.departments.Any(d => d.DepId == employeeWithSkills.Employee.DepId);
                    var skil = employeeWithSkills.Sklids.All(skillId => _context.skills.Any(s => s.Sklid == skillId));
                    if (!dep || !skil)
                    {
                        return BadRequest("Invalid DepId or Sklid");
                    }
                    _context.employees.Add(employeeWithSkills.Employee);
                    _context.SaveChanges();

                    //var savedEmployee = _context.employees.FirstOrDefault(e => e.EmpId == employeeWithSkills.Employee.EmpId);
                    if (_context.employees != null)
                    {
                        foreach (var skillId in employeeWithSkills.Sklids)
                        {
                            EmpSkill empSkill = new EmpSkill
                            {
                                EmpId = employeeWithSkills.Employee.EmpId,
                                Sklid = skillId
                            };
                            _context.empSkills.Add(empSkill);
                        }
                    }
                    _context.SaveChanges();
                    serviceResponse.issuccess = true;
                    serviceResponse.message = "Data Saved Succefully";
                    //return Ok("Employee and Skills saved successfully");
                }
                return Ok(JsonConvert.SerializeObject(serviceResponse));
            }
            catch (Exception ex)
            {
                // Handle exception
                return StatusCode(500, "Failed to create employee record");
            }
        }
        //return BadRequest("Failed to create employee record");

        [HttpPut]
        [Route("Update")]
        public IActionResult Update(EmployeeWithSkills updatedEmployeeWithSkills)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                var existingEmployee = _context.employees.FirstOrDefault(e => e.EmpId == updatedEmployeeWithSkills.Employee.EmpId);

                if (existingEmployee == null)
                {
                    serviceResponse.issuccess = false;
                    serviceResponse.message = "User not Found!";
                }
                else
                {
                    // Update employee details
                    existingEmployee.Name = updatedEmployeeWithSkills.Employee.Name == "" || updatedEmployeeWithSkills.Employee.Name == "string" ? existingEmployee.Name : updatedEmployeeWithSkills.Employee.Name;
                    existingEmployee.Salary = updatedEmployeeWithSkills.Employee.Salary == 0 ? existingEmployee.Salary : updatedEmployeeWithSkills.Employee.Salary;
                    existingEmployee.DepId = updatedEmployeeWithSkills.Employee.DepId == 0 ? existingEmployee.DepId : updatedEmployeeWithSkills.Employee.DepId;

                    if (updatedEmployeeWithSkills.Sklids[0] != 0)
                    {
                        // Remove existing skills
                        var existingEmpSkills = _context.empSkills.Where(es => es.EmpId == updatedEmployeeWithSkills.Employee.EmpId).ToList();
                        _context.empSkills.RemoveRange(existingEmpSkills);
                        foreach (var skillId in updatedEmployeeWithSkills.Sklids)
                        {
                            EmpSkill empSkill = new EmpSkill
                            {
                                EmpId = updatedEmployeeWithSkills.Employee.EmpId,
                                Sklid = skillId
                            };
                            _context.empSkills.Update(empSkill);
                        }
                    }
                    _context.SaveChanges();
                    serviceResponse.issuccess = true;
                    serviceResponse.message = "Data Updated Succefully";
                }

                return Ok(JsonConvert.SerializeObject(serviceResponse));
            }
            catch (Exception ex)
            {
                // Handle exception
                return StatusCode(500, "An error occurred while updating employee record");
            }
        }

        [HttpDelete]
        [Route("delete/{empId}")]
        public IActionResult delete(int empId)   
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                var del = _context.employees.FirstOrDefault(x => x.EmpId == empId);
                if (del == null)
                {
                    serviceResponse.issuccess = false;
                    serviceResponse.message = "User not Found!";
                }
                else
                {
                    _context.employees.Remove(del);
                    _context.SaveChanges();
                    serviceResponse.issuccess = true;
                    serviceResponse.message = "Data Deleted Succefully";
                }
            }
            catch (Exception ex)
            {

            }
            return Ok(JsonConvert.SerializeObject(serviceResponse));
        }
    }
}