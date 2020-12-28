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
        public async System.Threading.Tasks.Task TestAddNewWorkoutAsync()
        {
            // Assemble
            var newWorkout = new Fittrack.Workouts.Dto.PlannedWorkout() 
            {
                ProgramName = "Workout1",
                WorkoutTime = _currentTime,
                Exercises = new List<Fittrack.Workouts.Dto.Exercise>()
                {
                    new Fittrack.Workouts.Dto.Exercise()
                    {
                        ExerciseType = Fittrack.Workouts.Dto.ExerciseType.Deadlift,
                        Order = 1,
                        Sets = new List<Fittrack.Workouts.Dto.Set>()
                        {
                            new Fittrack.Workouts.Dto.Set()
                            {
                                Reps = 5,
                                Weight = 130,
                                Order = 1
                            }
                        }
                    }
                }
            };
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
