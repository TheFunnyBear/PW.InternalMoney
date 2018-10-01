namespace PW.InternalMoney.Tests
{
    public class Person
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string FullName { get { return $"{FirstName} {LastName}"; } }
        public Person(string first, string last)
        {
            FirstName = first;
            LastName = last;
        }
    }
}
