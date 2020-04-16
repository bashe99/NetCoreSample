
namespace HelloServices.DataModels
{
    public class AnotherMessage : IMessage
    {
        string value { get; set; }

        public AnotherMessage()
        {
            this.value = nameof(AnotherMessage);
        }

        public AnotherMessage(string value)
        {
            this.value = value;
        }

        public string getValue()
        {
            return $"value: {this.value}";
        }
    }
}
