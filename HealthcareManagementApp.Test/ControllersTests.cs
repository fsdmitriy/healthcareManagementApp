using HealthcareManagementApp.Controllers;
using HealthcareManagementApp.Models;
using HealthcareManagementApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;

namespace HealthcareManagementApp.Test;

[TestFixture]
public class ControllersTests
{
    private Mock<IRepository<Appointment>> _appointmentRepositoryMock;
    private Mock<IRepository<Patient>> _patientRepositoryMock;
    private Mock<IRepository<Prescription>> _prescriptionRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _appointmentRepositoryMock = new Mock<IRepository<Appointment>>();
        _patientRepositoryMock = new Mock<IRepository<Patient>>();
        _prescriptionRepositoryMock = new Mock<IRepository<Prescription>>();
    }

    [Test]
    public async Task AppointmentController_GetAll_ReturnsAllAppointments()
    {
        // Arrange
        var expectedAppointments = new List<Appointment>
        {
            new Appointment { Id = 1, DateTime = DateTime.Now, DoctorName = "Dr. John Doe", Location = "123 Main St" },
            new Appointment { Id = 2, DateTime = DateTime.Now.AddDays(1), DoctorName = "Dr. Jane Smith", Location = "456 2nd St" }
        };

        _appointmentRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedAppointments);
        var controller = new AppointmentController(_appointmentRepositoryMock.Object, null!);

        // Act
        var result = await controller.GetAll();

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = new OkObjectResult(result);

        Assert.That(((OkObjectResult)okResult.Value).Value, Is.InstanceOf<List<Appointment>>());
        var appointments = (List<Appointment>)((OkObjectResult)okResult.Value).Value;

        Assert.That(appointments, Is.EqualTo(expectedAppointments));
    }

    [Test]
    public async Task AppointmentController_GetById_ReturnsAppointment()
    {
        // Arrange
        var expectedAppointment = new Appointment { Id = 1, DateTime = DateTime.Now, DoctorName = "Dr. John Doe", Location = "123 Main St" };

        _appointmentRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(expectedAppointment);
        var controller = new AppointmentController(_appointmentRepositoryMock.Object, null!);

        // Act
        var result = await controller.GetById(1);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = new OkObjectResult(result);

        Assert.That(((OkObjectResult)okResult.Value).Value, Is.InstanceOf<Appointment>());
        var appointment = (Appointment)((OkObjectResult)okResult.Value).Value;

        Assert.That(appointment, Is.EqualTo(expectedAppointment));
    }

    [Test]
    public async Task AppointmentController_GetById_ReturnsNotFound()
    {
        // Arrange
        _appointmentRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Appointment)null);
        var controller = new AppointmentController(_appointmentRepositoryMock.Object, null!);

        // Act
        var result = await controller.GetById(1);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task AppointmentController_Create_ReturnsCreated()
    {
        // Arrange
        var appointment = new Appointment { Id = 1, DateTime = DateTime.Now, DoctorName = "Dr. John Doe", Location = "123 Main St" };
        _appointmentRepositoryMock.Setup(r => r.AddAsync(appointment)).Returns(Task.CompletedTask);
        var controller = new AppointmentController(_appointmentRepositoryMock.Object, null!);

        // Act
        var result = await controller.Create(appointment);

        // Assert
        Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
        var createdResult = new CreatedAtActionResult("Create", "AppointmentController", new { id = appointment.Id }, appointment);

        Assert.That(createdResult.Value, Is.EqualTo(appointment));
    }

    [Test]
    public async Task AppointmentController_Update_ReturnsNoContent()
    {
        // Arrange
        var appointment = new Appointment { Id = 1, DateTime = DateTime.Now, DoctorName = "Dr. John Doe", Location = "123 Main St" };
        _appointmentRepositoryMock.Setup(r => r.UpdateAsync(appointment)).Returns(Task.CompletedTask);
        var controller = new AppointmentController(_appointmentRepositoryMock.Object, null!);

        // Act
        var result = await controller.Update(1, appointment);

        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task AppointmentController_Update_ReturnsBadRequest()
    {
        // Arrange
        var appointment = new Appointment { Id = 1, DateTime = DateTime.Now, DoctorName = "Dr. John Doe", Location = "123 Main St" };
        var controller = new AppointmentController(_appointmentRepositoryMock.Object, null!);

        // Act
        var result = await controller.Update(2, appointment);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestResult>());
    }

    [Test]
    public async Task AppointmentController_Delete_ReturnsNoContent()
    {
        // Arrange
        _appointmentRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Appointment { Id = 1, DateTime = DateTime.Now, DoctorName = "Dr. John Doe", Location = "123 Main St" });
        _appointmentRepositoryMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);
        var controller = new AppointmentController(_appointmentRepositoryMock.Object, null!);

        // Act
        var result = await controller.Delete(1);

        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task AppointmentController_Delete_ReturnsNotFound()
    {
        // Arrange
        _appointmentRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Appointment)null);
        var controller = new AppointmentController(_appointmentRepositoryMock.Object, null!);

        // Act
        var result = await controller.Delete(1);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task AppointmentController_GetCoordinates_ReturnsNotFound()
    {
        // Arrange
        _appointmentRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Appointment)null);
        var controller = new AppointmentController(_appointmentRepositoryMock.Object, null!);

        // Act
        var result = await controller.GetCoordinates(1);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task PatientsController_GetAll_ReturnsAllPatients()
    {
        // Arrange
        var expectedPatients = new List<Patient>
        {
            new Patient { Id = 1, FirstName = "John", LastName = "Doe" },
            new Patient { Id = 2, FirstName = "Jane", LastName = "Smith" }
        };

        _patientRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedPatients);
        var controller = new PatientsController(_patientRepositoryMock.Object);

        // Act
        var result = await controller.GetAllAsync();

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = new OkObjectResult(result);

        Assert.That(((OkObjectResult)okResult.Value).Value, Is.InstanceOf<List<Patient>>());
        var patients = (List<Patient>)((OkObjectResult)okResult.Value).Value;

        Assert.That(patients, Is.EqualTo(expectedPatients));
    }

    [Test]
    public async Task PatientsController_GetById_ReturnsPatient()
    {
        // Arrange
        var expectedPatient = new Patient { Id = 1, FirstName = "John", LastName = "Doe" };

        _patientRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(expectedPatient);
        var controller = new PatientsController(_patientRepositoryMock.Object);

        // Act
        var result = await controller.GetById(1);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = new OkObjectResult(result);

        Assert.That(((OkObjectResult)okResult.Value).Value, Is.InstanceOf<Patient>());
        var patient = (Patient)((OkObjectResult)okResult.Value).Value;

        Assert.That(patient, Is.EqualTo(expectedPatient));
    }

    [Test]
    public async Task PatientsController_GetById_ReturnsNotFound()
    {
        // Arrange
        _patientRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Patient)null);
        var controller = new PatientsController(_patientRepositoryMock.Object);

        // Act
        var result = await controller.GetById(1);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task PatientsController_Create_ReturnsCreated()
    {
        // Arrange
        var patient = new Patient { Id = 1, FirstName = "John", LastName = "Doe" };
        _patientRepositoryMock.Setup(r => r.AddAsync(patient)).Returns(Task.CompletedTask);
        var controller = new PatientsController(_patientRepositoryMock.Object);

        // Act
        var result = await controller.Create(patient);

        // Assert
        Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
        var createdResult = new CreatedAtActionResult("Create", "PatientsController", new { id = patient.Id }, patient);

        Assert.That(createdResult.Value, Is.EqualTo(patient));
    }

    [Test]
    public async Task PatientsController_Update_ReturnsNoContent()
    {
        // Arrange
        var patient = new Patient { Id = 1, FirstName = "John", LastName = "Doe" };
        _patientRepositoryMock.Setup(r => r.UpdateAsync(patient)).Returns(Task.CompletedTask);
        var controller = new PatientsController(_patientRepositoryMock.Object);

        // Act
        var result = await controller.Update(1, patient);

        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task PatientsController_Update_ReturnsBadRequest()
    {
        // Arrange
        var patient = new Patient { Id = 1, FirstName = "John", LastName = "Doe" };
        var controller = new PatientsController(_patientRepositoryMock.Object);

        // Act
        var result = await controller.Update(2, patient);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestResult>());
    }

    [Test]
    public async Task PatientsController_Delete_ReturnsNoContent()
    {
        // Arrange
        _patientRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Patient { Id = 1, FirstName = "John", LastName = "Doe" });
        _patientRepositoryMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);
        var controller = new PatientsController(_patientRepositoryMock.Object);

        // Act
        var result = await controller.Delete(1);

        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task PatientsController_Delete_ReturnsNotFound()
    {
        // Arrange
        _patientRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Patient)null);
        var controller = new PatientsController(_patientRepositoryMock.Object);

        // Act
        var result = await controller.Delete(1);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task PrescriptionController_GetAll_ReturnsAllPrescriptions()
    {
        // Arrange
        var expectedPrescriptions = new List<Prescription>
        {
            new Prescription { Id = 1, Name = "Medication 1" },
            new Prescription { Id = 2, Name = "Medication 2" }
        };

        _prescriptionRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedPrescriptions);
        var controller = new PrescriptionController(_prescriptionRepositoryMock.Object);

        // Act
        var result = await controller.GetAll();

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = new OkObjectResult(result);

        Assert.That(((OkObjectResult)okResult.Value).Value, Is.InstanceOf<List<Prescription>>());
        var prescriptions = (List<Prescription>)((OkObjectResult)okResult.Value).Value;

        Assert.That(prescriptions, Is.EqualTo(expectedPrescriptions));
    }

    [Test]
    public async Task PrescriptionController_GetById_ReturnsPrescription()
    {
        // Arrange
        var expectedPrescription = new Prescription { Id = 1, Name = "Medication 1" };

        _prescriptionRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(expectedPrescription);
        var controller = new PrescriptionController(_prescriptionRepositoryMock.Object);

        // Act
        var result = await controller.GetById(1);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = new OkObjectResult(result);

        Assert.That(((OkObjectResult)okResult.Value).Value, Is.InstanceOf<Prescription>());
        var prescription = (Prescription)((OkObjectResult)okResult.Value).Value;

        Assert.That(prescription, Is.EqualTo(expectedPrescription));
    }

    [Test]
    public async Task PrescriptionController_GetById_ReturnsNotFound()
    {
        // Arrange
        _prescriptionRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Prescription)null);
        var controller = new PrescriptionController(_prescriptionRepositoryMock.Object);

        // Act
        var result = await controller.GetById(1);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task PrescriptionController_Create_ReturnsCreated()
    {
        // Arrange
        var prescription = new Prescription { Id = 1, Name = "Medication 1" };
        _prescriptionRepositoryMock.Setup(r => r.AddAsync(prescription)).Returns(Task.CompletedTask);
        var controller = new PrescriptionController(_prescriptionRepositoryMock.Object);

        // Act
        var result = await controller.Create(prescription);

        // Assert
        Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
        var createdResult = new CreatedAtActionResult("Create", "", new { id = prescription.Id }, prescription);


        Assert.That(createdResult.Value, Is.EqualTo(prescription));
    }

    [Test]
    public async Task PrescriptionController_Update_ReturnsNoContent()
    {
        // Arrange
        var prescription = new Prescription { Id = 1, Name = "Medication 1" };
        _prescriptionRepositoryMock.Setup(r => r.UpdateAsync(prescription)).Returns(Task.CompletedTask);
        var controller = new PrescriptionController(_prescriptionRepositoryMock.Object);

        // Act
        var result = await controller.Update(1, prescription);

        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task PrescriptionController_Update_ReturnsBadRequest()
    {
        // Arrange
        var prescription = new Prescription { Id = 1, Name = "Medication 1" };
        var controller = new PrescriptionController(_prescriptionRepositoryMock.Object);

        // Act
        var result = await controller.Update(2, prescription);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestResult>());
    }

    [Test]
    public async Task PrescriptionController_Delete_ReturnsNoContent()
    {
        // Arrange
        _prescriptionRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Prescription { Id = 1, Name = "Medication 1" });
        _prescriptionRepositoryMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);
        var controller = new PrescriptionController(_prescriptionRepositoryMock.Object);

        // Act
        var result = await controller.Delete(1);

        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task PrescriptionController_Delete_ReturnsNotFound()
    {
        // Arrange
        _prescriptionRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Prescription)null);
        var controller = new PrescriptionController(_prescriptionRepositoryMock.Object);

        // Act
        var result = await controller.Delete(1);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }
}
