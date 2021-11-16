namespace Core
{
    public static class ErrorMessages
    {
        public const string FirstNameError = " Firstname field is required.";
        public const string LastNameError = " Lastname field is required.";
        public const string EmailError = " Email field is required.";
        public const string RoleNameError = " Role field is required.";
        public const string PhoneNumberError = " PhoneNumber field is required.";
        public const string AddressError = " Address field is required.";
        public const string PhotoError = " Photo field is required.";
        public const string DayOfBirthError = " DayOfBirth field is required and needs to be over age of 16.";
        public const string StudentNumberError = " Student field is required.";
        public const string PersonalNumberError = " PersonalNumber field is required.";
        public const string BigNumberError = " BigNumber field is required.";
        public const string DateError = " Date field is required.";
        public const string AppointmentTypeError = " AppointmentType field is required.";
        public const string IntakeError = " Intake field is required.";
        public const string SessionError = " Session field is required.";
        public const string PatientError = " Patient field is required.";
        public const string EmployeeError = " Employee field is required.";
        public const string AgeError = " Age field is required.";
        public const string DossierError = " Dossier field is required.";
        public const string TreatmentPlanError = " Dossier requires a treatmentplan.";
        public const string DescriptionError = " Description field is required.";
        public const string DiagnoseCodeError = " DiagnoseCode field is required.";
        public const string DiagnoseDescriptionError = " DiagnoseDescription field is required.";
        public const string SessionPerWeekError = " SessionPerWeek field is required.";
        public const string SessionDurationError = " SessionDurationError field is required.";
        public const string TextError = " Text field is required.";
        public const string AuthorError = " Author field is required.";
        public const string IdNotFound = " Item does not exist";
        public const string AppointmentNotAvailable = " appointment is not available";
        public const string SessionPerWeek = " You can't add anymore sessions this week";
        public const string NoteError = "A note is required for this Type";
    }

    public interface IResult<T>
    {
        public T Payload { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }


    public class Result<T> : IResult<T>
    {
        public T Payload { get; set; }

        public bool Success { get; set; } = true;
        public string Message { get; set; } = "";
    }
}