using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoApp.DataAccessLayer;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class MyTaskController : Controller
    {
        TaskDataAccessLayer _taskDataAccessLayer = new TaskDataAccessLayer();
        // GET: MyTask
        public ActionResult Index(string searchBy, string searchValue)
        {
            
            var taskList = _taskDataAccessLayer.GetAllTheTasks();
            if (taskList.Count == 0)
            {
                TempData["InfoMessage"] = "No tasks to Diplay Currently.";
            }
            else
            {
                if (string.IsNullOrEmpty(searchValue))
                {
                    TempData["InfoMessage"] = "Provide search value.";
                    return View(taskList);
                }
                else
                {
                    if(searchBy.ToLower() == "task")
                    {
                        var searchByTask = taskList.Where(p => p.Task.ToLower().Contains(searchValue.ToLower()));
                        return View(searchByTask);
                    }
                    else if(searchBy.ToLower() == "due")
                    {
                        var searchByDueDate = taskList.Where(p => p.Task.ToLower().Contains(searchValue.ToLower()));
                        return View(searchByDueDate);
                    }
                }
            }
            
            return View(taskList);
        }

        // GET: MyTask/Details/5
        public ActionResult Details(int id)
        {
            var myTask = _taskDataAccessLayer.GetTasksByID(id).FirstOrDefault();
            try
            {
                if (myTask == null)
                {
                    TempData["InfoMessage"] = "Task does not exist";
                    return RedirectToAction("Index");
                }
                return View(myTask);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // GET: MyTask/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: MyTask/Create
        [HttpPost]
        public ActionResult Create(MyTask myTask)
        {
            bool IsInserted = false;
            
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    IsInserted = _taskDataAccessLayer.InsertTask(myTask);

                    if (IsInserted)
                    {
                        TempData["SuccessMessage"] = "Task added Successfully";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unable to save Task/Task exists already";
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // GET: MyTask/Edit/5
        public ActionResult Edit(int id)
        {
            var myTask = _taskDataAccessLayer.GetTasksByID(id).FirstOrDefault();

            if(myTask == null)
            {
                TempData["InfoMessage"] = "Task does not exist";
                return RedirectToAction("Index");
            }
            return View(myTask);
        }

        // POST: MyTask/Edit/5
        [HttpPost,ActionName("Edit")]
        public ActionResult UpdateTask(MyTask myTask)
        {
            
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    bool IsUpdated = _taskDataAccessLayer.UpdateTask(myTask);
                    if (IsUpdated)
                    {
                        TempData["SuccessMessage"] = "Task Updated Successfully";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unable to save Task/Task exists already";

                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // GET: MyTask/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var myTask = _taskDataAccessLayer.GetTasksByID(id).FirstOrDefault();

                if (myTask == null)
                {
                    TempData["ErrorMessage"] = "Task does not exist";
                    return RedirectToAction("Index");
                }
                return View(myTask);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // POST: MyTask/Delete/5
        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteConfirmation(int id)
        {
            string result = _taskDataAccessLayer.DeleteTask(id);
            
            try
            {
                // TODO: Add delete logic here
                if (result.Contains("Deleted"))
                {
                    TempData["SuccessMessage"] = result;
                }
                else
                {
                    TempData["ErrorMessage"] = result;
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        //GET : MyTask/DeleteAllTask
        public ActionResult DeleteAllTask()
        {
            return View();
        }

        //POST:
        [HttpPost]
        public ActionResult DeleteAllTask(MyTask mystask)
        {
            bool IsDeletedAll = false;
            try
            {
                IsDeletedAll = _taskDataAccessLayer.DeleteAllTasks();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }

            
        }
    }
}
