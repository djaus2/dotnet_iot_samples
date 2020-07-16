﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectClasses;
using GetAnIOTSampleApp.Shared;
using Newtonsoft;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GetAnIOTSampleApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SamplesController : ControllerBase
    {
        // GET: api/<SamplesController>
        //[HttpGet]
        //public IEnumerable<IGrouping<char, KeyValuePair<string, List<Project>>>> Get()
        //{
        //    var alpha = WeatherForecast.AlphaSort;
        //    return alpha;
        //}
        

        [HttpGet]
        public string Get()
        {
            var alpha = WeatherForecast.Projects;//.AlphaSort;
            string json = JsonConvert.SerializeObject(alpha);
            return json;
        }

        // GET api/<SamplesController>/5
        [HttpGet("{param}")]
        public string Get(string param)
        {
            string[] names = param.Split(new char[] { '-' });
            string DeviceName = names[0];
            string ProjectName = names[1];
            string FileType = names[2];
            List<Project> projects =
                GetAnIOTSampleApp.Shared.WeatherForecast.Projects[DeviceName];
            var xproject = from p in projects where p.Name == ProjectName select p;
            var project = xproject.FirstOrDefault();
            string path="";
            if (FileType == "SourceFile")
                path = $"{project.Path}/{project.ProjectCSFileName}";
            else if (FileType == "ProjectFile")
                path = $"{project.Path}/{project.ProjectFileName}";
            else if (FileType == "ProjectFileUse")
                path = $"Project.csproj.txt";
            string text = System.IO.File.ReadAllText(path);
            return text;


            //return "aaa\nbbb\r\nxxxx\nsss";
        }

        /*
        // GET api/<SamplesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SamplesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SamplesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SamplesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
