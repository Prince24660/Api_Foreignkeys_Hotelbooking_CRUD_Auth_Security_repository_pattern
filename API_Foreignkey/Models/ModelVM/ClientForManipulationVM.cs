namespace API_Foreignkey.Models.ModelVM
{
    public class ClientForManipulationVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Sex Sex { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public AddressVM Address { get; set; }
    }
}
