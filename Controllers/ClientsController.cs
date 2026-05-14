using Ludo.Attributes;
using Ludo.Business;
using Ludo.Database;
using Ludo.Models;
using Ludo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Ludo.Controllers
{
    [LudoAuthorization]
    public class ClientsController : BaseController
    {
        private ClientBusiness clientBusiness { get; set; }

        public ClientsController(LudoDbContext db) : base(db)
        {
            clientBusiness = new ClientBusiness(db);
        }

        [ViewbagAssignmentAttribute]
        public IActionResult Index(int page = 1)
        {
            var model = new ClientListViewModel();
            try
            {
                if (page <= 0)
                {
                    return RedirectToAction("index", new { page = 1 });
                }

                model.Clients = clientBusiness.GetClients(null, page, PagingViewModel.PageSize, out int totalItemCount);

                model.Paging = new PagingViewModel
                {
                    Action = "index",
                    Controller = "clients",
                    CurrentPage = page,
                    PageCount = (int)Math.Ceiling((double)totalItemCount / PagingViewModel.PageSize),
                    TotalCount = totalItemCount
                };

                if (page > model.Paging.PageCount)
                    return RedirectToAction("index", new { page = 1 });


            }
            catch (Exception ex)
            {
                logBusiness.Add(new Log
                {
                    DateTime = DateTime.Now,
                    Description = ex.Message,
                    LogType = LogType.Error,
                    UserId = CurrentUser.Id
                });
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(ClientListViewModel model)
        {
            try
            {
                model.Clients = clientBusiness.GetClients(model.SearchText, 1, PagingViewModel.PageSize, out int totalItemCount);

                model.Paging = new PagingViewModel
                {
                    Action = "index",
                    Controller = "clients",
                    CurrentPage = 1,
                    PageCount = (int)Math.Ceiling((double)totalItemCount / PagingViewModel.PageSize),
                    TotalCount = totalItemCount
                };

            }
            catch (Exception ex)
            {
                logBusiness.Add(new Log
                {
                    DateTime = DateTime.Now,
                    Description = ex.Message,
                    LogType = LogType.Error,
                    UserId = CurrentUser.Id
                });
            }
            return View(model);

        }

        public IActionResult New()
        {
            return View(new EditClient());
        }


        [Route("clients/new/{id:int}")]
        [ViewbagAssignmentAttribute]
        public IActionResult New(int id)
        {
            var model = new EditClient();

            try
            {
                var client = clientBusiness.GetById(id);
                if (client != null)
                {
                    model = new EditClient()
                    {
                        Id = id,
                        Email = client.Email,
                        Firstname = client.Firstname,
                        Lastname = client.Lastname,
                        Mobile = client.Mobile,
                        Gender = client.IsMale ? Gender.Male : Gender.Female,
                    };
                }
                else if (id >= 0)
                {
                    return Redirect("/clients/new");
                }
            }
            catch (Exception ex)
            {
                logBusiness.Add(new Log
                {
                    DateTime = DateTime.Now,
                    Description = ex.Message,
                    LogType = LogType.Error,
                    UserId = CurrentUser.Id
                });
            }
            return View(model);
        }

        [HttpPost]
        [Route("clients/new/{id:int?}")]
        public IActionResult New(EditClient model)
        {
            try
            {
                if (model.Id > 0)
                {
                    var client = clientBusiness.Update(model, CurrentUser.Id, out bool mobileTaken);
                    if (client == null)
                    {
                        return Redirect("/clients/new");
                    }
                    if (mobileTaken)
                    {
                        ModelState.AddModelError("", "این موبایل قبلا استفاده شده است.");
                    }

                    if (ModelState.IsValid)
                    {
                        TempData["Result"] = "مشتری با موفقیت بروزرسانی شد.";
                    }
                }
                else
                {
                    var existingUser = clientBusiness.GetByMobile(model.Mobile);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("", "این موبایل قبلا استفاده شده است.");
                    }

                    if (ModelState.IsValid)
                    {
                        model = clientBusiness.Add(model, CurrentUser.Id);
                        TempData["Result"] = "مشتری با موفقیت ایجاد شد.";
                    }
                }

                if (ModelState.IsValid)
                    return RedirectToAction("new", "clients", new { id = model.Id });

            }
            catch (Exception ex)
            {
                logBusiness.Add(new Log
                {
                    DateTime = DateTime.Now,
                    Description = ex.Message,
                    LogType = LogType.Error,
                    UserId = CurrentUser.Id
                });
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var deleted = clientBusiness.Delete(id, CurrentUser.Id, out bool isUsed);
                if (isUsed)
                    TempData["Error"] = "به علت وجود رزرو امکان حذف این مشتری وجود ندارد.";
                else
                {
                    TempData["Result"] = "مشتری با موفقیت حذف شد.";
                }
                if (Request.Headers.Referer[0].EndsWith("/clients") || deleted)
                {
                    return RedirectToAction("index");
                }
            }
            catch(Exception ex)
            {
                logBusiness.Add(new Log
                {
                    DateTime = DateTime.Now,
                    Description = ex.Message,
                    LogType = LogType.Error,
                    UserId = CurrentUser.Id
                });
            }

            return RedirectToAction("new", "clients", new { id = id });
        }
    }
}
