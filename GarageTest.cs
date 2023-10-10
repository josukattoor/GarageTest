using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyGarage;

namespace GarageTest
{
    [TestClass]
    public class GarageTests
    {
        [TestMethod]
        public void Park_ShouldAddVehicleToGarage()
        {
            // Arrange
            var garage = new Garage<Vehicle>(2);
            var vehicle1 = new Vehicle { RegistrationNumber = "ABC123" };
            var vehicle2 = new Vehicle { RegistrationNumber = "def456" };

            // Act
            var result1 = garage.Park(vehicle1);
            var result2 = garage.Park(vehicle2);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.AreEqual(2, garage.Count);
        }

        [TestMethod]
        public void Park_ShouldNotAddDuplicateVehicle()
        {
            // Arrange
            var garage = new Garage<Vehicle>(2);
            var vehicle1 = new Vehicle { RegistrationNumber = "ABC123" };
            var vehicle2 = new Vehicle { RegistrationNumber = "ABC123" };

            // Act
            var result1 = garage.Park(vehicle1);
            var result2 = garage.Park(vehicle2);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.AreEqual(1, garage.Count);
        }

        [TestMethod]
        public void Park_ShouldNotExceedCapacity()
        {
            // Arrange
            var garage = new Garage<Vehicle>(1);
            var vehicle1 = new Vehicle { RegistrationNumber = "ABC123" };
            var vehicle2 = new Vehicle { RegistrationNumber = "DEF456" };

            // Act
            var result1 = garage.Park(vehicle1);
            var result2 = garage.Park(vehicle2);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.AreEqual(1, garage.Count);
        }

        [TestMethod]
        public void FindVehicleByRegistrationNumber_ShouldReturnCorrectVehicle()
        {
            // Arrange
            var garage = new Garage<Vehicle>(2);
            var vehicle1 = new Vehicle { RegistrationNumber = "ABC123" };
            var vehicle2 = new Vehicle { RegistrationNumber = "DEF456" };

            // Act
            garage.Park(vehicle1);
            garage.Park(vehicle2);
            var foundVehicle = garage.FindVehicleByRegistrationNumber("abc123");

            // Assert
            Assert.IsNotNull(foundVehicle);
            Assert.AreEqual("ABC123", foundVehicle.RegistrationNumber);
        }

        [TestMethod]
        public void FindVehicleByRegistrationNumber_ShouldReturnNullIfNotFound()
        {
            // Arrange
            var garage = new Garage<Vehicle>(1);
            var vehicle1 = new Vehicle { RegistrationNumber = "ABC123" };

            // Act
            garage.Park(vehicle1);
            var foundVehicle = garage.FindVehicleByRegistrationNumber("XYZ789");

            // Assert
            Assert.IsNull(foundVehicle);
        }

        [TestMethod]
        public void SearchVehicles_ShouldReturnMatchingVehicles()
        {

            // Arrange
            var garage = new Garage<Vehicle>(3);
            var vehicle1 = new Vehicle { RegistrationNumber = "ABC123", Color = "Black", NumberOfWheels = 4 };
            var vehicle2 = new Vehicle { RegistrationNumber = "DEF456", Color = "Red", NumberOfWheels = 2 };
            var vehicle3 = new Vehicle { RegistrationNumber = "GHI789", Color = "Black", NumberOfWheels = 2 };

            // Act
            garage.Park(vehicle1);
            garage.Park(vehicle2);
            garage.Park(vehicle3);
            var matchingVehicles = garage.SearchVehicles(v => v.Color == "Black" && v.NumberOfWheels == 4);

            // Assert
            CollectionAssert.Contains(matchingVehicles.ToList(), vehicle1);
            CollectionAssert.DoesNotContain(matchingVehicles.ToList(), vehicle2);
            CollectionAssert.DoesNotContain(matchingVehicles.ToList(), vehicle3);
        }

        [TestMethod]
        public void SearchVehicles_ShouldReturnNoMatchingVehicles()
        {
            // Arrange
            var garage = new Garage<Vehicle>(3);
            var vehicle1 = new Vehicle { RegistrationNumber = "ABC123", Color = "Red", NumberOfWheels = 4 };
            var vehicle2 = new Vehicle { RegistrationNumber = "DEF456", Color = "Blue", NumberOfWheels = 2 };
            var vehicle3 = new Vehicle { RegistrationNumber = "GHI789", Color = "Green", NumberOfWheels = 4 };

            // Act
            garage.Park(vehicle1);
            garage.Park(vehicle2);
            garage.Park(vehicle3);
            var matchingVehicles = garage.SearchVehicles(v => v.Color == "Black");

            // Assert
            Assert.IsFalse(matchingVehicles.Any());
        }


        [TestMethod]
        public void Remove_ShouldRemoveVehicleFromGarage()
        {
            // Arrange
            var garage = new Garage<Vehicle>(2);
            var vehicle1 = new Vehicle { RegistrationNumber = "ABC123" };
            var vehicle2 = new Vehicle { RegistrationNumber = "DEF456" };

            // Act
            garage.Park(vehicle1);
            garage.Park(vehicle2);
            var result = garage.Remove(vehicle1);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, garage.Count);
        }

        [TestMethod]
        public void Remove_ShouldNotRemoveNonExistentVehicle()
        {
            // Arrange
            var garage = new Garage<Vehicle>(1);
            var vehicle1 = new Vehicle { RegistrationNumber = "ABC123" };
            var vehicle2 = new Vehicle { RegistrationNumber = "DEF456" };

            // Act
            garage.Park(vehicle1);
            var result = garage.Remove(vehicle2);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(1, garage.Count);
        }
        [TestMethod]
        public void ListVehicleTypes_ShouldReturnEmptyDictionary_WhenNoVehiclesParked()
        {
            // Arrange
            var garage = new Garage<Vehicle>(5);

            // Act
            var typeCounts = garage.ListVehicleTypes();

            // Assert
            Assert.AreEqual(0, typeCounts.Count);
        }

        [TestMethod]
        public void ListVehicleTypes_ShouldReturnCorrectCounts_WhenVehiclesAreParked()
        {
            // Arrange
            var garage = new Garage<Vehicle>(5);
            var car1 = new Car { RegistrationNumber = "ABC123" };
            var car2 = new Car { RegistrationNumber = "DEF456" };
            var motorcycle1 = new Motorcycle { RegistrationNumber = "GHI789" };

            garage.Park(car1);
            garage.Park(car2);
            garage.Park(motorcycle1);

            // Act
            var typeCounts = garage.ListVehicleTypes();

            // Assert
            Assert.AreEqual(2, typeCounts.Count);
            Assert.AreEqual(2, typeCounts[typeof(Car)]);
            Assert.AreEqual(1, typeCounts[typeof(Motorcycle)]);
        }
        [TestMethod]
        public void ExtendCapacity_ShouldIncreaseCapacity()
        {
            // Arrange
            int initialCapacity = 5;
            int newCapacity = 10;
            var garage = new Garage<Vehicle>(initialCapacity);

            // Act
            bool result = garage.ExtendCapacity(newCapacity);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(newCapacity, garage.Capacity);
        }

        [TestMethod]
        public void ReduceCapacity_ShouldReduceCapacity()
        {
            // Arrange
            int initialCapacity = 10;
            int newCapacity = 5;
            var garage = new Garage<Vehicle>(initialCapacity);
            // Park some vehicles
            for (int i = 0; i < 5; i++)
            {
                garage.Park(new Car { RegistrationNumber = $"CAR{i}", Color = "Red", NumberOfWheels = 4, NumberOfDoors = 4 });
            }

            // Act
            bool result = garage.ReduceCapacity(newCapacity);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(newCapacity, garage.Capacity);
            // Check if the number of parked vehicles is still within the new capacity
            Assert.AreEqual(newCapacity, garage.Count);
        }
    
    }



}