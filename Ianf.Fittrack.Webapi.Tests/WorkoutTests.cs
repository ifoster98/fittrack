using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Xunit;

namespace Ianf.Fittrack.Webapi.Tests
{
    public class WorkoutTests
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly string _baseUrl = "http://localhost:8080";
        private readonly DateTime _currentTime = DateTime.Now.AddDays(-1);

        private Ianf.Fittrack.Services.Dto.Workout GetWorkout() 
        {
            return new Ianf.Fittrack.Services.Dto.Workout() 
            {
                ProgramName = "Workout1",
                WorkoutTime = _currentTime,
                ProgramType = Ianf.Fittrack.Services.Dto.ProgramType.MadCow,
                Exercises = new List<Ianf.Fittrack.Services.Dto.Exercise>()
                {
                    new Ianf.Fittrack.Services.Dto.Exercise()
                    {
                        ExerciseType = Ianf.Fittrack.Services.Dto.ExerciseType.Deadlift,
                        Order = 1,
                        Sets = new List<Ianf.Fittrack.Services.Dto.Set>()
                        {
                            new Ianf.Fittrack.Services.Dto.Set()
                            {
                                PlannedReps = 5,
                                PlannedWeight = 130,
                                ActualReps = 5,
                                ActualWeight = 130,
                                Order = 1
                            }
                        }
                    }
                }
            };
        }

        [Fact]
        public async System.Threading.Tasks.Task TestAddWorkoutAsync()
        {
            // Assemble
            var newWorkout = GetWorkout();
            var url = $"{_baseUrl}/Workout"; 
            var body = JsonConvert.SerializeObject(newWorkout);
            var content = new StringContent(body,
                                    Encoding.UTF8, 
                                    "application/json");

            // Act
            var result = await _client.PostAsync(url, content);

            // Assert
            result.EnsureSuccessStatusCode();
            var resultContent = await result.Content.ReadAsStringAsync();
            Assert.Equal("{\"value\":1}", resultContent);
        }
    }
}
