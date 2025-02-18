﻿using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RegistaMaster.Application.Repositories;
using RegistaMaster.Domain.DTOModels.Entities.UserModel;
using RegistaMaster.Domain.Entities;

namespace RegistaMaster.WebApp.Controllers;

public class UserController : Controller
{
    private readonly IUnitOfWork uow;
    public UserController(IUnitOfWork _uow)
    {
        uow = _uow;
    }
    public async Task<object> GetList(DataSourceLoadOptions options)

    {
        var models = await uow.UserRepository.GetList();
        return DataSourceLoader.Load(models, options);
    }
    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> AddUser(string values)
    {
        try
        {
            var model = JsonConvert.DeserializeObject<User>(values);
            await uow.UserRepository.AddUser(model);
            return Ok();
        }
        catch (Exception e)
        {

            throw e;
        }
    }
    public async Task<string> UserEdit(int Key, string values)
    {
        try
        {
            var size = await uow.Repository.GetById<User>(Key);
            JsonConvert.PopulateObject(values, size);
            uow.UserRepository.Update(size);
            await uow.SaveChanges();
            return "1";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public async Task<string> DeleteUser(int Key)
    {
        try
        {
            await uow.Repository.Delete<User>(Key);
            await uow.SaveChanges();
            return "1";

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [HttpGet]
    public async Task<IActionResult> UserDetail()
    {
        try
        {
            var model = await uow.UserRepository.GetById<User>(uow.GetSession().ID);

            var userdetail = new UserDetailDto()
            {
                ID = model.ID.ToString(),
                Username = model.Username,
                Name = model.Name,
                Surname = model.Surname,
                Parola = model.Password,
                Email = model.Email,
            };

            return View(userdetail);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [HttpPost]
    public async Task<IActionResult> UserDetail(UserDetailDto userDetail)
    {
        try
        {
            var model = await uow.UserRepository.GetById<User>(uow.GetSession().ID);

            model.Username = userDetail.Username;
            model.Name = userDetail.Name;
            model.Password = userDetail.Parola;
            model.Email = userDetail.Email;


            uow.UserRepository.Update<User>(model);
            await uow.SaveChanges();


            return RedirectToAction("Login", "Security");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new UserDetailDto();
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Create(UserDetailDto userDetailDto)
    {
        var user = new User()
        {
            Name = userDetailDto.Name,
            Surname = userDetailDto.Surname,
            Username = userDetailDto.Username,
            Email = userDetailDto.Email,
            Password = userDetailDto.Parola,
            Image = userDetailDto.Image
        };

        await uow.UserRepository.AddUser(user);

        await uow.SaveChanges();

        return RedirectToAction("Index", "User");

    }
    public async Task<string> FileUpload(IFormFile FileUrl)
    {
        try
        {
            string fileName = "";
            if (FileUrl != null)
            {
                string extension = Path.GetExtension(FileUrl.FileName);
                Guid guidFile = Guid.NewGuid();
                fileName = "customer" + guidFile + extension;
                var path = Path.Combine("wwwroot/Modernize/Img/ProfilePhotos/", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    FileUrl.CopyTo(stream);
                }
            }
            return fileName;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}
