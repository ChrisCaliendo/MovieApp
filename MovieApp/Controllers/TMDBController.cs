using Microsoft.AspNetCore.Components;
using MovieApp.Interfaces;
using MovieApp.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MovieApp.Dto;
using MovieApp.Repositories;
using MovieApp.Wrapper;

namespace MovieApp.Controllers
{

    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]

    public class TMDBControllern : Controller
    {

        private readonly TMDBWrapper tMDBWrapper;
    }
}
