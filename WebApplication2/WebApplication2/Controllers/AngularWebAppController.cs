using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication2.Controllers
{
    [RoutePrefix("api/Employee")]
    public class AngularWebAppController : ApiController
    {
        masterEntities entity = new masterEntities();

        [HttpGet]
        [Route("AllEmployeeDetail")]
        public IHttpActionResult GetEmployees()
        {
            try
            {
                var result = (from tblEmp in entity.tblEmployeeMasters
                               join tblCountry in entity.CountryMasters on tblEmp.CountryId equals tblCountry.CountryId
                               join tblState in entity.StateMasters on tblEmp.StateId equals tblState.StateId
                               join tblCity in entity.CityMasters on tblEmp.Cityid equals tblCity.Cityid
                               select new
                               {
                                    EmpId = tblEmp.EmpId,
                                    FirstName = tblEmp.FirstName,
                                    LastName = tblEmp.LastName,
                                    DateofBirth = tblEmp.DateofBirth,
                                    EmailId = tblEmp.EmailId,
                                    Gender = tblEmp.Gender,
                                    CountryId = tblEmp.CountryId,
                                    StateId = tblEmp.StateId,
                                    Address = tblEmp.Address,
                                    PinCode = tblEmp.Pincode,
                                    Country = tblCountry.CountryName,
                                    State = tblState.StateName,
                                    City =tblCity.CityName
                               }
                ).ToList();

            return Ok(result);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet]
        [Route("GetEmployeeDetailsById/{employeeId}")]
        public IHttpActionResult GetEmployeeById(string employeeId)
        {
            try 
            {
                tblEmployeeMaster objEmp = new tblEmployeeMaster();
                int ID =  Convert.ToInt32(employeeId);
                try
                {
                    objEmp = entity.tblEmployeeMasters.Find(ID);
                    if (objEmp == null)
                    {
                        return NotFound();
                    }  
                }
                catch (Exception)
                {
                    throw;
                }
                return Ok(objEmp);
            }
            catch(Exception)
            {
                throw;
            }
           
        }

        [HttpPost]
        [Route("InsertEmployeeDetails")]
        public IHttpActionResult PostEmployee(tblEmployeeMaster data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                try
                {
                    data.DateofBirth = data.DateofBirth.HasValue ? 
                                        data.DateofBirth.Value.AddDays(1) : 
                                        (DateTime?)null;
                    entity.tblEmployeeMasters.Add(data);
                    entity.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
                return Ok(data);
            }
            catch(Exception)
            {
                throw;
            }
        }

        [HttpPut]
        [Route("UpdateEmployeeDetails")]
        public IHttpActionResult PutEmployeeMaster(tblEmployeeMaster employee)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                try
                {
                    tblEmployeeMaster objEmployee = new tblEmployeeMaster();
                    objEmployee = entity.tblEmployeeMasters.Find(employee.EmpId);
                    
                    if (objEmployee != null)
                    {
                        objEmployee.FirstName = employee.FirstName;
                        objEmployee.LastName = employee.LastName;
                        objEmployee.Address = employee.Address;
                        objEmployee.EmailId = employee.EmailId;
                        objEmployee.DateofBirth = employee.DateofBirth;
                        objEmployee.Gender = employee.Gender;
                        objEmployee.CountryId = employee.CountryId;
                        objEmployee.StateId = employee.StateId;
                        objEmployee.Cityid = employee.Cityid;
                        objEmployee.Pincode = employee.Pincode;
                    }
                    this.entity.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
                return Ok(employee);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [HttpDelete]
        [Route("DeleteEmployeeDetail")]
        public IHttpActionResult DeleteEmployeeDelete(int id)
        {
            try
            {
                tblEmployeeMaster employee = entity.tblEmployeeMasters.Find(id);
                if (employee == null)
                {
                    return NotFound();
                }
                entity.tblEmployeeMasters.Remove(employee);
                entity.SaveChanges();
                return Ok(employee);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("Country")]
        public IQueryable<CountryMaster> GetCountries()
        {
            try
            {
                return entity.CountryMasters;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CountryMaster> CountryData()
        {
           try
            {
                return entity.CountryMasters.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("Country/{CountryId}/State")]
        [HttpGet]
        public List<StateMaster> StateData(int CountryId)
        {
            try
            {
                return entity.StateMasters.Where(s => s.CountryId == CountryId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        [Route("State/{StateId/City}")]
        [HttpGet]
        public List<CityMaster> CityData(int StateId)
        {
            try
            {
                return entity.CityMasters.Where(s => s.StateId == StateId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("DeteleRecord")]
        public IHttpActionResult DeleteRecord(List<tblEmployeeMaster> user)
        {
            string result = "";
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                result = DeleteData(user);
            }
            catch (Exception)
            {

                throw;
            }
            
            return Ok(result);
        }

        public string DeleteData(List<tblEmployeeMaster> users)
        {
            string str = "";
            try
            {
                foreach (var user in users)
                {
                    tblEmployeeMaster objEmp = new tblEmployeeMaster();
                    objEmp.EmpId = user.EmpId;
                    objEmp.FirstName = user.FirstName;
                    objEmp.LastName = user.LastName;
                    objEmp.Address = user.Address;
                    objEmp.EmailId = user.EmailId;
                    objEmp.DateofBirth = user.DateofBirth.HasValue ?
                                         user.DateofBirth.Value.AddDays(1):
                                         (DateTime?)null;
                    objEmp.Gender = user.Gender;
                    objEmp.CountryId = user.CountryId;
                    objEmp.StateId = user.StateId;
                    objEmp.Cityid = user.Cityid;
                    objEmp.Pincode = user.Pincode;

                    var entry = entity.Entry(objEmp);
                    if (entry.State == EntityState.Detached)
                    {
                        entity.tblEmployeeMasters.Attach(objEmp);
                    }
                    entity.tblEmployeeMasters.Remove(objEmp);
                }
    
                int i = entity.SaveChanges();
                
                if (i > 0)
                {
                    str = "Records has been deleted";
                }
                else
                {
                    str = "Records deletion has been faild";
                }
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
           return str;
        }
    }
}
