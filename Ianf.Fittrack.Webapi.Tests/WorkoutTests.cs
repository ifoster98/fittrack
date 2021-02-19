using System.Runtime;
using System.Runtime.Serialization;
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
        private readonly DateTime _currentTime = DateTime.UtcNow;

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
                        ExerciseType = Ianf.Fittrack.Services.Dto.ExerciseType.BenchPress,
                        Order = 1,
                        Sets = new List<Ianf.Fittrack.Services.Dto.Set>()
                        {
                            new Ianf.Fittrack.Services.Dto.Set()
                            {
                                PlannedReps = 12,
                                PlannedWeight = 57.5M,
                                ActualReps = 12,
                                ActualWeight = 57.5M,
                                Order = 1
                            },
                            new Ianf.Fittrack.Services.Dto.Set()
                            {
                                PlannedReps = 10,
                                PlannedWeight = 57.5M,
                                ActualReps = 10,
                                ActualWeight = 57.5M,
                                Order = 2
                            },
                            new Ianf.Fittrack.Services.Dto.Set()
                            {
                                PlannedReps = 8,
                                PlannedWeight = 57.5M,
                                ActualReps = 8,
                                ActualWeight = 57.5M,
                                Order = 3
                            }
                        }
                    },
                    new Ianf.Fittrack.Services.Dto.Exercise()
                    {
                        ExerciseType = Ianf.Fittrack.Services.Dto.ExerciseType.BentOverRow,
                        Order = 2,
                        Sets = new List<Ianf.Fittrack.Services.Dto.Set>()
                        {
                            new Ianf.Fittrack.Services.Dto.Set()
                            {
                                PlannedReps = 12,
                                PlannedWeight = 52.5M,
                                ActualReps = 12,
                                ActualWeight = 52.5M,
                                Order = 1
                            },
                            new Ianf.Fittrack.Services.Dto.Set()
                            {
                                PlannedReps = 10,
                                PlannedWeight = 52.5M,
                                ActualReps = 10,
                                ActualWeight = 52.5M,
                                Order = 2
                            },
                            new Ianf.Fittrack.Services.Dto.Set()
                            {
                                PlannedReps = 8,
                                PlannedWeight = 52.5M,
                                ActualReps = 8,
                                ActualWeight = 52.5M,
                                Order = 3
                            }
                        }
                    },
                    new Ianf.Fittrack.Services.Dto.Exercise()
                    {
                        ExerciseType = Ianf.Fittrack.Services.Dto.ExerciseType.OverheadPress,
                        Order = 3,
                        Sets = new List<Ianf.Fittrack.Services.Dto.Set>()
                        {
                            new Ianf.Fittrack.Services.Dto.Set()
                            {
                                PlannedReps = 12,
                                PlannedWeight = 35,
                                ActualReps = 12,
                                ActualWeight = 35,
                                Order = 1
                            },
                            new Ianf.Fittrack.Services.Dto.Set()
                            {
                                PlannedReps = 10,
                                PlannedWeight = 35,
                                ActualReps = 10,
                                ActualWeight = 35,
                                Order = 2
                            },
                            new Ianf.Fittrack.Services.Dto.Set()
                            {
                                PlannedReps = 8,
                                PlannedWeight = 35,
                                ActualReps = 8,
                                ActualWeight = 35,
                                Order = 3
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
            newWorkout.WorkoutTime = _currentTime;
            Console.WriteLine(_currentTime.ToString("yyyy-MM-ddThh:mm:ss"));
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

        [Fact]
        public async System.Threading.Tasks.Task TestGetWorkoutForDate()
        {
            // Assemble
            var newWorkout = GetWorkout();
            var url = $"{_baseUrl}/Workout"; 
            var body = JsonConvert.SerializeObject(newWorkout);
            var content = new StringContent(body,
                                    Encoding.UTF8, 
                                    "application/json");
            var result = await _client.PostAsync(url, content);
            result.EnsureSuccessStatusCode();

            // Act
            url = $"{_baseUrl}/Workout/{this._currentTime.ToString("yyyy-MM-dd")}"; 
            result = await _client.GetAsync(url);

            // Assert
            result.EnsureSuccessStatusCode();
            var resultContent = await result.Content.ReadAsStringAsync();
            var workout = JsonConvert.DeserializeObject<Ianf.Fittrack.Services.Dto.Workout>(resultContent);
            Assert.Equal(57.5M, workout.Exercises[0].Sets[0].PlannedWeight);
        }
    }
}
