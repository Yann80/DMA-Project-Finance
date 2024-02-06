using Core.DTO;
using Core.Entities;
using Core.Repositories;
using DMA_ProjectManagement.Core.Services.Interfaces;
using Newtonsoft.Json;
using System.Net;

namespace Core.Services
{
    public class AsanaService : IAsanaService
    {
        const string API_VERSION = "api/1.0";

        private readonly HttpClient _httpClient;
        private readonly IProjectRepository _projectRepository;

        public AsanaService(IHttpClientFactory httpClientFactory,IProjectRepository projectRepository)
        {
            _httpClient = httpClientFactory.CreateClient("httpClientAsana");
            _projectRepository = projectRepository;
        }

        public async Task<List<Project>> GetAllProjectFromDb()
        {
            return await _projectRepository.GetAllSavedProjects();
        }

        public async Task<List<Project>> GetAllProjectsFromApi()
        {
            var project = new List<Project>();
            var result = await _httpClient.GetAsync($"{API_VERSION}/projects");

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var responseBody = await result.Content.ReadAsStringAsync();
                var projectJsonResponse = JsonConvert.DeserializeObject<ProjectJsonResponse>(responseBody);

                project.AddRange(projectJsonResponse.Data);
            }

            return project;
        }

        public async Task<List<TaskItem>> GetAllProjectTasksFromDb(int projectId)
        {
            return await _projectRepository.GetProjectTasks(projectId);
        }

        public async Task<List<TaskItem>> GetProjectTasksFromApi(string gid)
        {
            var tempTasks = new List<TaskInfo>();
            var tempResult = await _httpClient.GetAsync($"{API_VERSION}/projects/{gid}/tasks");
            var tasks = new List<TaskItem>();

            if (tempResult.StatusCode == HttpStatusCode.OK)
            {
                var tempResponseBody = await tempResult.Content.ReadAsStringAsync();
                var taskInfoResponse = JsonConvert.DeserializeObject<TaskInfoResponse>(tempResponseBody);

                tempTasks.AddRange(taskInfoResponse.Data);

                foreach(var taskInfo in taskInfoResponse.Data)
                {
                    var result = await _httpClient.GetAsync($"{API_VERSION}/tasks/{taskInfo.Gid}");

                    if(result.StatusCode == HttpStatusCode.OK)
                    {
                        var responseBody = await result.Content.ReadAsStringAsync();
                        var objTaskItem = JsonConvert.DeserializeObject<TaskItemDetail>(responseBody);

                        tasks.Add(objTaskItem.data);
                    }
                }
            }

            return tasks;
        }

        public Task ImportProjectTasks(int projectId)
        {
            throw new NotImplementedException();
        }
    }
}
