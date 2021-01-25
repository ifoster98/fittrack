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
        private readonly string _baseUrl = "http://localhost";
        private readonly DateTime _currentTime = DateTime.Now;

        [Fact]
        public async System.Threading.Tasks.Task TestAddPlannedWorkoutAsync()
        {
            // Assemble
            var newWorkout = new Ianf.Fittrack.Services.Dto.PlannedWorkout() 
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
                                Reps = 5,
                                Weight = 130,
                                Order = 1
                            }
                        }
                    }
                }
            };
            var url = $"{_baseUrl}/PlannedWorkout"; 
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

        [Fact]
        public async System.Threading.Tasks.Task TestAddActualWorkoutAsync()
        {
            // Assemble
            var newWorkout = new Ianf.Fittrack.Services.Dto.ActualWorkout() 
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
                                Reps = 5,
                                Weight = 130,
                                Order = 1
                            }
                        }
                    }
                }
            };
            var url = $"{_baseUrl}/ActualWorkout"; 
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
