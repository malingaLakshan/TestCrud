using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestCrud.Data;
using TestCrud.Models;
using TestCrud.Models.Domain;

namespace TestCrud.Controllers
{
	public class EmployeeController : Controller
	{
		private readonly MVCDemoDBContext mvcDemoDBContext;
        public EmployeeController(MVCDemoDBContext mvcDemoDBContext)
        {
			this.mvcDemoDBContext = mvcDemoDBContext; 
        }

		[HttpGet]
        public async Task<IActionResult> Index()
        {
        var employeees = await mvcDemoDBContext.Employees.ToListAsync();
			return View(employeees);
        }

        [HttpGet]
		public IActionResult Add()
		{
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
			var employee = new Employee()
			{
				Id = Guid.NewGuid(),
				Name = addEmployeeRequest.Name,
				Email = addEmployeeRequest.Email,
				Salary = addEmployeeRequest.Salary,
				Department = addEmployeeRequest.Department
		
			};

			await mvcDemoDBContext.Employees.AddAsync(employee);
			await mvcDemoDBContext.SaveChangesAsync();
			return RedirectToAction("Index");
        }

		[HttpGet]
		public async Task<IActionResult> View(Guid id)
		{
			var employee = await mvcDemoDBContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

			if (employee != null)
			{
				var viewModel = new UpdateEmployeeViewModel()
				{
					Id = employee.Id,
					Name = employee.Name,
					Email = employee.Email,
					Salary = employee.Salary,
					Department = employee.Department

				};
				return await Task.Run(() => View("View",viewModel));

            }
	

            return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> View(UpdateEmployeeViewModel model)
		{
			var employee = await mvcDemoDBContext.Employees.FindAsync(model.Id);
			if (employee != null)
			{
				employee.Name = model.Name;
				employee.Email = model.Email;
				employee.Salary = model.Salary;
				employee.Department = model.Department;
				await mvcDemoDBContext.SaveChangesAsync(); 
				return RedirectToAction("Index");
			}
            return RedirectToAction("Index");
        }

		[HttpPost]
		public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
		{
			var employee = await mvcDemoDBContext.Employees.FindAsync(model.Id);
			if(employee != null)
			{
				mvcDemoDBContext.Employees.Remove(employee);
				await mvcDemoDBContext.SaveChangesAsync(); 
				return RedirectToAction("Index");
			}
            return RedirectToAction("Index");
        }



	}
}
