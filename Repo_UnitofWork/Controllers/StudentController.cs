using Microsoft.AspNetCore.Mvc;
using Repo_UnitofWork.Models;
using Repo_UnitofWork.Repository;


namespace Repo_UnitofWork.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;

        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        // Hiển thị danh sách Student
        public IActionResult Index()
        {
            var students = _studentRepository.GetAllStudents();
            return View(students);
        }

        // Hiển thị form thêm mới
        public IActionResult Create()
        {
            return View();
        }

        // Xử lý thêm mới
        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _studentRepository.AddStudent(student);
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // Hiển thị form sửa
        public IActionResult Edit(int id)
        {
            var student = _studentRepository.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // Xử lý sửa
        [HttpPost]
        public IActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                _studentRepository.UpdateStudent(student);
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // Hiển thị xác nhận xóa
        public IActionResult Delete(int id)
        {
            var student = _studentRepository.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // Xử lý xóa
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _studentRepository.DeleteStudent(id);
            return RedirectToAction("Index");
        }
    }
}
